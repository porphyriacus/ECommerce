using ECommerce.Application.Common.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Features.Products.Command.Restore
{
    public sealed record RestoreProductCommand(Guid productId) : IRequest<Result>;
}
