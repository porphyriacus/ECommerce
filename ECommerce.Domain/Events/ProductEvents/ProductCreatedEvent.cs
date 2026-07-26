using ECommerce.Domain.Entities;
using ECommerce.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Domain.Events.ProductEvents
{
    public class ProductCreatedEvent : DomainEvent
    {
        public Guid ProductId { get; }
        public string Name { get; }
        public Money Price { get; }
        public int StockQuantity { get; }
        public Guid CategoryId { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public ProductCreatedEvent(
            Guid productId,
            string name,
            Money price,
            Guid categoryId,
            DateTime createdAt)
        {
            ProductId = productId;
            Name = name;
            Price = price;
            CategoryId = categoryId;
            CreatedAt = createdAt;
        }
    }
}
