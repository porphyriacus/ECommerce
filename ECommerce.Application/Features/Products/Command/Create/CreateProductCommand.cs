using ECommerce.Application.Common.Models;
using ECommerce.Domain.ValueObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Features.Products.Command.Create
{
    public sealed record CreateProductCommand(
        string name
        , string? description
        , Money price
        , int stockQuantity
        , Guid categoryId) 
            : IRequest<Result>;
}
