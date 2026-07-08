using ECommerce.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Domain.Entities
{
    public class Product : Entity
    {
        public string Name { get; private set; }
        public string? Description { get; private set; }
        public Money Price { get; private set; }

        public int StockQuantity { get; private set; }

        public Guid CategoryId { get; private set; }
        public Category Category { get; private set; }

        public double? Rating { get; private set; } = null;

        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
        
        public bool IsDeleted { get; private set; } = false;
        public DateTime? DeletedAt { get; private set; }


        public ICollection<Review> Reviews { get; private set; } = new List<Review>();

        protected Product() { }
        public Product(string name, string? description, Money price, int stockQuantity, Guid categoryId)
        {
            Name = name;
            Description = description;

            Price = price;
            StockQuantity = stockQuantity;

            CategoryId = categoryId;

        }

        public void Delete()
        {
            IsDeleted = true;
            DeletedAt = DateTime.UtcNow;
        }

        public void Restore()
        {
            IsDeleted = false;
            DeletedAt = null;
        }
    }
}
