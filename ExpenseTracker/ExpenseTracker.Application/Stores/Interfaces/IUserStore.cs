using ExpenseTracker.Application.Requests.Common;
using ExpenseTracker.Application.Requests.User;
using ExpenseTracker.Application.ViewModels.User;
using Microsoft.AspNetCore.Http;

namespace ExpenseTracker.Application.Stores.Interfaces;

public interface IUserStore
{
    Task<List<UserViewModel>> GetAll(GetUserRequest request);
    Task<UserViewModel> GetById(UserRequest request);
    Task<UserViewModel> Create(CreateUserRequest request, IFormFile attachment);
    Task Update(UpdateUserRequest request, IFormFile? attachment);
    Task Delete(UserRequest request);
}