using AutoMapper;
using ECommerce.Application.Common.Behaviors.Errors;
using ECommerce.Application.Common.Models;
using ECommerce.Application.Features.Products.Command.ChangeDescription;
using ECommerce.Domain.Interfaces;
using ECommerce.Domain.Specifications;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Features.Products.Query.GetById
{

    internal class GetProductByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        : IRequestHandler<GetProductByIdQuery, Result<ProductDTO>>
    {
        public async Task<Result<ProductDTO>> Handle(GetProductByIdQuery request, CancellationToken cancellation)
        {
            try
            {
                var product = await unitOfWork.Products.FirstOrDefaultAsync(ProductSpecfactory.GetById(request.id), cancellation);
                if (product == null)
                    return Error.NotFound("GetProductByIdQuery", $"Product with id {request.id} nit found");

                return mapper.Map<ProductDTO>(product);
            }
            catch (Exception ex)
            {
                return Error.Failure("GetProductByIdQuery", ex.Message);
            }
        }
    }
}
