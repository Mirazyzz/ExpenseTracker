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
            Note = transfer.Note,
            Amount = transfer.Amount,
            Date = transfer.Date,
            Category = transfer.Category.ToViewModel(),
            Images = transfer.Images.Select(x => Convert.ToBase64String(x.Data)).ToList()
        };
    }

    public static TransferRequest ToTransferRequest(this UpdateTransferRequest updateTransferRequest)
    {
        return new TransferRequest
        {
            TransferId = updateTransferRequest.TransferId,
            UserId = updateTransferRequest.UserId,
        };
    }

    public static Transfer ToEntity(this CreateTransferRequest transfer)
    {
        return new Transfer
        {    
            CategoryId = transfer.CategoryId,
            UserId=transfer.UserId,
            Note = transfer.Note,
            Amount = transfer.Amount,
            Date = transfer.Date,
            Category = null,
        };
    }

    public static Transfer ToEntity(this UpdateTransferViewModel transfer)
    {
        return new Transfer
        {
            Id = transfer.Id,
            Note = transfer.Note,
            Amount = transfer.Amount,
            Date = transfer.Date,
            CategoryId = transfer.CategoryId,
            Category = null,
        };
    }
    
}
