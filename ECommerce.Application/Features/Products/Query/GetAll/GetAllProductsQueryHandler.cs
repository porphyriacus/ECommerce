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

namespace ECommerce.Application.Features.Products.Query.GetAll
{

    internal class GetAllProductsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<GetAllProductsQuery, Result<IReadOnlyList<ProductDTO>>>
    {
        public async Task<Result<IReadOnlyList<ProductDTO>>> Handle(GetAllProductsQuery request, CancellationToken cancellation)
        {
            try
            {
                var products = await unitOfWork.Products.ListAllAsync(cancellation);
                if (products == null || !products.Any())
                    return Error.NotFound("GetAllProductsQuery", $"Products not found");

                var dtos = mapper.Map<List<ProductDTO>>(products);
                return Result<IReadOnlyList<ProductDTO>>.Ok(dtos);
            }
            catch (Exception ex)
            {
                return Error.Failure("GetAllProductsQuery", ex.Message);
            }
        }
    }
}
