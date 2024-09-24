
namespace ExpenseTracker.Middlewares;

public class LogRequestMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        //context.Request .Body = null;

        var controller = context.Request.PathBase.Value;
        var method = context.Request.Method;

        Console.WriteLine($"Controller: {controller}\n Method: {method}");

        await next.Invoke(context);
    }
}
