using ExpenseTracker.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ExpenseTracker.Filters;

public class ExceptionHandlerFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        var exception = context.Exception;

        Console.WriteLine(exception.Message);

        int statusCode = GetStatusCode(exception);
        var actionName = GetActionName(statusCode);

        context.Result = new RedirectToActionResult(actionName, "Home", null);
        context.ExceptionHandled = true;
    }

    private static int GetStatusCode(Exception exception)
    {
        if (exception is EntityNotFoundException)
        {
            return 404;
        }

        return 500;
    }

    private static string GetActionName(int statusCode)
    {
        if (statusCode == 404)
        {
            return "NotFoundError";
        }

        return "InternalError";
    }
}
