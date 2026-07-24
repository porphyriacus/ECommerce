using ECommerce.Application.Common.Models;
using ECommerce.Domain.ValueObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Features.Products.Command.ChangeQuantity
{

    public sealed record ChangeQuantityProductCommand(Guid id, int quantity) : IRequest<Result>;
}
