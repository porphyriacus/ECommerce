using AutoMapper;
using ECommerce.Application.Common.Behaviors.Errors;
using ECommerce.Application.Common.Models;
using ECommerce.Application.Features.Products.Query.GetById;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Domain.Specifications;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Features.Products.Query.Get
{

    internal class GetProductsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<GetProductsQuery, Result<PagedResult<ProductDTO>>>
    {
        public async Task<Result<PagedResult<ProductDTO>>> Handle(GetProductsQuery request, CancellationToken cancellation)
        {
            try
            {
                var spec = ProductSpecfactory.GetFiltered(
                   request.searchTerm,
                   request.categoryId,
                   request.minPrice,
                   request.maxPrice,
                   request.minRating,
                   request.onlyInStock,
                   request.sortBy,
                   request.sortAscending,
                   request.pageNumber,
                   request.pageSize
               );

                var totalCount = await unitOfWork.Products.CountAsync(spec, cancellation);
                var products = await unitOfWork.Products.ListAsync(spec, cancellation);

                var productDtos = mapper.Map<List<ProductDTO>>(products);

                return Result<PagedResult<ProductDTO>>.Ok(
                    new PagedResult<ProductDTO>
                    {
                        Items = productDtos,
                        PageNumber = request.pageNumber,
                        PageSize = request.pageSize,
                        TotalCount = totalCount,
                        TotalPages = (int)Math.Ceiling(totalCount / (double)request.pageSize)
                    }
                );
            }
            catch (Exception ex)
            {
                return Error.Failure("GetProductsQuery", ex.Message);
            }
        }
    }
}
