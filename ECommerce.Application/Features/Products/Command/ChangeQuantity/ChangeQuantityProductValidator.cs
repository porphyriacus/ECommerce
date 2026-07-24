using ECommerce.Application.Features.Products.Command.ChangePrice;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Features.Products.Command.ChangeQuantity
{

    public class ChangeQuantityProductValidator : AbstractValidator<ChangeQuantityProductCommand>
    {

        public ChangeQuantityProductValidator()
        {
            RuleFor(p => p.quantity)
                 .GreaterThanOrEqualTo(0).WithMessage("Quantity can not be negative");
        }
    }
}
