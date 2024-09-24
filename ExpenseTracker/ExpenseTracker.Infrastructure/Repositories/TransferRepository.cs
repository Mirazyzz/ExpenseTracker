using ExpenseTracker.Domain.Entities;
using ExpenseTracker.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Infrastructure.Repositories;

internal class TransferRepository : RepositoryBase<Transfer>, ITransferRepository
{
    public TransferRepository(ExpenseTrackerDbContext context) : base(context) { }

    public List<Transfer> GetAll(Guid userId, int? categoryId, string? search)
    {
        var query = _context.Transfers
            .AsNoTracking()
            .AsQueryable().Where(x=>x.UserId==userId);

        if (!string.IsNullOrEmpty(search))
        {
            query = query.Where(x => x.Category.Name.Contains(search) ||
                (x.Note != null && x.Note.Contains(search)));
        }

        if (categoryId.HasValue)
        {
            query = query.Where(x => x.CategoryId == categoryId.Value);
        }

        var transfers = query
            .OrderByDescending(x => x.Date).Where(x => x.UserId == userId)
            .ToList();

        return transfers;
    }

    public List<Transfer> GetAll(Guid userId, decimal? minAmount, decimal? maxAmount)
    {
        if (minAmount is null && maxAmount is null)
        {
            return GetAll(userId);
        }

        var transfers = _context.Transfers.Where(x => x.Amount >= minAmount && x.Amount <= maxAmount ||
        ((minAmount == null && maxAmount != null) && x.Amount <= maxAmount) ||
        ((minAmount != null && maxAmount == null) && x.Amount >= minAmount)
        ).Where(x=>x.UserId==userId).ToList();

        return transfers;
    }
}
