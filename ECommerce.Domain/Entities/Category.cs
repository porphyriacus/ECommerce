using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Domain.Entities
{
    public class Category : Entity
    {
        public string Name { get; private set; }
        public string? Description { get; private set; }

        public Guid? ParentCategoryId { get; private set; }

        protected Category() { }
        public Category(Guid categoryId, string name, string description)
        {
            ParentCategoryId = categoryId;
            Name = name;
            Description = description;
        }
    }
}
