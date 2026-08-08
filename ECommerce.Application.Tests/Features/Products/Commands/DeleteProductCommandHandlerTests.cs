using Ardalis.Specification;
using ECommerce.Application.Common.Behaviors.Errors;
using ECommerce.Application.Features.Products.Command.Delete;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Events;
using ECommerce.Domain.Interfaces;
using ECommerce.Domain.Specifications;
using ECommerce.Domain.ValueObjects;
using FluentAssertions;
using MediatR;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ECommerce.Application.Tests.Features.Products.Commands
{
    public class DeleteProductCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMediator> _mediatorMock;
        private readonly DeleteProductCommandHandler _handler;

        public DeleteProductCommandHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mediatorMock = new Mock<IMediator>();
            _handler = new DeleteProductCommandHandler(_unitOfWorkMock.Object, _mediatorMock.Object);
        }

        [Fact]
        public async Task Handle_ProductExists_SoftDeletesProduct()
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

            _unitOfWorkMock
                .Setup(u => u.Products.FirstOrDefaultAsync(
                    It.IsAny<ISpecification<Product>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(product);

            _unitOfWorkMock
                .Setup(u => u.Products.DeleteAsync(
                    It.IsAny<Product>(), 
                    It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            _unitOfWorkMock
                .Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            _unitOfWorkMock
                .Setup(u => u.GetDomainEvents())
                .Returns(new List<IDomainEvent>());

            var command = new DeleteProductCommand(productId);

            // 
            var result = await _handler.Handle(command, CancellationToken.None);

            // 
            result.IsSuccess.Should().BeTrue();

            _unitOfWorkMock.Verify(
               u => u.BeginTransactionAsync(It.IsAny<CancellationToken>()),
               Times.Once);

            _unitOfWorkMock.Verify(
                u => u.Products.DeleteAsync(
                    It.IsAny<Product>(), 
                    It.IsAny<CancellationToken>()),
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

            var command = new DeleteProductCommand(productId);

            // 
            var result = await _handler.Handle(command, CancellationToken.None);

            // 
            result.IsFailure.Should().BeTrue();
            result.Error.Type.Should().Be(ErrorType.NotFound);
            result.Error.Code.Should().Contain("DeleteProductCommand");
        }
    }
}