using ECommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Domain.Interfaces
{
    public interface ISoftDeleteRepository<T> : IRepository<T> where T : Entity
    {
        Task<T> RestoreAsync(T entity, CancellationToken cancellationToken = default);

    }
}
