using ExpenseTracker.Domain.Entities;

namespace ExpenseTracker.Domain.Interfaces
{
    public interface ITransferRepository : IRepositoryBase<Transfer>
    {
        List<Transfer> GetAll(Guid userId, int? categoryId, string? search);
        List<Transfer> GetAll(Guid userId, decimal? minAmount, decimal? maxAmount);

    }
}
