using ExpenseTracker.Application.Mappings;
using ExpenseTracker.Application.Requests.Category;
using ExpenseTracker.Application.Services.Interfaces;
using ExpenseTracker.Application.ViewModels.Category;
using ExpenseTracker.Domain.Exceptions;
using ExpenseTracker.Domain.Interfaces;
using ExpenseTracker.Stores.Interfaces;
using Microsoft.EntityFrameworkCore;

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
        var entity = _repository.Categories.GetById(request.Id);

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

        try
        {
            _repository.Categories.Update(entity);
            _repository.SaveChanges();
        }
        catch(DbUpdateConcurrencyException)
        {
            if (!_repository.Categories.Exists(request.Id))
            {
                throw new EntityNotFoundException($"Category with id: {request.Id} is not found.");
            }
        }
    }

    public void Delete(CategoryRequest request)
    {
        _repository.Categories.Delete(request.Id);
        _repository.SaveChanges();
    }
}
