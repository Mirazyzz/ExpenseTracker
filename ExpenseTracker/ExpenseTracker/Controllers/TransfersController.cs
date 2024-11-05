using ExpenseTracker.Application.Requests.Category;
using ExpenseTracker.Application.Requests.Common;
using ExpenseTracker.Application.Requests.Transfer;
using ExpenseTracker.Application.Requests.Wallet;
using ExpenseTracker.Application.Stores.Interfaces;
using ExpenseTracker.Application.ViewModels.Transfer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ExpenseTracker.Controllers;

public class TransfersController : Controller
{
    public const int MaxFileSize = 2 * 1024 * 1024;
    public const int MinFileSize = 1024;

    private static readonly List<string> _allowedFileTypes =
    [
        "png", "jpeg", "gif", "pdf"
    ];

    private readonly ITransferStore _transferStore;
    private readonly ICategoryStore _categoryStore;
    private readonly IWalletStore _walletStore;

    public TransfersController(ITransferStore store, ICategoryStore categoryStore, IWalletStore walletStore)
    {
        _transferStore = store ?? throw new ArgumentNullException(nameof(store));
        _categoryStore = categoryStore ?? throw new ArgumentNullException(nameof(categoryStore));
        _walletStore = walletStore ?? throw new ArgumentNullException(nameof(walletStore));
    }

    public IActionResult Index([FromQuery] GetTransfersRequest request)
    {
        var transfers = _transferStore.GetAll(request);
        var categories = _categoryStore.GetAll(new GetCategoriesRequest(request.UserId, null));
        var wallets = _walletStore.GetAll(new GetWalletsRequest(request.UserId, null));


        ViewBag.Search = request.Search;
        ViewBag.Categories = categories;
        ViewBag.SelectedCategory = request.CategoryId;
        ViewBag.Wallets = wallets;

        return View(transfers);
    }

    public IActionResult Details([FromRoute] TransferRequest request)
    {
        var transfer = _transferStore.GetById(request);

        return View(transfer);
    }

    public IActionResult Create([FromHeader] UserRequestId request)
    {
        PopulateViewBag(request);

        var model = new CreateTransferRequest(
            UserId: request.UserId,
            CategoryId: default,
            WalletId: default,
            Notes: default,
            Amount: default,
            Date: DateTime.UtcNow);

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create([FromForm] CreateTransferRequest request, [FromForm] List<IFormFile> attachments)
    {
        if (!ModelState.IsValid)
        {
            return RedirectToAction("Create");
        }

        foreach (var attachment in attachments)
        {
            if (!TryValidateFile(ModelState, attachment))
            {
                PopulateViewBag(request, request.CategoryId);

                return View(request);
            }
        }

        var createdTransfer = _transferStore.Create(request, attachments);

        return RedirectToAction(nameof(Details), new { id = createdTransfer.Id });
    }

    public IActionResult Edit([FromRoute] TransferRequest request)
    {
        var transfer = _transferStore.GetById(request);

        PopulateViewBag(request, transfer.Category.Id);

        return View(transfer);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit([FromRoute] int id, [FromBody] UpdateTransferRequest request)
    {
        if (id != request.Id)
        {
            return BadRequest("Route id does not match with body id.");
        }

        if (!ModelState.IsValid)
        {
            return View(request);
        }

        _transferStore.Update(request);

        return RedirectToAction(nameof(Index));
    }

    public IActionResult Delete([FromRoute] TransferRequest request)
    {
        var transfer = _transferStore.GetById(request);

        return View(transfer);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed([FromRoute] TransferRequest request)
    {
        _transferStore.Delete(request);

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
        var result = _transferStore.GetAll(request);

        return Ok(result);
    }

    private static bool TryValidateFile(ModelStateDictionary modelState, IFormFile? formFile)
    {
        if (formFile is null)
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

    private void PopulateViewBag(UserRequestId request, int? categoryId = null, int? walletId = null)
    {
        var categories = _categoryStore.GetAll(new GetCategoriesRequest(request.UserId, null));
        var wallets = _walletStore.GetAll(new GetWalletsRequest(request.UserId, null));

        var defaultCategory = categoryId.HasValue
            ? categories.First(x => x.Id == categoryId.Value)
            : categories.FirstOrDefault();
        var defaultWallet = walletId.HasValue
            ? wallets.First(x => x.Id == walletId)
            : wallets.FirstOrDefault();

        ViewBag.Wallets = wallets;
        ViewBag.Categories = categories;
        ViewBag.DefaultCategory = defaultCategory;
        ViewBag.DefaultWallet = defaultWallet;
    }
}
