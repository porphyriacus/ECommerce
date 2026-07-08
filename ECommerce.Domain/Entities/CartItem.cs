using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Domain.Entities
{
    public class CartItem : Entity
    {
        public Guid CartId { get; private set; }
        public Cart Cart { get; private set; }

        public Guid ProductId { get; private set; }
        public Product Product { get; private set; }

        public int Count { get; private set; }
        public double Price { get; private set; }

        protected CartItem() { }
        public CartItem(Guid cartId,Guid productId, int count, double price)
        {
            if (cartId == Guid.Empty)
                throw new ArgumentException("CartItem can not exist without Cart");
            if (productId == Guid.Empty)
                throw new ArgumentException("CartItem can not exist without Product");
            if (price < 0)
                throw new ArgumentException("Price can not be lower then 0");

            CartId = cartId;
            ProductId = productId;
            Count = count;
            Price = price;
        }

        public void IncrementCount()
        {
            Count++;
        }
        public void DecrementCount() { Count--; }

        public void UpdatePrice(double price)
        {
            if (price < 0)
                throw new ArgumentException("Price can not be lower then 0");
            Price = price;
        }
    }
}
