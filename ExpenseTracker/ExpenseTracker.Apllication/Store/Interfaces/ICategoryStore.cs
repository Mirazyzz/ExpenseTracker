using ExpenseTracker.Application.ViewModels.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Application.Store.Interfaces;

public interface ICategoryStore
{
    List<CategoryViewModel> GetAll(string? search);
    CategoryViewModel GetById(int id);
    CategoryViewModel Create(CreateCategoryViewModel category);
    void Update(UpdateCategoryViewModel category);
    void Delete(int id);
}
