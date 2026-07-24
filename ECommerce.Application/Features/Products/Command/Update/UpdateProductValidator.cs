using ECommerce.Application.Features.Products.Command.Create;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Features.Products.Command.Update
{

    public class UpdateProductValidator : AbstractValidator<UpdateProductCommand>
    {

        public UpdateProductValidator()
        {
            RuleFor(p => p.name)
                .NotEmpty().WithMessage("Product name can not be empty")
                .MaximumLength(100).WithMessage("Product name can not be longer than 100 symbols");

            RuleFor(p => p.description)
                .MaximumLength(500).WithMessage("Product description can not be longer than 500 symbols");

            RuleFor(p => p.price.Amount)
                 .GreaterThanOrEqualTo(0).WithMessage("Price can not be negative");

            RuleFor(p => p.stockQuantity)
                 .GreaterThanOrEqualTo(0).WithMessage("Quantity can not be negative");


            RuleFor(p => p.categoryId)
               .NotEmpty().WithMessage("Category ID can not be empty")
               .Must(id => id != Guid.Empty).WithMessage("Category ID must be a valid GUID");

        }
    }
}
