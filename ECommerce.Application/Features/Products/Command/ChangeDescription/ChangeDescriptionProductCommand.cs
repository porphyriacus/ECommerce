using ECommerce.Application.Common.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Features.Products.Command.ChangeDescription
{
    public sealed record ChangeDescriptionProductCommand(Guid id, string? description) : IRequest<Result>;
}
