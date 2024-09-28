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

    public abstract List<TEntity> GetAll(Guid userId);

    public virtual TEntity GetById(int id)
    {
        var entity = GetOrThrow(id);

        return entity;
    }

    public TEntity Create(TEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        _context.Set<TEntity>().Add(entity);

        return entity;
    }

    public void Update(TEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        if (!_context.Set<TEntity>().Any(x => x.Id == entity.Id))
        {
            throw new EntityNotFoundException($"{typeof(TEntity)} with id: {entity.Id} is not found.");
        }

        _context.Set<TEntity>().Update(entity);
    }

    public void Delete(int id)
    {
        var entity = GetOrThrow(id);

        _context.Set<TEntity>().Remove(entity);
    }

    private TEntity GetOrThrow(int id)
    {
        var entity = _context.Set<TEntity>()
            .AsNoTracking()
            .FirstOrDefault(x => x.Id == id);

        if (entity is null)
        {
            throw new EntityNotFoundException($"{typeof(TEntity)} with id:{id} is not found");
        }

        return entity;
    }
}
