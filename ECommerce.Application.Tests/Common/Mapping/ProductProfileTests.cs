using AutoMapper;
using ECommerce.Application.Common.Mapping;
using ECommerce.Application.Features.Products;
using ECommerce.Domain.Entities;
using ECommerce.Domain.ValueObjects;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using Xunit;

namespace ECommerce.Application.Tests.Common.Mappings
{
    public class ProductProfileTests
    {
        private readonly IMapper _mapper;

        public ProductProfileTests()
        {
            var config = new MapperConfiguration(
                cfg => cfg.AddProfile<ProductProfile>(),
                NullLoggerFactory.Instance  
            );
            _mapper = config.CreateMapper();
        }

        [Fact]
        public void ProductProfile_ShouldBeValid()
        {
            var config = new MapperConfiguration(
                cfg => cfg.AddProfile<ProductProfile>(),
                NullLoggerFactory.Instance
            );
            config.AssertConfigurationIsValid();
        }

        [Fact]
        public void Map_ProductToProductDTO_MapsAllPropertiesCorrectly()
        {
            var categoryId = Guid.NewGuid();
            var product = Product.Create(
                "iPhone 15",
                "New iPhone",
                new Money(999.99m, "USD"),
                10,
                categoryId
            );

            var dto = _mapper.Map<ProductDTO>(product);

            dto.Should().NotBeNull();
            dto.Name.Should().Be("iPhone 15");
            dto.PriceAmount.Should().Be(999.99m);
            dto.PriceCurrency.Should().Be("USD");
            dto.StockQuantity.Should().Be(10);
            dto.CategoryId.Should().Be(categoryId);
        }

        [Fact]
        public void Map_NullProduct_ReturnsNull()
        {
            Product product = null;

            var dto = _mapper.Map<ProductDTO>(product);

            dto.Should().BeNull();
        }
    }
}