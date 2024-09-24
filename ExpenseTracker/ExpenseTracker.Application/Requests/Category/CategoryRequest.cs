using ExpenseTracker.Application.Requests.Common;

namespace ExpenseTracker.Application.Requests.Category;

public class CategoryRequest : UserRequest
{
    public int CategoryId { get; set; }
}
