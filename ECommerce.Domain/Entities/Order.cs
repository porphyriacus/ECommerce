using ECommerce.Domain.Enums;
using ECommerce.Domain.Exceptions;
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

        public DateTime DeliveryDate {  get; private set; }
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

        public OrderStatus Status {  get; private set; }
        public Money TotalAmount { get; private set; }

        public Address ShippingAddress { get; private set; }

        public ICollection<OrderItem> Items { get; private set; } = new List<OrderItem>();

        protected Order() { }
        public Order(Guid userId, DateTime orderDate, OrderStatus status, Address shippingAddress, ICollection<OrderItem> items)
        {
            if (userId == Guid.Empty)
                throw new ArgumentException("Order can not exist without User");
            UserId = userId;
            DeliveryDate = orderDate;
            Status = status;
            ShippingAddress = shippingAddress;
            Items = items;
        }

        public void ChangeDeliveryDate(DateTime deliveryDate) {
            DeliveryDate = deliveryDate;
        }


        public void MarkAsPaid()
        {
            if (Status != OrderStatus.Pending)
                throw new DomainException("Only pending orders can be paid");
            Status = OrderStatus.Paid;
        }

        public void MarkAsShipped()
        {
            if (Status != OrderStatus.Paid)
                throw new DomainException("Only paid orders can be shipped");
            Status = OrderStatus.Shipped;
        }

        public void MarkAsDelivered()
        {
            if (Status != OrderStatus.Shipped)
                throw new DomainException("Only shipped orders can be delivered");
            Status = OrderStatus.Delivered;
        }
    }
}
