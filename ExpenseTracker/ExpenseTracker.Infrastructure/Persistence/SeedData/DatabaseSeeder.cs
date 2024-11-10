using Bogus;
using ExpenseTracker.Domain.Entities;
using ExpenseTracker.Domain.Enums;

namespace ExpenseTracker.Infrastructure.Persistence.SeedData
{
    public static class DatabaseSeeder
    {
        private readonly static Faker _faker = new();

        public static void SeedAllData(ExpenseTrackerDbContext context)
        {
            try
            {
                SeedUsers(context);
                SeedWallets(context);
                SeedCategories(context);
                SeedTransfers(context);
                SeedWalletShares(context);
                SeedNotifications(context);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Database seeding error: {ex.Message}");
            }
        }

        private static void SeedUsers(ExpenseTrackerDbContext context)
        {
            if (context.Users.Any()) return;

            for (int i = 0; i < 10; i++)
            {
                var user = new ApplicationUser
                {
                    Id = Guid.NewGuid(),
                    UserName = _faker.Internet.UserName(),
                    Email = _faker.Internet.Email(),
                    FirstName = _faker.Name.FirstName(),
                    LastName = _faker.Name.LastName(),
                    Birthdate = _faker.Date.Past(30, DateTime.Now.AddYears(-18)),
                    Notifications = new List<Notification>()
                };

                context.Users.Add(user);
            }

            context.SaveChanges();
        }

        private static void SeedWallets(ExpenseTrackerDbContext context)
        {
            if (context.Wallets.Any()) return;

            var users = context.Users.ToList();

            for (int i = 0; i < 5; i++)
            {
                var wallet = new Wallet
                {
                    Name = $"{_faker.Finance.AccountName()} Wallet",
                    Description = _faker.Lorem.Sentence(),
                    Balance = _faker.Finance.Amount(100, 10000),
                    IsMain = i == 0, // Можно задать первый кошелек как основной
                    Owner = _faker.PickRandom(users)
                };

                context.Wallets.Add(wallet);
            }

            context.SaveChanges();
        }

        private static void SeedCategories(ExpenseTrackerDbContext context)
        {
            if (context.Categories.Any()) return;

            var users = context.Users.ToList();

            for (int i = 0; i < 20; i++)
            {
                var category = new Category
                {
                    Name = _faker.Commerce.Department(),
                    Description = _faker.Commerce.ProductDescription(),
                    Type = _faker.PickRandom<CategoryType>(),
                    User = _faker.PickRandom(users)
                };

                context.Categories.Add(category);
            }

            context.SaveChanges();
        }

        private static void SeedTransfers(ExpenseTrackerDbContext context)
        {
            if (context.Transfers.Any()) return;

            var wallets = context.Wallets.ToList();
            var categories = context.Categories.ToList();

            for (int i = 0; i < 50; i++)
            {
                var transfer = new Transfer
                {
                    Notes = _faker.Lorem.Sentence(),
                    Amount = _faker.Finance.Amount(-500, 500),
                    Date = _faker.Date.Past(),
                    Wallet = _faker.PickRandom(wallets),
                    Category = _faker.PickRandom(categories)
                };

                context.Transfers.Add(transfer);
            }

            context.SaveChanges();
        }

        private static void SeedWalletShares(ExpenseTrackerDbContext context)
        {
            if (context.WalletShares.Any()) return;

            var wallets = context.Wallets.ToList();
            var users = context.Users.ToList();

            foreach (var wallet in wallets)
            {
                for (int i = 0; i < 3; i++)
                {
                    var share = new WalletShare
                    {
                        Date = _faker.Date.Past(),
                        AccessType = _faker.PickRandom<WalletAccessType>(),
                        IsAccepted = _faker.Random.Bool(),
                        Wallet = wallet,
                        User = _faker.PickRandom(users.Where(u => u.Id != wallet.OwnerId).ToList())
                    };

                    context.WalletShares.Add(share);
                }
            }

            context.SaveChanges();
        }

        private static void SeedNotifications(ExpenseTrackerDbContext context)
        {
            if (context.Notifications.Any()) return;

            var users = context.Users.ToList();

            for (int i = 0; i < 30; i++)
            {
                var notification = new Notification
                {
                    Title = _faker.Lorem.Sentence(),
                    Body = _faker.Lorem.Paragraph(),
                    RedirectUrl = _faker.Internet.Url(),
                    IsRead = _faker.Random.Bool(),
                    User = _faker.PickRandom(users)
                };

                context.Notifications.Add(notification);
            }

            context.SaveChanges();
        }
    }
}
