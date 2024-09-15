﻿using ExpenseTracker.Stores.Interfaces;
using ExpenseTracker.Stores;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using ExpenseTracker.Infrastructure.Email.Interfaces;
using ExpenseTracker.Infrastructure.Email;
using ExpenseTracker.Infrastructure.Extensions;
using Syncfusion.Licensing;

namespace ExpenseTracker.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.RegisterInfrastructure(configuration);
        services.AddScoped<ICategoryStore, CategoryStore>();
        services.AddScoped<ITransferStore, TransferStore>();
        services.AddScoped<IEmailService, EmailService>();

        AddControllers(services);
        AddProviders(configuration);

        services.ConfigureApplicationCookie(options =>
        {
            options.Cookie.HttpOnly = true;
            options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
            options.LoginPath = "/Account/Login";
            options.AccessDeniedPath = "/Account/AccessDenied";
            options.SlidingExpiration = true;
        });

        return services;
    }

    private static void AddControllers(this IServiceCollection services)
    {
        services
            .AddControllersWithViews()
            .AddRazorRuntimeCompilation();

        services
            .AddControllers(options =>
            {
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
                options.Filters.Add(new AuthorizeFilter(policy));
            })
            .AddJsonOptions(x =>
            {
                x.JsonSerializerOptions.PropertyNamingPolicy = null;
            });
    }

    private static void AddProviders(IConfiguration configuration)
    {
        var syncfusionKey = configuration.GetValue<string>("SyncfusionKey")
            ?? throw new InvalidOperationException("Syncfusion key is not found.");

        SyncfusionLicenseProvider.RegisterLicense(syncfusionKey);
    }
}