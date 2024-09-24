using ExpenseTracker.Application.Requests.Category;
using ExpenseTracker.Application.Requests.Transfer;
using ExpenseTracker.Application.ViewModels.Transfer;
using Microsoft.AspNetCore.Http;

namespace ExpenseTracker.Stores.Interfaces
{
    public interface ITransferStore
    {
        List<TransferViewModel> GetAll(GetTransfersRequest request, CategoryRequest category);
        TransferViewModel GetById(TransferRequest request);
        TransferViewModel Create(CreateTransferRequest transfer, IEnumerable<IFormFile> attachments);
        void Update(UpdateTransferRequest transfer);
        void Delete(TransferRequest request);
    }
}
