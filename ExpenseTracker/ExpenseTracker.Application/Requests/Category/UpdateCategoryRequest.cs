namespace ExpenseTracker.Application.Requests.Category;

public class UpdateCategoryRequest : CreateCategoryRequest
{
    public int CategoryId { get; set; }
}
