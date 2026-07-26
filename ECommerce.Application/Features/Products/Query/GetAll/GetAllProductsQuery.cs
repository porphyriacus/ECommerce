using ECommerce.Application.Common.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Features.Products.Query.GetAll
{

    public sealed record GetAllProductsQuery : IRequest<Result<IReadOnlyList<ProductDTO>>>;
}
