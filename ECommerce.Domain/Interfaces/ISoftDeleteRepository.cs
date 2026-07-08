using ECommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Domain.Interfaces
{
    public interface ISoftDeleteRepository<T> : IRepository<T> where T : Product
    {
        Task RestoreAsync(T entity, CancellationToken cancellationToken = default);

        Task SoftDeleteAsync(T entity, CancellationToken cancellationToken = default);
    }
}
