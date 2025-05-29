﻿using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Persistences;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Repositories;
public class GenericRepository<T> : IRepository<T> where T : class
{
    protected readonly OrderDbContext _context;
    private readonly DbSet<T> _dbSet;

    public GenericRepository(OrderDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<T?> GetByIdAsync(Guid id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public void Update(T entity)
    {
        _dbSet.Update(entity);
    }

    public void Remove(T entity)
    {
        _dbSet.Remove(entity);
    }
}