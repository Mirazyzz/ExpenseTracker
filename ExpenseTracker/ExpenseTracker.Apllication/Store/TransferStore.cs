using ExpenseTracker.Application.Mappings;
using ExpenseTracker.Application.Store.Interfaces;
using ExpenseTracker.Application.ViewModels.Transfer;
using ExpenseTracker.Domain.Entities;
using ExpenseTracker.Domain.Interfaces;
using Microsoft.AspNetCore.Http;

namespace ExpenseTracker.Application.Store;

public class TransferStore : ITransferStore
{
    private readonly ICommonRepository _repository;

    public TransferStore(ICommonRepository repository)
    {
        _repository = repository;
    }

    public List<TransferViewModel> GetAll(int? categoryId, string? search)
    {
        var transfers = _repository.Transfers.GetAll(categoryId, search);
        var viewModels = transfers
            .Select(x => x.ToViewModel()).ToList();

        return viewModels;
    }

    public TransferViewModel GetById(int id)
    {
        var transfer = _repository.Transfers.GetById(id);
        transfer.Images = _repository.ImageFiles.GetByTransferId(id);
        var viewModel = transfer.ToViewModel();

        return viewModel;
    }

    public UpdateTransferViewModel GetForUpdate(int id)
    {
        var transfer = _repository.Transfers.GetById(id);
        var viewModel = transfer.ToUpdateViewModel();

        return viewModel;
    }

    public TransferViewModel Create(CreateTransferViewModel transfer, IEnumerable<IFormFile> attachments)
    {
        ArgumentNullException.ThrowIfNull(transfer);

        var entity = transfer.ToEntity();

        var createdTransfer = _repository.Transfers.Create(entity);

        foreach (var image in attachments)
        {
            using var stream = new MemoryStream();
            image.CopyTo(stream);

            var imageFile = new ImageFile
            {
                Name = image.FileName,
                Type = image.ContentType,
                Data = stream.ToArray(),
                Transfer = createdTransfer,
            };

            _repository.ImageFiles.Create(imageFile);
        }

        _repository.SaveChanges(); // ACID -> Atomicity

        createdTransfer.Category = _repository.Categories.GetById(transfer.CategoryId);
        var viewModel = createdTransfer.ToViewModel();

        return viewModel;
    }

    public void Update(UpdateTransferViewModel transfer)
    {
        ArgumentNullException.ThrowIfNull(transfer);

        var entity = transfer.ToEntity();

        _repository.Transfers.Update(entity);
        _repository.SaveChanges();
    }
    public void Delete(int id)
    {
        _repository.Transfers.Delete(id);
        _repository.SaveChanges();
    }
}
