using ExpenseTracker.Application.ViewModels.Category;

namespace ExpenseTracker.Application.Stores.Interfaces;

public interface ICategoryStore
{
    List<CategoryViewModel> GetAll(string? search);
    CategoryViewModel GetById(int id);
    CategoryViewModel Create(CreateCategoryViewModel category);
    void Update(UpdateCategoryViewModel category);
    void Delete(int id);
}
