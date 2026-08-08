using Ardalis.Specification;
using AutoMapper;
using ECommerce.Application.Common.Behaviors.Errors;
using ECommerce.Application.Features.Products;
using ECommerce.Application.Features.Products.Query.GetById;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Tests.Features.Products.Queries
{
    public class GetProductByIdQueryHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetProductByIdQueryHandler _handler;

        public GetProductByIdQueryHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _handler = new GetProductByIdQueryHandler(
                _unitOfWorkMock.Object,
                _mapperMock.Object
            );
        }

        [Fact]
        public async Task Handle_ProductExists_ReturnsProductDTO()
        {
            // arrange
            var productId = Guid.NewGuid();
            var product = TestDataFactory.CreateProduct();
            var productDto = TestDataFactory.CreateProductDTO();

            _unitOfWorkMock
                .Setup(u => u.Products.FirstOrDefaultAsync(
                    It.IsAny<ISpecification<Product>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(product);

            _mapperMock
                .Setup(m => m.Map<ProductDTO>(product))
                .Returns(productDto);

            var query = new GetProductByIdQuery(productId);

            // act
            var result = await _handler.Handle(query, CancellationToken.None);

            // assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Be(productDto);
            result.Value.Name.Should().Be("iPhone 15");
        }

        [Fact]
        public async Task Handle_ProductNotFound_ReturnsNotFoundError()
        {
            // arrange
            var productId = Guid.NewGuid();

            _unitOfWorkMock
                .Setup(u => u.Products.FirstOrDefaultAsync(
                    It.IsAny<ISpecification<Product>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((Product)null!);

            var query = new GetProductByIdQuery(productId);

            // act
            var result = await _handler.Handle(query, CancellationToken.None);

            // assert
            result.IsFailure.Should().BeTrue();
            result.Error.Type.Should().Be(ErrorType.NotFound);
            result.Error.Code.Should().Contain("GetProductByIdQuery");
        }
    }
}
