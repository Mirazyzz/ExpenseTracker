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

    public List<Category> GetAll(string? search)
    {
        var query = _context.Categories
            .AsQueryable()
            .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(x => x.Name.Contains(search));
        }

        var entities = query.ToList();

        return entities;
    }
}
