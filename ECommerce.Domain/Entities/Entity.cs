using ECommerce.Domain.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Domain.Entities
{
    public abstract class Entity
    {
        public Guid Id { get; protected set; }
        public Entity()
        {
            Id = Guid.NewGuid();
        }

        private readonly List<IDomainEvent> _domainEvents = new(); 
        public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        protected void AddDomainEvent(IDomainEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }

        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }
    }
}
