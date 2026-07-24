using ECommerce.Application.Features.Products.Command.ChangeDescription;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Features.Products.Command.ChangePrice
{

    public class ChangePriceProductValidator : AbstractValidator<ChangePriceProductCommand>
    {

        public ChangePriceProductValidator()
        {

            RuleFor(p => p.price.Amount)
                 .GreaterThanOrEqualTo(0).WithMessage("Price can not be negative");
        }
    }
}
