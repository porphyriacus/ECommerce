using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Domain.Exceptions.Product
{
    public class ProductNotFoundException : DomainException
    {
        public Guid ProductId { get; }

        public ProductNotFoundException(Guid productId)
            : base($"Product with ID {productId} was not found")
        {
            ProductId = productId;
        }
    }
}
