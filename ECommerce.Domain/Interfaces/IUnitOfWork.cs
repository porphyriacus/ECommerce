using ECommerce.Domain.Entities;
using ECommerce.Domain.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        public Task BeginTransactionAsync(CancellationToken cancellationToken);
        public Task CommitTransactionAsync(CancellationToken cancellationToken);
        public Task RollbackTransactionAsync(CancellationToken cancellationToken);
        public Task<int> SaveChangesAsync(CancellationToken cancellationToken);

        public IReadOnlyCollection<IDomainEvent> GetDomainEvents();
        public void ClearDomainEvents();


        public ISoftDeleteRepository<Product> Products { get; }

        public IRepository<Order> Orders { get; }
        public IRepository<Cart> Carts { get; }
        public IRepository<Category> Categories { get; }
        public IRepository<Notification> Notifications { get; }
        public IRepository<Payment> Payments { get; }
        public IRepository<Review> Reviews { get; }

    }
}
