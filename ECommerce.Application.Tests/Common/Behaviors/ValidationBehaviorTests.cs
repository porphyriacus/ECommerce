using ECommerce.Application.Common.Behaviors;
using ECommerce.Application.Common.Behaviors.Errors;
using ECommerce.Application.Common.Models;
using ECommerce.Application.Features.Products.Command.Create;
using ECommerce.Domain.ValueObjects;
using FluentAssertions;
using FluentValidation;
using MediatR;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Tests.Common.Behaviors
{
    public class ValidationBehaviorTests
    {
        [Fact]
        public async Task Handle_ValidRequest_CallsNext()
        {
            //
            var validator = new CreateProductValidator();
            var validators = new List<IValidator<CreateProductCommand>> { validator };
            var behavior = new ValidationBehavior<CreateProductCommand, Result>(validators);

            var request = new CreateProductCommand(
                "Valid Name",
                null,
                new Money(100, "USD"),
                10,
                Guid.NewGuid()
            );
          
            var nextMock = new Mock<RequestHandlerDelegate<Result>>();
            nextMock
                .Setup(n => n())
                .ReturnsAsync(Result.Ok);

            // 
            var result = await behavior.Handle(request, nextMock.Object, CancellationToken.None);

            // 
            nextMock.Verify(n => n(), Times.Once);
            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_InvalidRequest_ReturnsValidationError_AndDoesNotCallNext()
        {
            // 
            var validator = new CreateProductValidator();
            var validators = new List<IValidator<CreateProductCommand>> { validator };
            var behavior = new ValidationBehavior<CreateProductCommand, Result>(validators);

            var request = new CreateProductCommand(
                "", // пустое имя
                null,
                new Money(0, "USD"), // отрицательная цена
                -5, // отриц количество
                Guid.Empty // пустой GUID
            );

            //
            var nextMock = new Mock<RequestHandlerDelegate<Result>>();
            nextMock
                .Setup(n => n())
                .ReturnsAsync(Result.Ok);

            // 
            var result = await behavior.Handle(request, nextMock.Object, CancellationToken.None);

            // 
            nextMock.Verify(n => n(), Times.Never); // некст НЕ вызывается
            result.IsFailure.Should().BeTrue();
            result.Error.Type.Should().Be(ErrorType.Validation);

            result.Error.Message.Should().Contain("Product name can not be empty");
            result.Error.Message.Should().Contain("Quantity can not be negative");
            result.Error.Message.Should().Contain("Category ID must be a valid GUID");

        }
    }
}
