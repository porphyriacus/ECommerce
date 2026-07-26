using ECommerce.Application.Common.Models;
using ECommerce.Domain.ValueObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Features.Products.Command.Update
{

    public sealed record UpdateProductCommand(
    Guid id
    , string name
    , string? description
    , Money price
    , int stockQuantity
    )
        : IRequest<Result>;
}
