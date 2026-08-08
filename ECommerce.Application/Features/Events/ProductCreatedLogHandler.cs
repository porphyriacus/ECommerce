using ECommerce.Domain.Events.ProductEvents;
using ECommerce.Domain.ValueObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Features.Events
{
    public class ProductCreatedLogHandler : INotificationHandler<ProductCreatedEvent>
    {

        public async Task Handle(ProductCreatedEvent notification, CancellationToken cancellationToken)
        {
            Console.WriteLine($"{notification.CreatedAt} ::: Product {notification.Name} with id {notification.ProductId} was created. \n\t price : {notification.Price} || quantity : {notification.StockQuantity} || categoryId : {notification.CategoryId}");
        }
    }
}
