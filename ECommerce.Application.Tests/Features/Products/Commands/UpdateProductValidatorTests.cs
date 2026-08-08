using ECommerce.Application.Features.Products.Command.Update;
using ECommerce.Domain.ValueObjects;
using FluentValidation.TestHelper;
using System;
using Xunit;

namespace ECommerce.Application.Tests.Features.Products.Commands
{
    public class UpdateProductValidatorTests
    {
        private readonly UpdateProductValidator _validator;

        public UpdateProductValidatorTests()
        {
            _validator = new UpdateProductValidator();
        }

        [Fact]
        public void Validate_EmptyName_HasError()
        {
            var command = new UpdateProductCommand(
                Guid.NewGuid(),
                "",
                "Description",
                new Money(100, "USD"),
                10
            );

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(c => c.name);
        }

        [Fact]
        public void Validate_NameTooLong_HasError()
        {
            var command = new UpdateProductCommand(
                Guid.NewGuid(),
                new string('a', 101),
                "Description",
                new Money(100, "USD"),
                10
            );

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(c => c.name);
        }

        [Fact]
        public void Validate_DescriptionTooLong_HasError()
        {
            var command = new UpdateProductCommand(
                Guid.NewGuid(),
                "Valid Name",
                new string('a', 501),
                new Money(100, "USD"),
                10
            );

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(c => c.description);
        }

        [Fact]
        public void Validate_NegativeStockQuantity_HasError()
        {
            var command = new UpdateProductCommand(
                Guid.NewGuid(),
                "Valid Name",
                "Description",
                new Money(100, "USD"),
                -5
            );

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(c => c.stockQuantity);
        }

        [Fact]
        public void Validate_ValidPrice_NoError()
        {
            var command = new UpdateProductCommand(
                Guid.NewGuid(),
                "Valid Name",
                "Description",
                new Money(100, "USD"),
                10
            );

            var result = _validator.TestValidate(command);

            result.ShouldNotHaveValidationErrorFor(c => c.price.Amount);
        }

        [Fact]
        public void Validate_ZeroPrice_NoError()
        {
            var command = new UpdateProductCommand(
                Guid.NewGuid(),
                "Valid Name",
                "Description",
                new Money(0, "USD"),
                10
            );
            var result = _validator.TestValidate(command);

            result.ShouldNotHaveValidationErrorFor(c => c.price.Amount);
        }

        [Fact]
        public void Validate_NegativePrice_ThrowsDomainException()
        {
            Assert.Throws<ArgumentException>(() =>
                new UpdateProductCommand(
                    Guid.NewGuid(),
                    "Valid Name",
                    "Description",
                    new Money(-100, "USD"),
                    10
                )
            );
        }

        [Fact]
        public void Validate_NullDescription_NoError()
        {
            var command = new UpdateProductCommand(
                Guid.NewGuid(),
                "Valid Name",
                null,
                new Money(100, "USD"),
                10
            );

            var result = _validator.TestValidate(command);

            result.ShouldNotHaveValidationErrorFor(c => c.description);
        }

        [Fact]
        public void Validate_ValidCommand_NoErrors()
        {
            var command = new UpdateProductCommand(
                Guid.NewGuid(),
                "Valid Name",
                "Valid Description",
                new Money(100, "USD"),
                10
            );

            var result = _validator.TestValidate(command);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}