using ECommerce.Application.Common.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Features.Products.Command.Delete
{
    public sealed record DeleteProductCommand(Guid productId) : IRequest<Result>;
}
