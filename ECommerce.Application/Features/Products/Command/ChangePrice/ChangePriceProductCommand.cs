using ECommerce.Application.Common.Models;
using ECommerce.Domain.ValueObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Features.Products.Command.ChangePrice
{
    public sealed record ChangePriceProductCommand(Guid id, Money money) : IRequest<Result>;
}
