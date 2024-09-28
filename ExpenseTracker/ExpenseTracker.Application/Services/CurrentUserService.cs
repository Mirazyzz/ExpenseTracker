using ExpenseTracker.Application.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace ExpenseTracker.Application.Services;

internal sealed class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _contextAccessor;

    public CurrentUserService(IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor ?? throw new ArgumentNullException(nameof(contextAccessor));
    }

    public Guid GetCurrentUserId()
    {
        var user = (_contextAccessor.HttpContext?.User)
            ?? throw new InvalidOperationException("Current context does not have user.");

        var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (!Guid.TryParse(userId, out Guid result))
        {
            throw new InvalidOperationException($"Could not parse user id: {userId}.");
        }

        return result;
    }
}
