using ECommerce.Domain.Events.ProductEvents;
using ECommerce.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace ECommerce.Domain.Entities
{
    public class Product : Deletable
    {
        public string Name { get; private set; }
        public string? Description { get; private set; }
        public Money Price { get; private set; }

        public int StockQuantity { get; private set; }

        public Guid CategoryId { get; private set; }
        public Category Category { get; private set; }

        public double? Rating { get; private set; } = null;

        public DateTime CreatedAt { get; private set; }
        
      

        public ICollection<Review> Reviews { get; private set; } = new List<Review>();

        protected Product() { }
        private Product(string name, string? description, Money price, int stockQuantity, Guid categoryId)
        {
            Name = name;
            Description = description;

            Price = price;
            StockQuantity = stockQuantity;

            CategoryId = categoryId;

            CreatedAt = DateTime.UtcNow;


        }

        public static Product Create(string name, string? description, Money price, int stockQuantity, Guid categoryId)
        {
            var product = new Product(name, description, price, stockQuantity, categoryId);
            product.AddDomainEvent(new ProductCreatedEvent(product.Id, product.Name, product.Price, product.CategoryId, product.CreatedAt));
            return product;
        }

        public void ChangeDescription(string description)
        {
            Description = description;
        }

        public void ChangePrice(Money money)
        {
            Price = money;
            AddDomainEvent(new ProductChangedPriceEvent(Id, Name, money));
        }
        
        public void ChangeQuantity(int quantity)
        {
            StockQuantity = quantity;
        }

        public void ChangeName(string name)
        {
            Name = name;
        }
    }
}
