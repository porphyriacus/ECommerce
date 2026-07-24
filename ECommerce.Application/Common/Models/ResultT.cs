using ECommerce.Application.Common.Behaviors.Errors;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Common.Models
{
    public class Result<T> : Result
    {
        private readonly T _value;

        private Result(T value) : base(true, Error.None)
        {
            _value = value;
        }

        private Result(Error error) : base(false, error)
        {
            _value = default;
        }

        public T Value => IsSuccess
            ? _value!
            : throw new InvalidOperationException("Value cannot be accessed when IsFailure");

        public static Result<T> Ok(T value) => new(value);
        public static Result<T> Failure(Error error) => new(error);

        public static implicit operator Result<T>(Error error) => Failure(error);
        public static implicit operator Result<T>(T value) => Ok(value);
    }
}
