using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Infrastructure.Repositories
{
    public class EfSoftDeleteRepository<T> : EfRepository<T>, ISoftDeleteRepository<T> where T : Deletable
    {
        public EfSoftDeleteRepository(AppDbContext context) : base(context) { }
        public async Task RestoreAsync(T entity, CancellationToken cancellationToken = default)
        {
            entity.Restore();
            await UpdateAsync(entity, cancellationToken);
        }

        public async Task SoftDeleteAsync(T entity,
         CancellationToken cancellationToken = default)
        {
            entity.Delete();
            await UpdateAsync(entity, cancellationToken);
        }
    }
}
