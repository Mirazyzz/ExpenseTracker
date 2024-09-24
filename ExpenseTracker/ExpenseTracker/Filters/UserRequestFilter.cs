using ExpenseTracker.Application.Requests.Common;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace ExpenseTracker.Filters;

public class UserRequestFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var args = context.ActionArguments.Values.OfType<UserRequest>().FirstOrDefault();
        
        if (args != null)
        {
            var user = context.HttpContext.User;
            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (Guid.TryParse(userId, out var result))
            {
                args.UserId = result;
            }
        }

        await next();
    }
}
