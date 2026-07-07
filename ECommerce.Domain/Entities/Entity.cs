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
    }
}
