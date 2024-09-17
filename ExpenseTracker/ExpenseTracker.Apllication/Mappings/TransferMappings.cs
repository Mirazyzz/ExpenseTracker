using ExpenseTracker.Application.ViewModels.Transfer;
using ExpenseTracker.Domain.Entities;

namespace ExpenseTracker.Application.Mappings;

public static class TransferMappings
{
    public static TransferViewModel ToViewModel(this Transfer transfer)
    {
        return new TransferViewModel()
        {
            Id = transfer.Id,
            Note = transfer.Note,
            Amount = transfer.Amount,
            Date = transfer.Date,
            Category = transfer.Category.ToViewModel(),
            Images = transfer.Images.Select(x => Convert.ToBase64String(x.Data)).ToList()
        };
    }
    public static UpdateTransferViewModel ToUpdateViewModel(this Transfer transfer)
    {
        return new UpdateTransferViewModel
        {
            Id = transfer.Id,
            Note = transfer.Note,
            Amount = transfer.Amount,
            Date = transfer.Date,
            CategoryId = transfer.Category.Id
        };
    }

    public static Transfer ToEntity(this CreateTransferViewModel transfer)
    {
        return new Transfer
        {
            Note = transfer.Note,
            Amount = transfer.Amount,
            Date = transfer.Date,
            CategoryId = transfer.CategoryId,
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
