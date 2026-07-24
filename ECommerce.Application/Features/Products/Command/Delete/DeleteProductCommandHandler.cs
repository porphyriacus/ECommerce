using ECommerce.Application.Common.Behaviors.Errors;
using ECommerce.Application.Common.Models;
using ECommerce.Domain.Interfaces;
using ECommerce.Domain.Specifications;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Features.Products.Command.Delete
{
    internal class DeleteProductCommandHandler(IUnitOfWork unitOfWork, IMediator mediator) 
        : IRequestHandler<DeleteProductCommand, Result>
    {
        public async Task<Result> Handle(DeleteProductCommand request, CancellationToken cancellation) 
        {
            await unitOfWork.BeginTransactionAsync(cancellation);
            try
            {
                var product = await unitOfWork.Products.FirstOrDefaultAsync(ProductSpecfactory.GetById(request.productId), cancellation);
                if (product == null)
                    return Result.Failure(Error.Failure("DeleteProductCommand", $"Product with id {request.productId} nit found"));
                await unitOfWork.Products.DeleteAsync(product, cancellation);
                await unitOfWork.SaveChangesAsync(cancellation);

                foreach(var events in unitOfWork.GetDomainEvents())
                {
                    await mediator.Publish(events);
                }

                await unitOfWork.CommitTransactionAsync(cancellation);
                return Result.Ok;
            }
            catch (Exception ex) {
                await unitOfWork.RollbackTransactionAsync(cancellation);
                return Result.Failure(Error.Failure("DeleteProductCommand", ex.Message));
            }
        }
    }
}
