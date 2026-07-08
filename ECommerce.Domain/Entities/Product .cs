using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Domain.Entities
{
    public class Product : Entity
    {
        public string Name { get; private set; }
        public string? Description { get; private set; }
        public int Price { get; private set; }

        public int StockQuantity { get; private set; }

        public Guid CategoryId { get; private set; }
        public Category Category { get; private set; }

        public double? Rating { get; private set; } = null;

        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

        protected Product() { }
        public Product(string name, string? description, int price, int stockQuantity, Guid categoryId)
        {
            Name = name;
            Description = description;

            Price = price;
            StockQuantity = stockQuantity;

            CategoryId = categoryId;

        }
    }
}
