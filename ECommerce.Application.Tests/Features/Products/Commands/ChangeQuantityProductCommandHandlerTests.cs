using Ardalis.Specification;
using ECommerce.Application.Common.Behaviors.Errors;
using ECommerce.Application.Features.Products.Command.ChangeQuantity;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Events;
using ECommerce.Domain.Interfaces;
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
    public class ChangeQuantityProductCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ChangeQuantityProductCommandHandler _handler;

        public ChangeQuantityProductCommandHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mediatorMock = new Mock<IMediator>();
            _handler = new ChangeQuantityProductCommandHandler(_unitOfWorkMock.Object, _mediatorMock.Object);
        }

        [Fact]
        public async Task Handle_ProductExists_ChangesQuantity()
        {
            // 
            var productId = Guid.NewGuid();
            var categoryId = Guid.NewGuid();
            var product = Product.Create(
                "Test Product",
                "Description",
                new Money(100, "USD"),
                10,
                categoryId
            );

            var command = new ChangeQuantityProductCommand(productId, 25);

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
            product.StockQuantity.Should().Be(25);

            _unitOfWorkMock.Verify(u => u.BeginTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.CommitTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
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

            var command = new ChangeQuantityProductCommand(productId, 25);

            // 
            var result = await _handler.Handle(command, CancellationToken.None);

            // 
            result.IsFailure.Should().BeTrue();
            result.Error.Type.Should().Be(ErrorType.NotFound);
        }
    }
}