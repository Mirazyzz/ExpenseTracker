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
        private readonly UserManager<ApplicationUser> _userManager;

        public UserStore(UserManager<ApplicationUser> userManager)
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

        public async Task<UserViewModel> Create(CreateUserRequest request, IFormFile? attachment)
        {
            var user = request.ToEntity();

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"Error creating user: {errors}");
            }

            if (attachment != null)
            {
                using var stream = new MemoryStream();
                await attachment.CopyToAsync(stream);
                user.Image.Data = stream.ToArray();
            }
            else
            {
                user.Image.Data = Array.Empty<byte>();
            }

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                var updateErrors = string.Join(", ", updateResult.Errors.Select(e => e.Description));
                throw new Exception($"Error updating user with image: {updateErrors}");
            }

            var userViewModel = user.ToViewModel();

            return userViewModel;
        }



        public async Task Update(UpdateUserRequest request, IFormFile? attachment)
        {
            var user = await _userManager.FindByIdAsync(request.Id.ToString());

            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID {request.Id} not found.");
            }

            user.UserName = request.UserName;
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.Birthdate = request.Birthdate;
            user.Email = request.Email;
            user.PhoneNumber = request.PhoneNumber;

            if (attachment != null)
            {
                using var stream = new MemoryStream();
                await attachment.CopyToAsync(stream);
                user.Image.Data = stream.ToArray();
            }

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
