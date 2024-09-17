using ExpenseTracker.Application.ViewModels.Transfer;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Application.Store.Interfaces;

public interface ITransferStore
{
    List<TransferViewModel> GetAll(int? categoryId, string? search);
    TransferViewModel GetById(int id);
    UpdateTransferViewModel GetForUpdate(int id);
    TransferViewModel Create(CreateTransferViewModel transfer, IEnumerable<IFormFile> attachments);
    void Update(UpdateTransferViewModel transfer);
    void Delete(int id);
}
