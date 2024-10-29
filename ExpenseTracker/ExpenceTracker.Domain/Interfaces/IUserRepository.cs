using ExpenseTracker.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace ExpenseTracker.Domain.Interfaces;

public interface IUserRepository
{
    List<Account> GetAll();
    Account GetById(Guid id);
    Account? GetByEmail(string email);
}
