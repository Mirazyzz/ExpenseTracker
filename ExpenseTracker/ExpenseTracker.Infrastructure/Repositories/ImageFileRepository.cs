using ExpenseTracker.Domain.Entities;
using ExpenseTracker.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Infrastructure.Repositories;

internal class ImageFileRepository : RepositoryBase<ImageFile>, IImageFileRepository
{
    public ImageFileRepository(ExpenseTrackerDbContext context)
        : base(context)
    {
    }

    public List<ImageFile> GetByTransferId(int transferId)
    {
        var images = _context.ImageFiles
            .AsNoTracking()
            .Where(x => x.TransferId == transferId)
            .ToList();

        return images;
    }
}
