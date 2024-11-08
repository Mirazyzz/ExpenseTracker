using ExpenseTracker.Domain.Entities;
using ExpenseTracker.Domain.Exceptions;
using ExpenseTracker.Domain.Interfaces;
using ExpenseTracker.Infrastructure.Persistence;
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
            .AsNoTracking() // Try AsNoTrackingWithIdentityResolution()
            .Include(x => x.Owner)
            .Include(x => x.Shares)
            .Where(x => x.OwnerId == userId || x.Shares.Any(s => s.UserId == userId && s.IsAccepted))
            .ToList();

        return wallets;
    }

    public List<Wallet> GetAll(Guid userId, string? search)
    {
        var query = _context.Wallets
            .AsNoTracking()
            .Include(x => x.Owner)
            .Where(x => x.OwnerId == userId || x.Shares.Any(s => s.UserId == userId && s.IsAccepted));

        if (!string.IsNullOrEmpty(search))
        {
            query = query.Where(x => x.Name.Contains(search) ||
                (x.Description != null && x.Description.Contains(search)));
        }

        var wallets = query.ToList();

        return wallets;
    }

    public Wallet GetById(int id, Guid userId)
    {
        var wallet = _context.Wallets
            .AsNoTracking()
            .Include(x => x.Owner)
            .Include(x => x.Shares)
            .FirstOrDefault(x =>
                (x.Id == id && x.OwnerId == userId) ||
                x.Shares.Any(share => share.UserId == userId && share.IsAccepted));

        if (wallet is null)
        {
            throw new EntityNotFoundException($"Wallet with id: {id} is not found.");
        }

        return wallet;
    }

    public Wallet? GetMain(Guid userId)
    {
        var wallet = _context.Wallets
            .AsTracking()
            .Include(x => x.Owner)
            .FirstOrDefault(x => x.OwnerId == userId && x.IsMain);

        return wallet;
    }

    public void Delete(int id, Guid userId)
    {
        var wallet = _context.Wallets
            .AsTracking()
            .FirstOrDefault(x => x.Id == id && x.OwnerId == userId);

        if (wallet is null)
        {
            throw new EntityNotFoundException($"Wallet with id: {id} is not found.");
        }

        _context.Wallets.Remove(wallet);
    }
}
