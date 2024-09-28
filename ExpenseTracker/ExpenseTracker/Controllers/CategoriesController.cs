using ExpenseTracker.Application.Requests.Category;
using ExpenseTracker.Application.ViewModels.Category;
using ExpenseTracker.Stores.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Controllers;

public class CategoriesController : Controller
{
    private readonly ICategoryStore _store;

    public CategoriesController(ICategoryStore store)
    {
        _store = store;
    }

    public IActionResult Index([FromQuery] GetCategoriesRequest request)
    {
        var result = _store.GetAll(request);
        ViewBag.Search = request.Search;

        return View(result);
    }

    public IActionResult Details([FromRoute] CategoryRequest request)
    {
        if (request?.Id == null)
        {
            return RedirectToAction("NotFoundError", "Home");
        }

        var result = _store.GetById(request);

        return View(result);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(CreateCategoryRequest request)
    {
        if (!ModelState.IsValid)
        {
            return View(request);
        }

        var createdCategory = _store.Create(request);

        return RedirectToAction(nameof(Index));
    }

    public IActionResult Edit([FromRoute] CategoryRequest request)
    {
        if (request?.Id == null)
        {
            return NotFound();
        }

        var category = _store.GetById(request);

        if (category is null)
        {
            return NotFound();
        }

        return View(category);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, [FromBody] UpdateCategoryRequest request)
    {
        if (id != request.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _store.Update(request);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(request))
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
        return View(request);
    }

    public IActionResult Delete(int id, CategoryRequest request)
    {
        if (request?.Id == null)
        {
            return NotFound();
        }

        // request.CategoryId = id;

        var category = _store.GetById(request);

        if (category is null)
        {
            return NotFound();
        }

        return View(category);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed([FromRoute] CategoryRequest request)
    {
        var category = _store.GetById(request);

        if (category is null)
        {
            return NotFound();
        }

        _store.Delete(request);
        return RedirectToAction(nameof(Index));
    }

    /// <summary>
    /// Filters categories
    /// </summary>
    /// <param name="search"></param>
    /// <returns>List of filtered categories</returns>
    [Route("getCategories")]
    public ActionResult<CategoryViewModel> GetCategories(GetCategoriesRequest request)
    {
        var result = _store.GetAll(request);

        return Ok(result);
    }

    private bool CategoryExists(UpdateCategoryRequest request)
    {
        return false;
    }
}