using ECommerce.Domain.Entities;
using ECommerce.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Features.Products
{
    public class ProductDTO
    {
        public string Name { get; set; }
        public string? Description { get; set; }

        public decimal PriceAmount { get; set; }
        public string PriceCurrency { get; set; }

        public int StockQuantity { get; set; }

        public string CategoryName { get; set; }
        public int CategoryId { get; set; }

        public double? Rating { get; set; } = null;  // add getRating, getReviews later 

        public int ReviewsCount { get; set; } = 0;
    }
}
