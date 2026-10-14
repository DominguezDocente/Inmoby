using Microsoft.EntityFrameworkCore;
using Properties.Application.Contracts.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Properties.Persistence.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly DataContext _context;

        public Repository(DataContext context)
        {
            _context = context;
        }

        public async Task<T> CreateAsync(T entity, CancellationToken cancellationToken = default)
        {
            await _context.Set<T>().AddAsync(entity, cancellationToken);
            return entity;
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            T? entity = await _context.Set<T>().FindAsync([id], cancellationToken);
            if (entity is null)
            {
                throw new KeyNotFoundException($"No se encontró una entidad {typeof(T).Name} con id {id}.");
            }

            _context.Remove(entity);
        }

        public async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Set<T>().FindAsync([id], cancellationToken);
        }

        public async Task<IEnumerable<T>> GetListAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Set<T>().ToListAsync(cancellationToken);
        }

        public Task<T> UpdateAsync(T entity, CancellationToken cancellationToken = default)
        {
            _context.Update(entity);
            return Task.FromResult(entity);
        }
    }
}
