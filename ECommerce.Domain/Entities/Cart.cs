using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Domain.Entities
{
    public class Cart : Entity
    {
        public Guid Userid { get; private set; }
        public User User { get; private set; }
        public ICollection<CartItem> CartItems { get; private set; } = new List<CartItem>();

        protected Cart() { }
        public Cart(Guid userid)
        {
            if (userid == Guid.Empty)
                throw new ArgumentException("Cart can not exist without User");
            Userid = userid;
        }
    }
}
