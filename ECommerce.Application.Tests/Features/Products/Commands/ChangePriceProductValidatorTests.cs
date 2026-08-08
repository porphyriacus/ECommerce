using ECommerce.Application.Features.Products.Command.ChangePrice;
using ECommerce.Domain.ValueObjects;
using FluentValidation.TestHelper;
using System;
using Xunit;

namespace ECommerce.Application.Tests.Features.Products.Commands
{
    public class ChangePriceProductValidatorTests
    {
        private readonly ChangePriceProductValidator _validator;

        public ChangePriceProductValidatorTests()
        {
            _validator = new ChangePriceProductValidator();
        }

        [Fact]
        public void Validate_ValidPrice_NoError()
        {
            // Arrange
            var command = new ChangePriceProductCommand(
                Guid.NewGuid(),
                new Money(100, "USD")
            );

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(c => c.money.Amount);
        }

        [Fact]
        public void Validate_ZeroPrice_NoError()
        {
            var command = new ChangePriceProductCommand(
                Guid.NewGuid(),
                new Money(0, "USD")
            );

            var result = _validator.TestValidate(command);

            result.ShouldNotHaveValidationErrorFor(c => c.money.Amount);
        }

        [Fact]
        public void Validate_NegativePrice_ThrowsDomainException()
        {
            Assert.Throws<ArgumentException>(() =>
                new ChangePriceProductCommand(
                    Guid.NewGuid(),
                    new Money(-100, "USD")
                )
            );
        }

        [Fact]
        public void Validate_ValidCommand_NoErrors()
        {
            var command = new ChangePriceProductCommand(
                Guid.NewGuid(),
                new Money(100, "USD")
            );

            var result = _validator.TestValidate(command);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
