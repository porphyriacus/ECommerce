using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Domain.Events
{
    public interface IDomainEvent
    {
        DateTime OccurredOn { get; }
    }
}
