using ExpenseTracker.Domain.Interfaces;

namespace ExpenseTracker.Infrastructure.Repositories;

internal class CommonRepository : ICommonRepository
{
    private readonly ExpenseTrackerDbContext _context;

    private readonly ITransferRepository _transfers;
    public ITransferRepository Transfers => _transfers;

    private readonly ICategoryRepository _categories;
    public ICategoryRepository Categories => _categories;

    private readonly IImageFileRepository _imageFiles;
    public IImageFileRepository ImageFiles => _imageFiles;

    private readonly IWalletRepository _wallet;
    public IWalletRepository Wallets => _wallet;

    private readonly IWalletShareRepository _walletShare;
    public IWalletShareRepository WalletShares => _walletShare;

    public CommonRepository(ExpenseTrackerDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));

        _categories = new CategoryRepository(context);
        _transfers = new TransferRepository(context);
        _imageFiles = new ImageFileRepository(context);
        _wallet = new WalletRepository(context);
        _walletShare = new WalletShareRepository(context);
    }

    public int SaveChanges() => _context.SaveChanges();
}
