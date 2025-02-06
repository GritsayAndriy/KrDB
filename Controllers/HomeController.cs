using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MyWebApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MyWebApp.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly AppDbContext _context;

    public HomeController(ILogger<HomeController> logger, AppDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult Index()
    {
        ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "FullName");
        ViewData["InventoryId"] = new SelectList(_context.Inventory, "Id", "SerialNumber");
        ViewData["TariffId"] = new SelectList(_context.Tariffs, "Id", "TariffName");
        ViewData["PaymentMethods"] = new SelectList(PaymentMethod.GetAllPaymentMethods());

        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    // POST: Home/Create
    [HttpPost]
    // [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(RentalCreateViewModel rentalViewModel, decimal paymentAmount)
    {
        if (ModelState.IsValid)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Customer customer = null;
                    if (rentalViewModel.IsNotExistingCustomer)
                    {
                        customer = new Customer
                        {
                            FullName = rentalViewModel.NewCustomerFullName,
                            Phone = rentalViewModel.NewCustomerPhone,
                            Email = rentalViewModel.NewCustomerEmail,
                            Address = rentalViewModel.NewCustomerAddress,
                            PassportNumber = rentalViewModel.NewCustomerPassportNumber
                        };

                        _context.Customers.Add(customer);
                        await _context.SaveChangesAsync();
                    }

                    // Якщо є існуючий клієнт, беремо його Id
                    customer = rentalViewModel.IsNotExistingCustomer ? customer : await _context.Customers.FindAsync(rentalViewModel.CustomerId);

                    // Розрахунок загальної вартості оренди
                    var equipment = await _context.Inventory
                        .Where(i => i.Id == rentalViewModel.InventoryId)
                        .Select(i => i.Equipment)
                        .FirstOrDefaultAsync();

                    if (equipment != null)
                    {
                        // var totalPrice = (rentalViewModel.EndDate - rentalViewModel.StartDate).Days * equipment.BasePricePerDay;
                        // rentalViewModel.TotalPrice = totalPrice;
                    }

                    // Додаємо оренду
                    _context.Rentals.Add(rentalViewModel);

                    // Додаємо платіж
                    var payment = new Payment
                    {
                        RentalId = rentalViewModel.Id,
                        PaymentDate = DateTime.Now,
                        PaymentMethod = rentalViewModel.PaymentMethod, // Вказуємо спосіб оплати
                        // Amount = rentalViewModel.TotalPrice
                    };
                    _context.Add(payment);

                    // Зберігаємо оренду та платіж
                    await _context.SaveChangesAsync();

                    // Підтверджуємо транзакцію
                    await transaction.CommitAsync();

                    return RedirectToAction(nameof(Index)); // Перехід на головну сторінку оренд
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    ModelState.AddModelError("", "An error occurred while processing your request.");
                }
            }
        }

        // ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "FullName", rentalViewModel.CustomerId);
        // ViewData["InventoryId"] = new SelectList(_context.Inventory, "Id", "SerialNumber", rentalViewModel.InventoryId);
        // ViewData["TariffId"] = new SelectList(_context.Tariffs, "Id", "TariffName", rentalViewModel.TariffId);
        // ViewData["PaymentMethods"] = new SelectList(PaymentMethod.GetAllPaymentMethods(), rentalViewModel.PaymentMethod);


        return RedirectToAction(nameof(Index));
    }
}
