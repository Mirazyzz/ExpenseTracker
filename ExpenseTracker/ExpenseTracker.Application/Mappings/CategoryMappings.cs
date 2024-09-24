using ExpenseTracker.Application.Requests.Category;
using ExpenseTracker.Application.ViewModels.Category;
using ExpenseTracker.Domain.Entities;

namespace ExpenseTracker.Mappings;

public static class CategoryMappings
{
    public static Category ToEntity(this CreateCategoryRequest request)
    {
        return new Category
        {
            Name = request.Name,
            Description = request.Description,
            Type = request.Type,
            UserId = request.UserId,
        };
    }
    public static Category ToEntity(this UpdateCategoryRequest request)
    {
        return new Category
        {
            Name = request.Name,
            Description = request.Description,
            Type = request.Type,
            UserId = request.UserId,

        };
    }

    public static CategoryViewModel ToViewModel(this Category category)
    {
        return new CategoryViewModel
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
            Type = category.Type,
            CreatedAt = category.CreatedAt,
            UpdatedAt = category.UpdatedAt,
        };
    }

    public static UpdateCategoryRequest ToUpdateViewModel(this CategoryViewModel category)
    {
        return new UpdateCategoryRequest
        {
            CategoryId = category.Id,
            Name = category.Name,
            Type = category.Type,
            Description = category.Description
        };
    }

    public static CategoryRequest ToCategoryRequest(this UpdateCategoryRequest updateCategory)
    {
        return new CategoryRequest
        {
            CategoryId = updateCategory.CategoryId,
            UserId = updateCategory.UserId,
        };
    }
}
