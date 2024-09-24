using Microsoft.AspNetCore.Mvc.Filters;

namespace ExpenseTracker.Filters;

public class HeaderResultFilter : IResultFilter
{
    public void OnResultExecuted(ResultExecutedContext context)
    {
    }

    public void OnResultExecuting(ResultExecutingContext context)
    {
        context.HttpContext.Response.Headers.Append("X-Powered-By", "Expense Tracker");
    }
}
