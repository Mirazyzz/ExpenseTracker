using Bogus;
using ExpenseTracker.Domain.Entities;
using ExpenseTracker.Domain.Enums;
using ExpenseTracker.Infrastructure;

namespace ExpenseTracker.Extensions;

public static class DatabaseInitializer
{
    private readonly static Faker _faker = new();

    public static void SeedDatabase(ExpenseTrackerDbContext context)
    {
        CreateCategories(context);
        CreateTransfers(context);
    }

    private static void CreateCategories(ExpenseTrackerDbContext context)
    {
        return;
        if (context.Categories.Any()) return;

        for (int i = 0; i < 20; i++)
        {
            var randomType = _faker.Random.Enum<CategoryType>();
            var category = new Category
            {
                Type = randomType,
                Name = _faker.Lorem.Word(),
                Description = _faker.Lorem.Sentence(),
            };

            context.Add(category);
        }

        context.SaveChanges();
    }

    private static void CreateTransfers(ExpenseTrackerDbContext context)
    {
        return;
        if (context.Transfers.Any()) return;

        var categoryIds = context.Categories.Select(x => x.Id).ToList();

        foreach (var categoryId in categoryIds)
        {
            var numberOfTransfers = _faker.Random.Int(1, 10);
            for (int i = 0; i < numberOfTransfers; i++)
            {
                var transfer = new Transfer
                {
                    Amount = _faker.Random.Decimal(10, 5_000),
                    Date = _faker.Date.Between(DateTime.Now.AddDays(-7), DateTime.Now).ToUniversalTime(),
                    Note = _faker.Lorem.Sentence(),
                    CategoryId = categoryId,
                    Category = null,
                };

                context.Transfers.Add(transfer);
            }
        }

        context.SaveChanges();
    }
}
