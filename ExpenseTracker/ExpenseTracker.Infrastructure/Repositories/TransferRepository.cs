using ExpenseTracker.Domain.Entities;
using ExpenseTracker.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Infrastructure.Repositories;

internal class TransferRepository : RepositoryBase<Transfer>, ITransferRepository
{
    public TransferRepository(ExpenseTrackerDbContext context)
        : base(context)
    {
    }

    public override List<Transfer> GetAll(Guid userId)
    {
        var transfers = _context.Transfers
            .AsNoTracking()
            .Where(x => x.Category.UserId == userId)
            .ToList();

        return transfers;
    }

    // TODO: refactor this -> Extract all params into one class
    public List<Transfer> GetAll(int? categoryId, string? search, Guid userId)
    {
        var query = _context.Transfers
            .AsNoTracking()
            .Where(t => t.Category.UserId == userId)
            .AsQueryable();

        if (!string.IsNullOrEmpty(search))
        {
            query = query.Where(x => x.Category.Name.Contains(search) ||
                (x.Notes != null && x.Notes.Contains(search)));
        }

        if (categoryId.HasValue)
        {
            query = query.Where(x => x.CategoryId == categoryId.Value);
        }

        var transfers = query
            .OrderByDescending(x => x.Date)
            .ToList();

        return transfers;
    }

    public List<Transfer> GetAll(decimal? minAmount, decimal? maxAmount, Guid userId)
    {
        if (minAmount is null && maxAmount is null)
        {
            return GetAll(userId);
        }

        var transfers = _context.Transfers
            .Where(x =>
                x.Category.UserId == userId &&
                (x.Amount >= minAmount && x.Amount <= maxAmount) ||
                ((minAmount == null && maxAmount != null) && x.Amount <= maxAmount) ||
                ((minAmount != null && maxAmount == null) && x.Amount >= minAmount))
            .ToList();

        return transfers;
    }
}
