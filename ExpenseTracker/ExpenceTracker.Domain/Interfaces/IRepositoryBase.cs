using ExpenseTracker.Domain.Common;

namespace ExpenseTracker.Domain.Interfaces
{
    public interface IRepositoryBase<TEntity> where TEntity : EntityBase
    {
        List<TEntity> GetAll(Guid userId);
        TEntity GetById(int id, Guid userId);
        TEntity Create(TEntity entity);
        void Update(TEntity entity);
        void Delete(int id, Guid userId);
    }
}
