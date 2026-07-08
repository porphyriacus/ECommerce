using ECommerce.Domain.Enums;
using ECommerce.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Domain.Entities
{
    public class Order : Entity
    {
        public Guid UserId { get; private set; }
        public User User { get; private set; }

        public DateTime OrderDate {  get; private set; }
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

        public OrderStatus Status {  get; private set; }
        public double TotalAmount { get; private set; }

        public Address ShippingAddress { get; private set; }

        public ICollection<OrderItem> Items { get; private set; } = new List<OrderItem>();
        public bool IsDelivered { get; private set; } = false;

        protected Order() { }
        public Order(Guid userId, DateTime orderDate, OrderStatus status, double totalAmount, Address shippingAddress, ICollection<OrderItem> items)
        {
            if (userId == Guid.Empty)
                throw new ArgumentException("Order can not exist without User");
            UserId = userId;
            OrderDate = orderDate;
            Status = status;
            TotalAmount = totalAmount;
            ShippingAddress = shippingAddress;
            Items = items;
        }

        public void ChangeDeliveryDate(DateTime deliveryDate) { 
            OrderDate = deliveryDate;
        }
    }
}
