using ExpenseTracker.Domain.Interfaces;

namespace ExpenseTracker.Infrastructure.Repositories;

internal class CommonRepository : ICommonRepository
{
    private readonly ExpenseTrackerDbContext _context;

    private ITransferRepository _transfers;
    public ITransferRepository Transfers => _transfers;

    private ICategoryRepository _categories;
    public ICategoryRepository Categories => _categories;

    private IImageFileRepository _imageFiles;
    public IImageFileRepository ImageFiles => _imageFiles;

    public CommonRepository(ExpenseTrackerDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));

        _categories = new CategoryRepository(context);
        _transfers = new TransferRepository(context);
        _imageFiles = new ImageFileRepository(context);
    }

    public int SaveChanges() => _context.SaveChanges();
}
