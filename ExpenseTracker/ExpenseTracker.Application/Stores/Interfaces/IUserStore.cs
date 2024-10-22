using ExpenseTracker.Application.Requests.User;
using ExpenseTracker.Application.ViewModels.User;
using Microsoft.AspNetCore.Http;

namespace ExpenseTracker.Application.Stores.Interfaces;

public interface IUserStore
{
    Task<List<UserViewModel>>  GetAll(GetUserRequest request);
    Task<UserViewModel>  GetById(UserRequest request);
    Task<UserViewModel> Create(CreateUserRequest request, IEnumerable<IFormFile> attachments);
    Task Update(UpdateUserRequest request);
    Task Delete(UpdateUserRequest request);
}