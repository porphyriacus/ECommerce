using Ardalis.Specification;
using AutoMapper;
using ECommerce.Application.Common.Models;
using ECommerce.Application.Features.Products;
using ECommerce.Application.Features.Products.Query.Get;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Domain.Specifications;
using ECommerce.Domain.ValueObjects;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ECommerce.Application.Tests.Features.Products.Queries
{
    public class GetProductsQueryHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetProductsQueryHandler _handler;

        public GetProductsQueryHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _handler = new GetProductsQueryHandler(_unitOfWorkMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_WithFilters_ReturnsPagedResult()
        {
            // 
            var products = new List<Product>
            {
                TestDataFactory.CreateProduct("iPhone 15", 1000),
                TestDataFactory.CreateProduct("iPad Pro", 800)
            };

            var productDtos = products.Select(p => new ProductDTO
            {
                Name = p.Name,
                PriceAmount = p.Price.Amount,
                PriceCurrency = p.Price.Currency
            }).ToList();

            _unitOfWorkMock
                .Setup(u => u.Products.CountAsync(
                    It.IsAny<ISpecification<Product>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(2);

            _unitOfWorkMock
                .Setup(u => u.Products.ListAsync(
                    It.IsAny<ISpecification<Product>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(products);

            _mapperMock
                .Setup(m => m.Map<List<ProductDTO>>(products))
                .Returns(productDtos);

            var query = new GetProductsQuery(
                searchTerm: "iPhone",
                pageNumber: 1,
                pageSize: 10
            );

            //
            var result = await _handler.Handle(query, CancellationToken.None);

            // 
            result.IsSuccess.Should().BeTrue();
            result.Value.Items.Should().HaveCount(2);
            result.Value.TotalCount.Should().Be(2);
            result.Value.TotalPages.Should().Be(1);
            result.Value.HasNextPage.Should().BeFalse();
            result.Value.HasPreviousPage.Should().BeFalse();
        }

        [Fact]
        public async Task Handle_NoProducts_ReturnsEmptyPagedResult()
        {
            // 
            var products = new List<Product>();

            _unitOfWorkMock
                .Setup(u => u.Products.CountAsync(
                    It.IsAny<ISpecification<Product>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(0);

            _unitOfWorkMock
                .Setup(u => u.Products.ListAsync(
                    It.IsAny<ISpecification<Product>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(products);

            _mapperMock
                .Setup(m => m.Map<List<ProductDTO>>(products))
                .Returns(new List<ProductDTO>());

            var query = new GetProductsQuery(pageNumber: 1, pageSize: 10);

            // 
            var result = await _handler.Handle(query, CancellationToken.None);

            // 
            result.IsSuccess.Should().BeTrue();
            result.Value.Items.Should().BeEmpty();
            result.Value.TotalCount.Should().Be(0);
            result.Value.TotalPages.Should().Be(0);
        }
    }
}