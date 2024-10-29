using ExpenseTracker.Application.Requests.User;
using ExpenseTracker.Application.Stores.Interfaces;
using ExpenseTracker.Application.ViewModels.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ExpenseTracker.Application.Mappings;
using Microsoft.AspNetCore.Http;
using ExpenseTracker.Domain.Entities;

namespace ExpenseTracker.Application.Stores
{
    public class UserStore : IUserStore
    {
        private readonly UserManager<Account> _userManager;

        public UserStore(UserManager<Account> userManager)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        public async Task<List<UserViewModel>> GetAll(GetUserRequest request)
        {
            var users = _userManager.Users;

            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                users = users.Where(u => EF.Functions.Like(u.Email, $"%{request.Search}%")
                                         || EF.Functions.Like(u.UserName, $"%{request.Search}%"));
            }

            var userViewModels = await users
                .Select(user => user.ToViewModel())
                .ToListAsync();

            return userViewModels;
        }

        public async Task<UserViewModel> GetById(UserRequest request)
        {
            var user = await _userManager.FindByIdAsync(request.Id.ToString());

            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID {request.Id} not found.");
            }

            var userViewModel = user.ToViewModel();

            return userViewModel;
        }

        public async Task<UserViewModel> Create(CreateUserRequest request, IEnumerable<IFormFile> attachments)
        {
            var user = request.ToEntity();

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"Error creating user: {errors}");
            }

            var userViewModel = user.ToViewModel();

            return userViewModel;
        }

        public async Task Update(UpdateUserRequest request)
        {
            var user = await _userManager.FindByIdAsync(request.Id.ToString());

            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID {request.Id} not found.");
            }

            user.UserName = request.UserName;
            user.Email = request.Email;
            user.PhoneNumber = request.PhoneNumber;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"Error when updating user: {errors}");
            }
        }

        public async Task Delete(UserRequest request)
        {
            var user = await _userManager.FindByIdAsync(request.Id.ToString());

            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID {request.Id} not found.");
            }

            var result = await _userManager.DeleteAsync(user);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"Error when deleting user: {errors}");
            }
        }
    }
}
