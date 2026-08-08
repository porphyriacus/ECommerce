using Moq;
using System;
using System.Linq.Expressions;

namespace ECommerce.Application.Tests.Helpers
{
    public static class MockExtensions
    {
        public static void SetupAsync<T, TResult>(
            this Mock<T> mock,
            Expression<Func<T, Task<TResult>>> expression,
            TResult returnValue) where T : class
        {
            mock.Setup(expression).ReturnsAsync(returnValue);
        }

        public static void SetupAsync<T>(
            this Mock<T> mock,
            Expression<Func<T, Task>> expression) where T : class
        {
            mock.Setup(expression).Returns(Task.CompletedTask);
        }
    }
}