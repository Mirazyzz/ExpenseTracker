using ExpenseTracker.Application.Requests.Category;
using ExpenseTracker.Application.Requests.Transfer;
using ExpenseTracker.Application.ViewModels.Transfer;
using ExpenseTracker.Domain.Entities;

namespace ExpenseTracker.Mappings;

public static class TransferMappings
{
    public static TransferViewModel ToViewModel(this Transfer transfer)
    {
        return new TransferViewModel
        {
            Id = transfer.Id,
            Note = transfer.Notes,
            Amount = transfer.Amount,
            Date = transfer.Date,
            Category = transfer.Category.ToViewModel(),
            Images = transfer.Images.Select(x => Convert.ToBase64String(x.Data)).ToList()
        };
    }



    public static Transfer ToEntity(this CreateTransferRequest transfer)
    {
        return new Transfer
        {
            Notes = transfer.Notes,
            Amount = transfer.Amount,
            Date = transfer.Date,
            CategoryId = transfer.CategoryId,
            Category = null,
            Wallet = null!
        };
    }
    public static Transfer ToEntity(this UpdateTransferRequest request)
    {
        return new Transfer
        {
            Notes = request.Notes,
            Amount = request.Amount,
            Date = request.Date,
            CategoryId = request.CategoryId,
            Category = null,
            Wallet = null!,
        };
    }

    public static TransferRequest ToTransferRequest(this UpdateTransferRequest request)
    {
        return new TransferRequest(request.UserId, request.Id);
    }

    public static GetCategoriesRequest ToGetCategoriesRequest(this GetTransfersRequest request)
    {
        return new GetCategoriesRequest(request.UserId, request.Search);
    }
    public static CategoryRequest ToCategoryRequest(this GetTransfersRequest request)
    {
        return new CategoryRequest(request.UserId, request.CategoryId);
    }
}
