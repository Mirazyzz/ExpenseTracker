using ExpenseTracker.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Domain.Entities;

public class Account : IdentityUser<Guid>
{
    public override Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime Birthday { get; set; }
    public Gender Gender { get; set; }
    public byte[] AccountImage { get; set; }
}
