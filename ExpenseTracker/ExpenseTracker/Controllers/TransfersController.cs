using ExpenseTracker.Application.Stores.Interfaces;
using ExpenseTracker.Application.ViewModels.Transfer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Controllers;
public class TransfersController : Controller
{
    public const int MaxFileSize = 2 * 1024 * 1024; // 2 MB
    public const int MinFileSize = 10 * 1024; // 10 kb

    private static readonly List<string> _allowedFileTypes =
    [
        "png", "jpeg", "gif", "pdf"
    ];

    private readonly ITransferStore _store;
    private readonly ICategoryStore _categoryStore;

    public TransfersController(ITransferStore store, ICategoryStore categoryStore)
    {
        _store = store;
        _categoryStore = categoryStore;
    }

    public IActionResult Index(int? categoryId, string? search)
    {

        var result = _store.GetAll(categoryId, search);

        ViewBag.Search = search;
        ViewBag.Categories = _categoryStore.GetAll(null);
        ViewBag.SelectedCategory = categoryId.HasValue ? _categoryStore.GetById(categoryId.Value) : null;

        return View(result);
    }

    public IActionResult Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var transfer = _store.GetById(id.Value);

        if (transfer is null)
        {
            return NotFound();
        }

        return View(transfer);
    }

    public IActionResult Create()
    {
        var categories = _categoryStore.GetAll("");
        var defaultCategory = categories.FirstOrDefault();

        ViewBag.Categories = categories;
        ViewBag.DefaultCategory = new { defaultCategory?.Id, defaultCategory?.Name };

        var model = new CreateTransferViewModel()
        {
            Date = DateTime.Now
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(CreateTransferViewModel transfer, List<IFormFile> attachments)
    {
        if (!ModelState.IsValid)
        {
            return RedirectToAction("Create");
        }

        foreach (var attachment in attachments)
        {
            if (!TryValidateFile(ModelState, attachment))
            {
                return View(transfer);
            }
        }

        var createdTransfer = _store.Create(transfer, attachments);

        return RedirectToAction(nameof(Details), new { id = createdTransfer.Id });
    }

    public IActionResult Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var viewModel = _store.GetForUpdate(id.Value);

        if (viewModel is null)
        {
            return NotFound();
        }

        var categories = _categoryStore.GetAll("");
        ViewBag.Categories = categories;

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, UpdateTransferViewModel transfer)
    {
        if (id != transfer.Id)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            return View(transfer);
        }

        try
        {
            _store.Update(transfer);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!TransferExists(transfer.Id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return RedirectToAction(nameof(Index));
    }

    public IActionResult Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var transfer = _store.GetById(id.Value);

        if (transfer is null)
        {
            return NotFound();
        }

        return View(transfer);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        var transfer = _store.GetById(id);

        if (transfer is null)
        {
            return NotFound();
        }

        _store.Delete(id);

        return RedirectToAction(nameof(Index));
    }

    /// <summary>
    /// Filters transfers
    /// </summary>
    /// <param name="categoryId"></param>
    /// <param name="search"></param>
    /// <returns>List of filtered transfers</returns>
    [Route("getTransfers")]
    public ActionResult<TransferViewModel> GetTransfers(int? categoryId, string? search)
    {
        var result = _store.GetAll(categoryId, search);

        return Ok(result);
    }

    private bool TransferExists(int id)
    {
        return _store.GetById(id) is not null;
    }

    private static bool TryValidateFile(ModelStateDictionary modelState, IFormFile? formFile)
    {
        if (formFile is null) // FIle is not required
        {
            return true;
        }

        if (formFile.Length < MinFileSize)
        {
            modelState.AddModelError(string.Empty, "Image file is too small.");

            return false;
        }

        if (formFile.Length > MaxFileSize)
        {
            modelState.AddModelError(string.Empty, "Image file is too big.");

            return false;
        }

        if (!_allowedFileTypes.Exists(type => formFile.ContentType.Contains(type)))
        {
            modelState.AddModelError(string.Empty, "Invalid image format");

            return false;
        }

        return true;
    }
}
