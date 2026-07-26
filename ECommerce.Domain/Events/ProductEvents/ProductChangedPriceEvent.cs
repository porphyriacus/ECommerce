using ECommerce.Domain.Entities;
using ECommerce.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Domain.Events.ProductEvents
{
    public class ProductChangedPriceEvent : DomainEvent
    {
        public Guid ProductId { get; }
        public string Name { get; }
        public Money Price { get; }

        public ProductChangedPriceEvent(
           Guid productId,
           string name,
           Money price)
        {
            ProductId = productId;
            Name = name;
            Price = price;
        }
    }
}
