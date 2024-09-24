using ExpenseTracker.Application.Requests.Common;
using ExpenseTracker.Domain.Enums;

namespace ExpenseTracker.Application.Requests.Category;

public class CreateCategoryRequest : UserRequest
{
    public string Name { get; set; }
    public string Description { get; set; }
    public CategoryType Type { get; set; }
}
