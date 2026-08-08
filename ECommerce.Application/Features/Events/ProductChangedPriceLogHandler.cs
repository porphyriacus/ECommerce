using ECommerce.Domain.Events.ProductEvents;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Features.Events
{
    public class ProductChangedPriceLogHandler : INotificationHandler<ProductChangedPriceEvent>
    {
        public async Task Handle(ProductChangedPriceEvent notification, CancellationToken cancellationToken)
        {
            Console.WriteLine($"{notification.OccurredOn } ::: Product {notification.Name} with id {notification.ProductId} was changed. \n\t New price : {notification.Price}");
        }
    }
}
