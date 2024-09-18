using ExpenseTracker.Application.Mappings;
using ExpenseTracker.Application.Stores.Interfaces;
using ExpenseTracker.Application.ViewModels.Category;
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

    public IActionResult Index(string? search)
    {
        var result = _store.GetAll(search);
        ViewBag.Search = search;

        return View(result);
    }

    public IActionResult Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var result = _store.GetById(id.Value);

        return View(result);
    }

    public IActionResult Create()
    {
       
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(CreateCategoryViewModel category)
    {
        if (!ModelState.IsValid)
        {
            return View(category);
        }

        var createdCategory = _store.Create(category);

        return RedirectToAction(nameof(Index));
    }

    public IActionResult Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var category = _store.GetById(id.Value);

        if (category is null)
        {
            return NotFound();
        }

        var viewModel = category.ToUpdateViewModel();

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, UpdateCategoryViewModel category)
    {
        if (id != category.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _store.Update(category);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(category.Id))
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
        return View(category);
    }

    public IActionResult Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var category = _store.GetById(id.Value);

        if (category is null)
        {
            return NotFound();
        }

        return View(category);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        var category = _store.GetById(id);

        if (category is null)
        {
            return NotFound();
        }

        _store.Delete(id);
        return RedirectToAction(nameof(Index));
    }

    /// <summary>
    /// Filters categories
    /// </summary>
    /// <param name="search"></param>
    /// <returns>List of filtered categories</returns>
    [Route("getCategories")]
    public ActionResult<CategoryViewModel> GetCategories(string? search)
    {
        var result = _store.GetAll(search);

        return Ok(result);
    }

    private bool CategoryExists(int id)
    {
        return _store.GetById(id) is not null;
    }
}
