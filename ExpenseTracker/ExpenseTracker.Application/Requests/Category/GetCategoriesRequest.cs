using ExpenseTracker.Application.Requests.Common;

namespace ExpenseTracker.Application.Requests.Category;

public class GetCategoriesRequest : UserRequest
{
    public string? Search { get; set; }
}
