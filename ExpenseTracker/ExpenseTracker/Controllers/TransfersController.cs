using ExpenseTracker.Application.Requests.Common;
using ExpenseTracker.Application.Requests.Transfer;
using ExpenseTracker.Application.ViewModels.Transfer;
using ExpenseTracker.Mappings;
using ExpenseTracker.Stores.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Controllers;

public class TransfersController : Controller
{
    public const int MaxFileSize = 2 * 1024 * 1024;
    public const int MinFileSize = 10 * 1024;

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


    public IActionResult Index(GetTransfersRequest request)
    {
        var result = _store.GetAll(request);

        ViewBag.Search = request.Search;

        var getCategoriesRequest = request.ToGetCategoriesRequest();

        var categoryRequest = request.ToCategoryRequest();
        ViewBag.Categories = _categoryStore.GetAll(getCategoriesRequest);
        ViewBag.SelectedCategory = request?.CategoryId == null ? _categoryStore.GetById(categoryRequest) : null;

        return View(result);
    }

    public IActionResult Details(TransferRequest request)
    {
        if (request?.TransferId == null)
        {
            return NotFound();
        }

        var transfer = _store.GetById(request);

        if (transfer is null)
        {
            return NotFound();
        }

        return View(transfer);
    }

    public IActionResult Create(UserRequest request)
    {
        //var getCategories = new GetCategoriesRequest();
        //getCategories.UserId = request.UserId;
        //getCategories.Search = null;

        var categories = _categoryStore.GetAll(null!);
        var defaultCategory = categories.FirstOrDefault();

        ViewBag.Categories = categories;
        ViewBag.DefaultCategory = new { defaultCategory?.Id, defaultCategory?.Name };

        var model = new CreateTransferRequest(default, null, default, default, default)
        {
            Date = DateTime.Now
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(CreateTransferRequest request, List<IFormFile> attachments)
    {
        if (!ModelState.IsValid)
        {
            return RedirectToAction("Create");
        }

        foreach (var attachment in attachments)
        {
            if (!TryValidateFile(ModelState, attachment))
            {
                return View(request);
            }
        }

        var createdTransfer = _store.Create(request, attachments);

        return RedirectToAction(nameof(Details), new { id = createdTransfer.Id });
    }

    public IActionResult Edit([FromRoute] UpdateTransferRequest request)
    {
        if (request?.TransferId == null)
        {
            return NotFound();
        }

        var viewModel = _store.GetForUpdate(request);

        if (viewModel is null)
        {
            return NotFound();
        }

        var categories = _categoryStore.GetAll(null!);
        ViewBag.Categories = categories;

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int? id, [FromBody] UpdateTransferRequest request)
    {
        if (id != request.TransferId)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            return View(request);
        }

        try
        {
            _store.Update(request);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!TransferExists(request))
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

    public IActionResult Delete([FromRoute] TransferRequest request)
    {
        if (request?.TransferId == null)
        {
            return NotFound();
        }

        var transfer = _store.GetById(request);

        if (transfer is null)
        {
            return NotFound();
        }

        return View(transfer);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed([FromRoute] TransferRequest request)
    {
        var transfer = _store.GetById(request);

        if (transfer is null)
        {
            return NotFound();
        }

        _store.Delete(request);

        return RedirectToAction(nameof(Index));
    }

    /// <summary>
    /// Filters transfers
    /// </summary>
    /// <param name="categoryId"></param>
    /// <param name="search"></param>
    /// <returns>List of filtered transfers</returns>
    [Route("getTransfers")]
    public ActionResult<TransferViewModel> GetTransfers(GetTransfersRequest request)
    {
        var result = _store.GetAll(request);

        return Ok(result);
    }

    private bool TransferExists(UpdateTransferRequest request)
    {
        var transferRequest = request.ToTransferRequest();

        return _store.GetById(transferRequest) is not null;
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
