using ECommerce.Application.Common.Models;
using ECommerce.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Features.Products.Query.GetById
{
    public sealed record GetProductByIdQuery(Guid id) : IRequest<Result<ProductDTO>>;
}
