using ECommerce.Domain.Entities;
using ECommerce.Domain.Events;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private IDbContextTransaction _transaction;

        public UnitOfWork(AppDbContext context) { 
            _context = context;
        }

        public async Task BeginTransactionAsync(CancellationToken cancellationToken)
        {
            _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        }
        public async Task CommitTransactionAsync(CancellationToken cancellationToken)
        {
            await _transaction.CommitAsync(cancellationToken);
            await _transaction.DisposeAsync();
            _transaction = null;

        }
        public async Task RollbackTransactionAsync(CancellationToken cancellationToken)
        {
            await _transaction.RollbackAsync(cancellationToken);
            await _transaction.DisposeAsync();
            _transaction = null;
        }
        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        public IReadOnlyCollection<IDomainEvent> GetDomainEvents()
        {
            return _context.ChangeTracker
               .Entries<Entity>()
               .Where(e => e.Entity.DomainEvents.Any())
               .SelectMany(e => e.Entity.DomainEvents)
               .ToList();
        }
        public void ClearDomainEvents()
        {
            foreach (var entity in _context.ChangeTracker.Entries<Entity>())
            {
                entity.Entity.ClearDomainEvents();
            }
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _context.Dispose();
        }
    }
}
