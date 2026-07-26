using ECommerce.Domain.Entities;
using ECommerce.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Features.Products
{
    public class ProductDTO
    {
        public string Name { get; private set; }
        public string? Description { get; private set; }

        public decimal PriceAmount { get; private set; }
        public string PriceCurrency { get; private set; }

        public int StockQuantity { get; private set; }

        public string CategoryName { get; private set; }
        public int CategoryId { get; private set; }

        public double? Rating { get; private set; } = null;  // add getRating, getReviews later 

        public int ReviewsCount { get; private set; } = 0;
    }
}
