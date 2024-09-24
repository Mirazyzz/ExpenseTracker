using ExpenseTracker.Application.Requests.Category;
using ExpenseTracker.Application.Services.Interfaces;
using ExpenseTracker.Application.ViewModels.Category;
using ExpenseTracker.Domain.Interfaces;
using ExpenseTracker.Mappings;
using ExpenseTracker.Stores.Interfaces;

namespace ExpenseTracker.Stores;

public class CategoryStore : ICategoryStore
{
    private readonly ICommonRepository _repository;
    private readonly ICurrentUserService _currentUserService;

    public CategoryStore(ICommonRepository repository, ICurrentUserService currentUserService)
    {
        _repository = repository;
        _currentUserService = currentUserService;
    }

    public List<CategoryViewModel> GetAll(GetCategoriesRequest request)
    {
        var userId = _currentUserService.GetCurrentUserId();
        var entities = _repository.Categories.GetAll(request.Search, request.UserId);
        var viewModels = entities
            .Select(x => x.ToViewModel())
            .ToList();

        return viewModels;
    }

    public CategoryViewModel GetById(CategoryRequest request)
    {
        var entity = _repository.Categories.GetById(request.CategoryId, request.UserId);

        return entity.ToViewModel();
    }

    public CategoryViewModel Create(CreateCategoryRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        var entity = request.ToEntity();

        var createdEntity = _repository.Categories.Create(entity);
        _repository.SaveChanges();

        return createdEntity.ToViewModel();
    }

    public void Update(UpdateCategoryRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        var entity = request.ToEntity();

        _repository.Categories.Update(entity);
        _repository.SaveChanges();
    }

    public void Delete(CategoryRequest request)
    {
        _repository.Categories.Delete(request.CategoryId, request.UserId);
        _repository.SaveChanges();
    }
}
