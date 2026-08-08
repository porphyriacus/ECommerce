using ECommerce.Application.Common.Behaviors.Errors;
using ECommerce.Application.Features.Products.Command.Create;
using ECommerce.Domain.Events;
using ECommerce.Domain.Interfaces;
using ECommerce.Domain.ValueObjects;
using MediatR;
using Moq;
using ECommerce.Domain.Entities;
using FluentAssertions;

namespace ECommerce.Application.Tests.Features.Products.Commands
{
    public class CreateProductCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMediator> _mediatorMock;
        private readonly CreateProductCommandHandler _handler;

        public CreateProductCommandHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mediatorMock = new Mock<IMediator>();
            _handler = new CreateProductCommandHandler(
                _unitOfWorkMock.Object,
                _mediatorMock.Object
            );
        }

        [Fact]
        public async Task Handle_ValidCommand_CreatesProductAndPublishesEvents()
        {
            // 
            var command = new CreateProductCommand(
                "MacBook Pro",
                "Мощный ноутбук",
                new Money(2500, "USD"),
                15,
                Guid.NewGuid()
            );

            _unitOfWorkMock
                .Setup(u => u.Products.AddAsync(
                    It.IsAny<Product>(),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

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

            _unitOfWorkMock.Verify(
                u => u.BeginTransactionAsync(It.IsAny<CancellationToken>()),
                Times.Once);

            _unitOfWorkMock.Verify(
                u => u.CommitTransactionAsync(It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_ExceptionOccurs_RollsBackTransaction()
        {
            //
            var command = new CreateProductCommand(
                "MacBook Pro",
                null,
                new Money(2500, "USD"),
                15,
                Guid.NewGuid()
            );

            _unitOfWorkMock
                .Setup(u => u.Products.AddAsync(
                    It.IsAny<Product>(),
                    It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Database error"));

            // 
            var result = await _handler.Handle(command, CancellationToken.None);

            // 
            result.IsFailure.Should().BeTrue();
            result.Error.Type.Should().Be(ErrorType.Failure);
            result.Error.Code.Should().Contain("CreateProductCommand");

            _unitOfWorkMock.Verify(
                u => u.RollbackTransactionAsync(It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}
