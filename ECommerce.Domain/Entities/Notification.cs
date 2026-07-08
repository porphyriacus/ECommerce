using ECommerce.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Domain.Entities
{
    public class Notification : Entity
    {
        public Guid UserId { get; private set; }
        public User User { get; private set; }

        public NotificationType NotificationType { get; private set; }
        public string Message { get; private set; }
        public bool IsSent { get; private set; } = false;
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

        protected Notification() { }

        public Notification(Guid userId, NotificationType notificationType, string message)
        {
            if (userId == Guid.Empty)
                throw new ArgumentException("Notification can not exist without User");
            UserId = userId;
            NotificationType = notificationType;
            Message = message;
        }
    }
}
