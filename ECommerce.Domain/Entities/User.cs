using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Domain.Entities
{
    public class User : IdentityUser<Guid>
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public string? ShippingAddress { get; private set; }

        public Cart Cart { get; private set; }
        public ICollection<Order> Orders { get; private set; }
        public ICollection<Review> Reviews { get; private set; }
    }
}
