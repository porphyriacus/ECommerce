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

    }
}
