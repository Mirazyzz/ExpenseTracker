using ExpenseTracker.Domain.Entities;
using ExpenseTracker.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Infrastructure.Repositories;

internal sealed class WalletRepository : RepositoryBase<Wallet>, IWalletRepository
{
    public WalletRepository(ExpenseTrackerDbContext context)
        : base(context)
    {
    }

    public override List<Wallet> GetAll(Guid userId)
    {
        var wallets = _context.Wallets
            .AsNoTracking()
            .Where(x => x.OwnerId == userId || x.Shares.Any(s => s.UserId == userId && s.IsAccepted))
            .ToList();

        return wallets;
    }
}
