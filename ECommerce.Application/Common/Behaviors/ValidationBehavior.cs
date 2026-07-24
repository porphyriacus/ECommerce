using ECommerce.Application.Common.Behaviors.Errors;
using ECommerce.Application.Common.Models;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Common.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
           where TRequest : IRequest<TResponse>
           where TResponse : Result
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            if (!_validators.Any())
                return await next();

            var failures = _validators
                .Select(v => v.Validate(request))
                .SelectMany(r => r.Errors)
                .Where(f => f != null)
                .ToList();


            if (!failures.Any())
                return await next();

            var errorMessages = string.Join("; ", failures.Select(f => f.ErrorMessage));
            var error = Error.Validation("ValidationError", errorMessages);

            return CreateFailureResult(error);
        }

        private static TResponse CreateFailureResult(Error error)
        {
            var responseType = typeof(TResponse);

            if (responseType.IsGenericType && responseType.GetGenericTypeDefinition() == typeof(Result<>))
            {
                var valueType = responseType.GetGenericArguments()[0];
                var resultType = typeof(Result<>).MakeGenericType(valueType);
                var failureMethod = resultType.GetMethod("Failure", new[] { typeof(Error) });

                if (failureMethod is not null)
                {
                    var failureResult = failureMethod.Invoke(null, new[] { error });
                    return (TResponse)failureResult!;
                }
            }

            if (responseType == typeof(Result))
            {
                return (TResponse)(object)Result.Failure(error);
            }

            throw new InvalidOperationException($"Тип {responseType.Name} не поддерживается. TResponse должен быть Result или Result<T>");
        }
    }
}
