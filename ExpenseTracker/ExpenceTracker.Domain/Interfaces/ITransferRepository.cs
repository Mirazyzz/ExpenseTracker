using ExpenseTracker.Domain.Entities;

namespace ExpenseTracker.Domain.Interfaces
{
    public interface ITransferRepository : IRepositoryBase<Transfer>
    {
        List<Transfer> GetAll(int? categoryId, string? search, Guid userId);
        List<Transfer> GetAll(decimal? minAmount, decimal? maxAmount, Guid userId);
    }
}
