using ECommerce.Application.Features.Products;
using ECommerce.Domain.Entities;
using ECommerce.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

public static class TestDataFactory
{
    public static Product CreateProduct(
        string name = "iPhone 15",
        decimal price = 999.99m,
        int stock = 10,
        string currency = "USD")
    {
        return Product.Create(
            name,
            $"Description of {name}",
            new Money(price, currency),
            stock,
            Guid.NewGuid()
        );
    }

    public static ProductDTO CreateProductDTO(
        string name = "iPhone 15",
        decimal price = 999.99m)
    {
        return new ProductDTO
        {
            Name = name,
            PriceAmount = price,
            PriceCurrency = "USD"
        };
    }

    public static Category CreateCategory(
           string name = "Electronics",
           string? description = null)
    {
        return new Category(Guid.NewGuid(), name, description);
    }
}
