using ExpenseTracker.Application.Requests.Category;
using ExpenseTracker.Application.ViewModels.Category;

namespace ExpenseTracker.Stores.Interfaces;

public interface ICategoryStore
{
    List<CategoryViewModel> GetAll(GetCategoriesRequest request);
    CategoryViewModel GetById(CategoryRequest request);
    CategoryViewModel Create(CreateCategoryRequest request);
    void Update(UpdateCategoryRequest request);
    void Delete(CategoryRequest request);
}
