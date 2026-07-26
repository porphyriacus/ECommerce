using ECommerce.Application.Features.Products.Command.Create;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Features.Products.Command.ChangeDescription
{
    public class ChangeDescriptionProductValidator : AbstractValidator<ChangeDescriptionProductCommand>
    {

        public ChangeDescriptionProductValidator()
        {

            RuleFor(p => p.description)
                .MaximumLength(500).WithMessage("Product description can not be longer than 500 symbols");

        }
    }
}
