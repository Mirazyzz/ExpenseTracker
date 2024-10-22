using ExpenseTracker.Application.Mappings;
using ExpenseTracker.Application.Requests.User;
using ExpenseTracker.Application.Stores.Interfaces;
using ExpenseTracker.Application.ViewModels.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Application.Stores;

public class UserStore : IUserStore
{
    private readonly UserManager<IdentityUser<Guid>> _userManager;

    public UserStore(UserManager<IdentityUser<Guid>> userManager)
    {
        _userManager = userManager;
    }

    public async Task<List<UserViewModel>> GetAll(GetUserRequest request)
    {
        var users = await _userManager.Users.ToListAsync();
        var result = users.Select(user => user.ToViewModel()).ToList();

        return result;
    }

    public async Task<UserViewModel> GetById(UserRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);
        
        var user = await _userManager.FindByIdAsync(request.Id.ToString());
        if (user == null)
        {
            throw new KeyNotFoundException("User not found.");
        }

        return user.ToViewModel();
    }

    public async Task<UserViewModel> Create(CreateUserRequest request, IEnumerable<IFormFile> attachments)
    {
        ArgumentNullException.ThrowIfNull(request);
        
        var user = request.ToEntity();
        var result = await _userManager.CreateAsync(user);
        
        if (!result.Succeeded)
        {
            throw new InvalidOperationException(string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        return user.ToViewModel();
    }

    public async Task Update(UpdateUserRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);
        
        var user = await _userManager.FindByIdAsync(request.Id.ToString());
        if (user == null)
        {
            throw new KeyNotFoundException("User not found.");
        }

        user.UserName = request.UserName;
        user.Email = request.Email;
        user.PhoneNumber = request.PhoneNumber;

        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            throw new InvalidOperationException(string.Join(", ", result.Errors.Select(e => e.Description)));
        }
    }

    public async Task Delete(UpdateUserRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);
        
        var user = await _userManager.FindByIdAsync(request.Id.ToString());
        if (user == null)
        {
            throw new KeyNotFoundException("User not found.");
        }

        var result = await _userManager.DeleteAsync(user);
        if (!result.Succeeded)
        {
            throw new InvalidOperationException(string.Join(", ", result.Errors.Select(e => e.Description)));
        }
    }
}