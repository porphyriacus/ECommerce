using AutoMapper;
using ECommerce.Application.Common.Behaviors.Errors;
using ECommerce.Application.Features.Products;
using ECommerce.Application.Features.Products.Query.GetAll;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
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
    public class GetAllProductsQueryHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetAllProductsQueryHandler _handler;

        public GetAllProductsQueryHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _handler = new GetAllProductsQueryHandler(_unitOfWorkMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ProductsExist_ReturnsProductList()
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
                .Setup(u => u.Products.ListAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(products);

            _mapperMock
                .Setup(m => m.Map<List<ProductDTO>>(products))
                .Returns(productDtos);

            var query = new GetAllProductsQuery();

            // 
            var result = await _handler.Handle(query, CancellationToken.None);

            // 
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().HaveCount(2);
            result.Value.Should().BeEquivalentTo(productDtos);
        }

        [Fact]
        public async Task Handle_NoProducts_ReturnsNotFoundError()
        {
            // 
            _unitOfWorkMock
                .Setup(u => u.Products.ListAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Product>());

            var query = new GetAllProductsQuery();

            // 
            var result = await _handler.Handle(query, CancellationToken.None);

            // 
            result.IsFailure.Should().BeTrue();
            result.Error.Type.Should().Be(ErrorType.NotFound);
            result.Error.Code.Should().Contain("GetAllProductsQuery");
        }
    }
}