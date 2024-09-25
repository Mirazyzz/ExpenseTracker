using ExpenseTracker.Domain.Entities;

namespace ExpenseTracker.Domain.Interfaces
{
    public interface ICategoryRepository : IRepositoryBase<Category>
    {
        List<Category> GetAll(string? search, Guid userId);
    }
}
