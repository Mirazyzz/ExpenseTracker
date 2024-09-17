using ExpenseTracker.Application.ViewModels.Category;
using ExpenseTracker.Domain.Entities;

namespace ExpenseTracker.Application.Mappings;

public static class CategoryMappings
{
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

    public static UpdateCategoryViewModel ToUpdateViewModel(this CategoryViewModel category)
    {
        return new UpdateCategoryViewModel
        {
            Id = category.Id,
            Name = category.Name,
            Type = category.Type,
            Description = category.Description
        };
    }

    public static Category ToEntity(this CreateCategoryViewModel category)
    {
        return new Category
        {
            Name = category.Name,
            Description = category.Description,
            Type = category.Type,
        };
    }

    public static Category ToEntity(this UpdateCategoryViewModel category)
    {
        return new Category
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
            Type = category.Type,
        };
    }
}
