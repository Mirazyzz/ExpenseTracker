using ExpenseTracker.Application.Requests.Transfer;
using ExpenseTracker.Application.ViewModels.Transfer;
using Microsoft.AspNetCore.Http;

namespace ExpenseTracker.Stores.Interfaces
{
    public interface ITransferStore
    {
        List<TransferViewModel> GetAll(GetTransfersRequest request);
        TransferViewModel GetById(TransferRequest request);
        TransferViewModel GetForUpdate(UpdateTransferRequest request);
        TransferViewModel Create(CreateTransferRequest request, IEnumerable<IFormFile> attachments);
        void Update(UpdateTransferRequest request);
        void Delete(TransferRequest request);
    }
}
