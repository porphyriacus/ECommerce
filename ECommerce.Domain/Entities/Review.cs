using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Domain.Entities
{
    public class Review : Entity
    {

        public Guid UserId { get; private set; }
        public User User { get; private set; }

        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

        public Product Product { get; private set; }
        public Guid ProductId { get; private set; }

        public string Comment { get; private set; }

        private int _rating;
        public int Rating
        {
            get => _rating;
            private set
            {
                if (value < 0 || value > 5)
                    throw new ArgumentException("Rating should be in range of 0 to 5");
                _rating = value;
            }
        }

        public bool IsEdited { get; private set; } = false;


        protected Review() { }
        public Review(Guid userId, Guid productId, string comment, int rating)
        {
            if (userId == Guid.Empty)
                throw new ArgumentException("Rewiew can not exist without User");
            if (productId == Guid.Empty)
                throw new ArgumentException("Rewiew can not exist without Product");

            UserId = userId;
            ProductId = productId;
            Comment = comment;
            Rating = rating;
        }

        public void ChangeComment(string newComment)
        {
            Comment = newComment;
            IsEdited = true;
        }
    }
}
