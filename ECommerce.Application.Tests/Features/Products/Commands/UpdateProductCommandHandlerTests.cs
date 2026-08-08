using Ardalis.Specification;
using ECommerce.Application.Common.Behaviors.Errors;
using ECommerce.Application.Features.Products.Command.Update;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Events;
using ECommerce.Domain.Interfaces;
using ECommerce.Domain.Specifications;
using ECommerce.Domain.ValueObjects;
using FluentAssertions;
using MediatR;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ECommerce.Application.Tests.Features.Products.Commands
{
    public class UpdateProductCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMediator> _mediatorMock;
        private readonly UpdateProductCommandHandler _handler;

        public UpdateProductCommandHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mediatorMock = new Mock<IMediator>();
            _handler = new UpdateProductCommandHandler(_unitOfWorkMock.Object, _mediatorMock.Object);
        }

        [Fact]
        public async Task Handle_ProductExists_UpdatesProduct()
        {
            // 
            var productId = Guid.NewGuid();
            var categoryId = Guid.NewGuid();

            var product = Product.Create(
                "Old Name",
                "Old Description",
                new Money(100, "USD"),
                10,
                categoryId
            );

            var command = new UpdateProductCommand(
                productId,
                "New Name",
                "New Description",
                new Money(200, "USD"),
                20
            );

            _unitOfWorkMock
                .Setup(u => u.Products.FirstOrDefaultAsync(
                    It.IsAny<ISpecification<Product>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(product);

            _unitOfWorkMock
                .Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            _unitOfWorkMock
                .Setup(u => u.GetDomainEvents())
                .Returns(new List<IDomainEvent>());

            // 
            var result = await _handler.Handle(command, CancellationToken.None);

            // 
            result.IsSuccess.Should().BeTrue();

            product.Name.Should().Be("New Name");
            product.Description.Should().Be("New Description");
            product.Price.Amount.Should().Be(200);
            product.StockQuantity.Should().Be(20);

            _unitOfWorkMock.Verify(
                u => u.BeginTransactionAsync(It.IsAny<CancellationToken>()),
                Times.Once);

            _unitOfWorkMock.Verify(
                u => u.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Once);

            _unitOfWorkMock.Verify(
                u => u.CommitTransactionAsync(It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_ProductNotFound_ReturnsNotFoundError()
        {
            // 
            var productId = Guid.NewGuid();

            _unitOfWorkMock
                .Setup(u => u.Products.FirstOrDefaultAsync(
                    It.IsAny<ISpecification<Product>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((Product)null!);

            var command = new UpdateProductCommand(
                productId,
                "New Name",
                null,
                new Money(200, "USD"),
                20
            );

            // 
            var result = await _handler.Handle(command, CancellationToken.None);

            // 
            result.IsFailure.Should().BeTrue();
            result.Error.Type.Should().Be(ErrorType.NotFound);
            result.Error.Code.Should().Contain("UpdateProductCommand");
        }

        [Fact]
        public async Task Handle_ExceptionOccurs_RollsBackTransaction()
        {
            // 
            var productId = Guid.NewGuid();
            var categoryId = Guid.NewGuid();

            var product = Product.Create(
                "Old Name",
                "Old Description",
                new Money(100, "USD"),
                10,
                categoryId
            );

            var command = new UpdateProductCommand(
                productId,
                "New Name",
                "New Description",
                new Money(200, "USD"),
                20
            );

            _unitOfWorkMock
                .Setup(u => u.Products.FirstOrDefaultAsync(
                    It.IsAny<ISpecification<Product>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(product);

            _unitOfWorkMock
                .Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Database error"));

            // 
            var result = await _handler.Handle(command, CancellationToken.None);

            // 
            result.IsFailure.Should().BeTrue();
            result.Error.Type.Should().Be(ErrorType.Failure);
            result.Error.Code.Should().Contain("UpdateProductCommand");

            _unitOfWorkMock.Verify(
                u => u.RollbackTransactionAsync(It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}