using ExpenseTracker.Domain.Enums;

namespace ExpenseTracker.Application.ViewModels.Category;

public class CreateCategoryViewModel
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public CategoryType Type { get; set; }
}
