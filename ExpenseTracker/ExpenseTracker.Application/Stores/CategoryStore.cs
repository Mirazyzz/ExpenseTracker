using ExpenseTracker.Application.Mappings;
using ExpenseTracker.Application.Stores.Interfaces;
using ExpenseTracker.Application.ViewModels.Category;
using ExpenseTracker.Domain.Interfaces;

namespace ExpenseTracker.Application.Stores;

public class CategoryStore : ICategoryStore
{
    private readonly ICommonRepository _repository;

    public CategoryStore(ICommonRepository repository)
    {
        _repository = repository;
    }

    public List<CategoryViewModel> GetAll(string? search)
    {
        var entities = _repository.Categories.GetAll(search);
        var viewModels = entities
            .Select(x => x.ToViewModel())
            .ToList();

        return viewModels;
    }

    public CategoryViewModel GetById(int id)
    {
        var entity = _repository.Categories.GetById(id);

        return entity.ToViewModel();
    }

    public CategoryViewModel Create(CreateCategoryViewModel category)
    {
        ArgumentNullException.ThrowIfNull(category);

        var entity = category.ToEntity();

        var createdEntity = _repository.Categories.Create(entity);
        _repository.SaveChanges();

        return createdEntity.ToViewModel();
    }

    public void Update(UpdateCategoryViewModel category)
    {
        ArgumentNullException.ThrowIfNull(category);

        var entity = category.ToEntity();

        _repository.Categories.Update(entity);
        _repository.SaveChanges();
    }

    public void Delete(int id)
    {
        _repository.Categories.Delete(id);
        _repository.SaveChanges();
    }
}

