using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Domain.Entities
{
    public class OrderItem : Entity
    {
        public Guid OrderId { get; private set; }
        public Order Order { get; private set; }

        public Guid ProductId { get; private set; }
        public Product Product { get; private set; }

        public int Quantity { get; private set; }
        public double Price { get; private set; }

        protected OrderItem() { }
        public OrderItem(Guid orderId, Guid productId, int quantity, double price)
        {
            if (orderId == Guid.Empty)
                throw new ArgumentException("OrderItem can not exist without Order");
            OrderId = orderId;
            ProductId = productId;
            Quantity = quantity;
            Price = price;
        }
    }
}
