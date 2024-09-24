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

    public List<TEntity> GetAll(Guid userId)
    {
        var entities = _context.Set<TEntity>()
        .AsNoTracking()
        .OrderByDescending(x => x.Id)
        .Where(x => x.UserId == userId)
        .ToList();

        return entities;
    }

    public TEntity GetById(int id, Guid userId)
    {
        var entity = GetOrThrow(id, userId);

        return entity;
    }

    public TEntity Create(TEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        _context.Set<TEntity>().Add(entity);

        return entity;
    }

    public void Delete(int id, Guid userId)
    {
        var entity = GetOrThrow(id, userId);

        _context.Set<TEntity>().Remove(entity);
    }

    public void Update(TEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        _context.Set<TEntity>().Update(entity);
    }

    private TEntity GetOrThrow(int id, Guid userId)
    {
        var entity = _context.Set<TEntity>().AsNoTracking().FirstOrDefault(x => x.Id == id && x.UserId == userId);

        if (entity is null)
        {
            throw new EntityNotFoundException($"{typeof(TEntity)} with id:{id} is not found");
        }

        return entity;
    }
}
