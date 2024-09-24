using ExpenseTracker.Application.Requests.Category;
using ExpenseTracker.Application.Requests.Transfer;
using ExpenseTracker.Application.Services.Interfaces;
using ExpenseTracker.Application.ViewModels.Transfer;
using ExpenseTracker.Mappings;
using ExpenseTracker.Stores.Interfaces;
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
    private readonly ICurrentUserService _currentUserService;

    public TransfersController(ITransferStore store, ICategoryStore categoryStore, ICurrentUserService currentUserService)
    {
        _store = store;
        _categoryStore = categoryStore;
        _currentUserService = currentUserService;
    }


    public IActionResult Index([FromQuery] CategoryRequest categoryRequest ,[FromQuery]GetTransfersRequest transfersRequest)
    {

        var result = _store.GetAll(transfersRequest , categoryRequest);
        var categories = new GetCategoriesRequest();
        categories.UserId=categoryRequest.UserId;
        categories.Search = "";
        
        ViewBag.Search = transfersRequest.Search;
        ViewBag.Categories = _categoryStore.GetAll(categories);
        if (categoryRequest.CategoryId ==0)
        {
            ViewBag.SelectedCategory = null;
        }
        else
        {
            ViewBag.SelectedCategory = _categoryStore.GetById(categoryRequest);
        }

        return View(result);
    }

    public IActionResult Details([FromQuery] TransferRequest request)
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

    public IActionResult Create()
    {
        var userId = _currentUserService.GetCurrentUserId();
        var category = new GetCategoriesRequest();
        category.UserId = userId;
        category.Search = "";

        var categories = _categoryStore.GetAll(category);
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

    public IActionResult Edit([FromQuery] TransferRequest transferRequest)
    {
        if (transferRequest?.TransferId == null)
        {
            return NotFound();
        }

        var viewModel = _store.GetById(transferRequest);

        if (viewModel is null)
        {
            return NotFound();
        }

        
        var category = new GetCategoriesRequest();
        category.UserId = transferRequest.UserId;
        category.Search = "";
        var categories = _categoryStore.GetAll(category);
        ViewBag.Categories = categories;

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, UpdateTransferRequest request)
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
        if (request?.TransferId== null)
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
    public ActionResult<TransferViewModel> GetTransfers(int? categoryId, string? search)
    {
        var result = _store.GetAll(null, null);

        return Ok(result);
    }

    private bool TransferExists(UpdateTransferRequest request)
    {
        var transfer = request.ToTransferRequest();

        return _store.GetById(transfer) is not null;
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
