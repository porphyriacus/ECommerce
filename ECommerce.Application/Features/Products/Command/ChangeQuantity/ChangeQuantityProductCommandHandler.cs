using ECommerce.Application.Common.Behaviors.Errors;
using ECommerce.Application.Common.Models;
using ECommerce.Application.Features.Products.Command.ChangePrice;
using ECommerce.Domain.Interfaces;
using ECommerce.Domain.Specifications;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Features.Products.Command.ChangeQuantity
{

    internal class ChangeQuantityProductCommandHandler(IUnitOfWork unitOfWork, IMediator mediator)
    : IRequestHandler<ChangeQuantityProductCommand, Result>
    {
        public async Task<Result> Handle(ChangeQuantityProductCommand request, CancellationToken cancellation)
        {
            await unitOfWork.BeginTransactionAsync(cancellation);
            try
            {
                var product = await unitOfWork.Products.FirstOrDefaultAsync(ProductSpecfactory.GetById(request.id), cancellation);
                if (product == null)
                    return Error.NotFound("ChangeQuantityProductCommand", $"Product with id {request.id} nit found");

                product.ChangeQuantity(request.quantity);
                await unitOfWork.SaveChangesAsync(cancellation);

                foreach (var events in unitOfWork.GetDomainEvents())
                {
                    await mediator.Publish(events);
                }

                await unitOfWork.CommitTransactionAsync(cancellation);
                return Result.Ok;
            }
            catch (Exception ex)
            {
                await unitOfWork.RollbackTransactionAsync(cancellation);
                return Result.Failure(Error.Failure("ChangeQuantityProductCommand", ex.Message));
            }
        }
    }
}
