using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "customers",
    pattern: "customers/{action=Index}/{id?}",
    defaults: new { controller = "Customers" });

app.MapControllerRoute(
    name: "categories",
    pattern: "categories/{action=Index}/{id?}",
    defaults: new { controller = "Categories" });

app.MapControllerRoute(
    name: "equipment",
    pattern: "equipment/{action=Index}/{id?}",
    defaults: new { controller = "Equipment" });

app.MapControllerRoute(
    name: "inventory",
    pattern: "inventory/{action=Index}/{id?}",
    defaults: new { controller = "Inventory" });

app.MapControllerRoute(
    name: "tariffs",
    pattern: "tariffs/{action=Index}/{id?}",
    defaults: new { controller = "Tariffs" });

app.MapControllerRoute(
    name: "rentals",
    pattern: "rentals/{action=Index}/{id?}",
    defaults: new { controller = "Rentals" });

app.MapControllerRoute(
    name: "payments",
    pattern: "payments/{action=Index}/{id?}",
    defaults: new { controller = "Payments" });

app.MapControllerRoute(
    name: "defects",
    pattern: "defects/{action=Index}/{id?}",
    defaults: new { controller = "Defects" });


app.Run();
