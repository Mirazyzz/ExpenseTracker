using ExpenseTracker.Domain.Entities;
using ExpenseTracker.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Infrastructure.Repositories;

internal sealed class WalletShareRepository : RepositoryBase<WalletShare>, IWalletShareRepository
{
    public WalletShareRepository(ExpenseTrackerDbContext context)
        : base(context)
    {
    }

    public override List<WalletShare> GetAll(Guid userId)
    {
        var walletShares = _context.WalletShares
            .AsNoTracking()
            .Where(x => x.UserId == userId)
            .ToList();

        return walletShares;
    }
}
