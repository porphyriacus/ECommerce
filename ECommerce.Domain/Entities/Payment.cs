using ECommerce.Domain.Enums;
using ECommerce.Domain.Exceptions;
using ECommerce.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Domain.Entities
{
    public class Payment : Entity
    {
        public Guid UserId { get; private set; }
        public User User { get; private set; }

        public Guid OrderId { get; private set; }
        public Order Order { get; private set; }

        public string StripePaymentIntentId { get; private set; }
        public string? StripeChargeId { get; private set; }

        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
        public PaymentStatus Status { get; private set; }

        public Money Amount { get; private set; }


        protected Payment() { }
        public Payment(Guid userId, Guid orderId, PaymentStatus status, Money amount)
        {

            if (userId == Guid.Empty)
                throw new ArgumentException("Payment can not exist without User");

            if (orderId == Guid.Empty)
                throw new ArgumentException("Payment can not exist without Order");
            UserId = userId;
            OrderId = orderId;
            Status = status;
            Amount = amount;
        }


        public void MarkAsSucceeded()
        {
            if (Status != PaymentStatus.Pending)
                throw new DomainException("Only pending payments can be succeeded");
            Status = PaymentStatus.Succeeded;
        }

        public void MarkAsFailed()
        {
            Status = PaymentStatus.Failed;
        }
    }
}
