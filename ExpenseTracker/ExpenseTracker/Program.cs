using ExpenseTracker.Extensions;
using ExpenseTracker.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureService(builder.Configuration);

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseStatusCodePagesWithReExecute("/Error/{0}");

    app.UseHsts();
}
else
{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<ExpenseTrackerDbContext>();
    DatabaseInitializer.SeedDatabase(context);
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();