using ECommerce.Domain.ValueObjects;
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
        public Address? ShippingAddress { get; private set; }

        public Cart Cart { get; private set; }
        public ICollection<Order> Orders { get; private set; } = new List<Order>();
        public ICollection<Review> Reviews { get; private set; } = new List<Review>();

        protected User() { }
        public User(string firstName, string lastName, Address? shippingAddress)
        {
            FirstName = firstName;
            LastName = lastName;
            CreatedAt = DateTime.UtcNow;
            ShippingAddress = shippingAddress;
            Cart = new Cart();
        }
    }
}
