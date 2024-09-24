using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.Controllers;

public class ErrorPageController : Controller
{
    [Route("ErrorPage/{statuscode}")]
    public IActionResult Index(int statuscode)
    {
        switch (statuscode)
        {
            case 404:
                ViewData["Error"] = "Page Not Found";
                break;
            default:
                break;
        }
        return View("ErrorPage");
    }
}
