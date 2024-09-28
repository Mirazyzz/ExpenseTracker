using ExpenseTracker.Domain.Enums;
using ExpenseTracker.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly ExpenseTrackerDbContext _context;

    public HomeController(ExpenseTrackerDbContext context)
    {
        _context = context;
    }
    public IActionResult Index()
    {
        PopulateWidgets();
        PopulateSplineChartData();
        PopulateDoughnutChart();
        PopulateRecentTransactions();

        return View();
    }

    [Route("Home/Error")]
    public IActionResult Error(int? statusCode = 500) =>
        statusCode switch
        {
            404 => View("NotFound"),
            _ => View("Error")
        };

    [Route("Home/NotFound")]
    public IActionResult NotFoundError() => View("NotFound");

    [Route("Home/InternalError")]
    public IActionResult InternalError() => View("Error");

    private void PopulateWidgets()
    {
        var totalIncome = _context.Transfers
            .Where(x => x.Category.Type == CategoryType.Income)
            .Sum(x => x.Amount);
        var totalExpense = _context.Transfers
            .Where(x => x.Category.Type == CategoryType.Expense)
            .Sum(x => x.Amount);

        ViewBag.Balance = totalIncome - totalExpense;
        ViewBag.TotalIncome = totalIncome;
        ViewBag.TotalExpense = totalExpense;
    }

    private void PopulateSplineChartData()
    {
        var allTransfers = _context.Transfers
            .Where(x => x.Date < DateTime.UtcNow && x.Date > DateTime.UtcNow.AddDays(-7))
            .Include(x => x.Category)
            .AsNoTracking()
            .ToList();

        //Income
        List<SplineChartData> incomeSummary = allTransfers
            .Where(x => x.Category.Type == CategoryType.Income)
            .GroupBy(j => j.Date)
            .Select(k => new SplineChartData()
            {
                day = k.First().Date.ToString("dd-MMM"),
                income = k.Sum(l => l.Amount)
            })
            .ToList();

        //Expense
        List<SplineChartData> expenseSummary = allTransfers
            .Where(x => x.Category.Type == CategoryType.Expense)
            .GroupBy(j => j.Date)
            .Select(k => new SplineChartData()
            {
                day = k.First().Date.ToString("dd-MMM"),
                expense = k.Sum(l => l.Amount)
            })
            .ToList();

        //Combine Income & Expense
        //Last 7 Days
        DateTime startDate = DateTime.UtcNow.AddDays(-6);
        DateTime endDate = DateTime.UtcNow;

        var last7Days = Enumerable.Range(0, 7)
            .Select(i => startDate.AddDays(i).ToString("dd-MMM"))
            .ToList();

        var data = from day in last7Days
                   join expense in expenseSummary on day equals expense.day into dayExpenseJoined
                   from totalExpense in dayExpenseJoined.DefaultIfEmpty()
                   join income in incomeSummary on day equals income.day into dayIncomeJoined
                   from totalIncome in dayIncomeJoined.DefaultIfEmpty()
                   select new
                   {
                       day = day,
                       income = totalIncome,
                       expense = totalExpense,
                   };

        ViewBag.SplineChartData = data
                        .Where(x => x.income != null && x.expense != null)
                        .GroupBy(x => x.day)
                        .Select(x => new
                        {
                            day = x.Key,
                            income = x.Sum(x => x.income.income),
                            expense = x.Sum(x => x.expense.expense)
                        })
                        .ToList();
    }

    private void PopulateDoughnutChart()
    {
        ViewBag.DoughnutChartData = _context.Transfers
            .Where(x => x.Category.Type == CategoryType.Expense)
                .GroupBy(j => j.Category.Id)
                .Select(k => new
                {
                    categoryTitleWithIcon = k.First().Category.Name,
                    amount = k.Sum(j => j.Amount),
                    formattedAmount = k.Sum(j => j.Amount).ToString("C0"),
                })
                .OrderByDescending(l => l.amount)
                .ToList();
    }

    private void PopulateRecentTransactions()
    {
        ViewBag.RecentTransfers = _context.Transfers
            .Include(x => x.Category)
            .AsNoTracking()
            .OrderByDescending(x => x.Date)
            .Take(10)
            .Select(x => new
            {
                Category = x.Category.Name,
                Amount = x.Amount,
                Date = x.Date
            })
            .ToList();
    }
}

public class SplineChartData
{
    public string day;
    public decimal income;
    public decimal expense;
}
