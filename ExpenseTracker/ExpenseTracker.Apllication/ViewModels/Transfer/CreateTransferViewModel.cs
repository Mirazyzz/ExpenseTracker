using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Application.ViewModels.Transfer;

public class CreateTransferViewModel
{
    public string? Note { get; set; }

    [Range(1, double.MaxValue, ErrorMessage = "Invalid amount.")]
    public decimal Amount { get; set; }

    [Required(ErrorMessage = "Category id is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Invalid Category id.")]
    public int CategoryId { get; set; }

    [Required(ErrorMessage = "Date is required.")]
    public DateTime Date { get; set; } = DateTime.Now;

    public List<IFormFile> Images { get; set; }

    public CreateTransferViewModel()
    {
        Images = new List<IFormFile>();
    }
}
