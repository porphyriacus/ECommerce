using Ardalis.Specification;
using ECommerce.Application.Common.Behaviors.Errors;
using ECommerce.Application.Features.Products.Command.ChangeDescription;
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
    public class ChangeDescriptionProductCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ChangeDescriptionProductCommandHandler _handler;

        public ChangeDescriptionProductCommandHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mediatorMock = new Mock<IMediator>();
            _handler = new ChangeDescriptionProductCommandHandler(
                _unitOfWorkMock.Object,
                _mediatorMock.Object
            );
        }

        [Fact]
        public async Task Handle_ProductExists_ChangesDescription()
        {
            // 
            var productId = Guid.NewGuid();
            var categoryId = Guid.NewGuid();
            var product = Product.Create(
                "Test Product",
                "Old Description",
                new Money(100, "USD"),
                10,
                categoryId
            );

            var command = new ChangeDescriptionProductCommand(productId, "New Description");

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
            product.Description.Should().Be("New Description");

            _unitOfWorkMock.Verify(
                u => u.BeginTransactionAsync(It.IsAny<CancellationToken>()),
                Times.Once
            );
            _unitOfWorkMock.Verify(
                u => u.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Once
            );
            _unitOfWorkMock.Verify(
                u => u.CommitTransactionAsync(It.IsAny<CancellationToken>()),
                Times.Once
            );
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

            var command = new ChangeDescriptionProductCommand(productId, "New Description");

            // 
            var result = await _handler.Handle(command, CancellationToken.None);

            // 
            result.IsFailure.Should().BeTrue();
            result.Error.Type.Should().Be(ErrorType.NotFound);
            result.Error.Code.Should().Contain("ChangeDescriptionProductCommand");
        }

        [Fact]
        public async Task Handle_ExceptionOccurs_RollsBackTransaction()
        {
            // 
            var productId = Guid.NewGuid();
            var categoryId = Guid.NewGuid();
            var product = Product.Create(
                "Test Product",
                "Old Description",
                new Money(100, "USD"),
                10,
                categoryId
            );

            var command = new ChangeDescriptionProductCommand(productId, "New Description");

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
            result.Error.Code.Should().Contain("ChangeDescriptionProductCommand");

            _unitOfWorkMock.Verify(
                u => u.RollbackTransactionAsync(It.IsAny<CancellationToken>()),
                Times.Once
            );
        }
    }
}