using ExpenseTracker.Domain.Common;
using ExpenseTracker.Domain.Exceptions;
using ExpenseTracker.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Infrastructure.Repositories;

internal abstract class RepositoryBase<TEntity> : IRepositoryBase<TEntity> where TEntity : EntityBase
{
    protected readonly ExpenseTrackerDbContext _context;

    protected RepositoryBase(ExpenseTrackerDbContext context)
    {
        _context = context;
    }

    public TEntity Create(TEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        _context.Set<TEntity>().Add(entity);

        return entity;
    }

    public void Delete(int id)
    {
        var entity = GetOrThrow(id);

        _context.Set<TEntity>().Remove(entity);
    }

    public List<TEntity> GetAll()
    {
        var entities = _context.Set<TEntity>()
        .AsNoTracking()
        .OrderByDescending(x => x.Id)
        .ToList();

        return entities;
    }

    public TEntity GetById(int id)
    {
        var entity = GetOrThrow(id);

        return entity;
    }

    public void Update(TEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        _context.Set<TEntity>().Update(entity);
    }

    private TEntity GetOrThrow(int id)
    {
        var entity = _context.Set<TEntity>().AsNoTracking().FirstOrDefault(x => x.Id == id);

        if (entity is null)
        {
            throw new EntityNotFoundException($"{typeof(TEntity)} with id:{id} is not found");
        }

        return entity;
    }
}
