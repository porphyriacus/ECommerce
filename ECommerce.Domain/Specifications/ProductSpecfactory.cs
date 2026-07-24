using ECommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Ardalis.Specification;
using ECommerce.Domain.ValueObjects;

namespace ECommerce.Domain.Specifications
{
    public static class ProductSpecfactory
    {
        public static Specification<Product> GetPopular(int minRating = 4)
        {
            var spec = new Specification<Product>();
            spec.Query.Where(p => p.Rating >= minRating);

            spec.Query.Include(p => p.Category);
            spec.Query.Include(p => p.Reviews);
            return spec;
        }

        public static Specification<Product> GetFiltered(
            string? searchTerm = null,
            Guid? categoryId = null,
            Money? minPrice = null,
            Money? maxPrice = null,
            int? minRating = null,
            bool onlyInStock = false,
            string? sortBy = null,
            bool sortAscending = true,
            int pageNumber = 1,
            int pageSize = 10)
        {
            var spec = new Specification<Product>();

            if (!string.IsNullOrEmpty(searchTerm))
                spec.Query.Where(p => p.Name.Contains(searchTerm) || p.Description.Contains(searchTerm));

            if (categoryId.HasValue)
                spec.Query.Where(p => p.CategoryId == categoryId);

            if (minPrice != null)
            {
                spec.Query.Where(p => p.Price.Currency == minPrice.Currency &&
                                      p.Price.Amount >= minPrice.Amount);
            }

            if (maxPrice != null)
            {
                spec.Query.Where(p => p.Price.Currency == maxPrice.Currency &&
                                      p.Price.Amount <= maxPrice.Amount);
            }

            if (minRating.HasValue)
                spec.Query.Where(p => p.Rating >= minRating);

            if (onlyInStock)
                spec.Query.Where(p => p.StockQuantity > 0);

            spec.Query.Include(p => p.Category);
            spec.Query.Include(p => p.Reviews);

            if (!string.IsNullOrEmpty(sortBy))
            {
                if (sortAscending)
                {
                    if (sortBy == "Price") spec.Query.OrderBy(p => p.Price);
                    else if (sortBy == "Rating") spec.Query.OrderBy(p => p.Rating);
                    else if (sortBy == "Name") spec.Query.OrderBy(p => p.Name);
                    else spec.Query.OrderBy(p => p.CreatedAt);
                }
                else
                {
                    if (sortBy == "Price") spec.Query.OrderByDescending(p => p.Price);
                    else if (sortBy == "Rating") spec.Query.OrderByDescending(p => p.Rating);
                    else if (sortBy == "Name") spec.Query.OrderByDescending(p => p.Name);
                    else spec.Query.OrderByDescending(p => p.CreatedAt);
                }
            }
            else
            {
                spec.Query.OrderBy(p => p.Name);
            }

            spec.Query.Skip((pageNumber - 1) * pageSize);
            spec.Query.Take(pageSize);

            return spec;
        }
    }
}
