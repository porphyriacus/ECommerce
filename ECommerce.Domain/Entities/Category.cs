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
        public ICollection<Category> Subcategories { get; private set; } = new List<Category>();
        public ICollection<Product> Products { get; private set; } = new List<Product>();

        protected Category() { }
        public Category(Guid parentCategoryId, string name, string description)
        {
            ParentCategoryId = parentCategoryId;
            Name = name;
            Description = description;
        }

        public void AddCategory(Category category) {
            if(category != null)
                Subcategories.Add(category);
        }
    }
}
