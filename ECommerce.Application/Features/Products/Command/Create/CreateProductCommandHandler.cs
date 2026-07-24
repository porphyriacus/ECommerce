using AutoMapper;
using ECommerce.Application.Common.Models;
using ECommerce.Domain.Interfaces;
using ECommerce.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.IdentityModel.Tokens.Experimental;
using ECommerce.Application.Common.Behaviors.Errors;

namespace ECommerce.Application.Features.Products.Command.Create
{
    internal class CreateProductCommandHandler(IUnitOfWork unitOfWork, IMediator mediator) 
        : IRequestHandler<CreateProductCommand, Result>
    {
        public async Task<Result> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            await unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                var product = Product.Create(
                    request.name
                    , request.description
                    , request.price
                    , request.stockQuantity
                    , request.categoryId);

                await unitOfWork.Products.AddAsync(product, cancellationToken);
                await unitOfWork.SaveChangesAsync(cancellationToken);

                foreach (var domainEvent in product.DomainEvents)
                {
                    await mediator.Publish(domainEvent, cancellationToken);
                }
                product.ClearDomainEvents();

                await unitOfWork.CommitTransactionAsync(cancellationToken);
                return Result.Ok;
            }
            catch (Exception ex) {
                await unitOfWork.RollbackTransactionAsync(cancellationToken);
                return Result.Failure(Error.Failure("CreateProductCommand", ex.Message));
            }
        }
    }
}
