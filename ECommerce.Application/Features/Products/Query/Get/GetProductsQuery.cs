using ECommerce.Application.Common.Models;
using ECommerce.Domain.ValueObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace ECommerce.Application.Features.Products.Query.Get
{
    public sealed record GetProductsQuery(
            string? searchTerm = null,
            Guid? categoryId = null,
            Money? minPrice = null,
            Money? maxPrice = null,
            int? minRating = null,
            bool onlyInStock = false,
            string? sortBy = null,
            bool sortAscending = true,
            int pageNumber = 1,
            int pageSize = 10) 
        : IRequest<Result<PagedResult<ProductDTO>>>;
}
