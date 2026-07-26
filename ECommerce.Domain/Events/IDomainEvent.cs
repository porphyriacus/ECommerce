using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Domain.Events
{
    public interface IDomainEvent
    {
        DateTime OccurredOn { get; }
    }
    public abstract class DomainEvent : IDomainEvent
    {
        public DateTime OccurredOn { get; }

        protected DomainEvent()
        {
            OccurredOn = DateTime.UtcNow;
        }
    }
}
