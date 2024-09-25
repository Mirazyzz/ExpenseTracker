using ExpenseTracker.Domain.Entities;
using ExpenseTracker.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Infrastructure.Repositories;

internal class CategoryRepository : RepositoryBase<Category>, ICategoryRepository
{
    public CategoryRepository(ExpenseTrackerDbContext context)
        : base(context)
    {
    }

    public List<Category> GetAll(string? search, Guid userId)
    {
        var query = _context.Categories
            .Where(x => x.UserId == userId)
            .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(x => x.Name.Contains(search));
        }

        var entities = query.ToList();

        return entities;
    }

    public override List<Category> GetAll(Guid userId)
    {
        var categories = _context.Categories
            .AsNoTracking()
            .Where(x => x.UserId == userId)
            .ToList();

        return categories;
    }
}
