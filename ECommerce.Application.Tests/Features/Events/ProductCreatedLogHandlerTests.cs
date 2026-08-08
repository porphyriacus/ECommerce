using ECommerce.Application.Features.Events;
using ECommerce.Domain.Events.ProductEvents;
using ECommerce.Domain.ValueObjects;
using FluentAssertions;
using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ECommerce.Application.Tests.Features.Events
{
    public class ProductCreatedLogHandlerTests
    {
        [Fact]
        public async Task Handle_ProductCreatedEvent_WritesToConsole()
        {
            // Arrange
            var handler = new ProductCreatedLogHandler(); // ← БЫЛО ProductChangedPriceLogHandler!
            var productId = Guid.NewGuid();
            var categoryId = Guid.NewGuid();
            var @event = new ProductCreatedEvent(
                productId,
                "Test Product",
                new Money(100, "USD"),
                categoryId,
                DateTime.UtcNow
            );

            using var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            // Act
            await handler.Handle(@event, CancellationToken.None);

            // Assert
            var output = stringWriter.ToString();

            // ✅ ПРОВЕРЯЕМ ТО, ЧТО РЕАЛЬНО ВЫВОДИТСЯ
            output.Should().Contain("Product Test Product");
            output.Should().Contain(productId.ToString());
            output.Should().Contain("price : Money { Amount = 100, Currency = USD }");
            output.Should().Contain($"categoryId : {categoryId}");
        }

        [Fact]
        public async Task Handle_ProductCreatedEvent_DoesNotThrowException()
        {
            // Arrange
            var handler = new ProductCreatedLogHandler();
            var @event = new ProductCreatedEvent(
                Guid.NewGuid(),
                "Test Product",
                new Money(100, "USD"),
                Guid.NewGuid(),
                DateTime.UtcNow
            );

            // Act & Assert
            var exception = await Record.ExceptionAsync(() =>
                handler.Handle(@event, CancellationToken.None));

            exception.Should().BeNull();
        }
    }
}