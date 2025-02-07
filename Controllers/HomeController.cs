using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MyWebApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Storage;
using System.Threading.Tasks;

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
        ViewData["PaymentMethods"] = new SelectList(PaymentMethod.GetAllPaymentMethods(), "Value", "Name");

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
    public async Task<IActionResult> Create(RentalCreateViewModel rentalViewModel, decimal paymentAmount)
    {
        if (ModelState.IsValid)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Customer customer = null;
                    if (rentalViewModel.isNewCustomer)
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

                    customer = rentalViewModel.isNewCustomer ? customer : await _context.Customers.FindAsync(rentalViewModel.CustomerId);

                    var equipment = await _context.Inventory
                        .Where(i => i.Id == rentalViewModel.InventoryId)
                        .Select(i => i.Equipment)
                        .FirstOrDefaultAsync();

                    decimal totalPrice = 0;
                    if (equipment != null)
                    {
                        Tariff tariff = await _context.Tariffs.FindAsync(rentalViewModel.TariffId);

                        totalPrice = tariff.DiscountAmount(equipment.BasePricePerDay);
                    }

                    Rental rental = new Rental{
                        CustomerId = customer.Id,
                        InventoryId = equipment.Id,
                        TariffId = rentalViewModel.TariffId,
                        StartDate = DateTime.Now,
                        EndDate = DateTime.Now,
                        TotalPrice = totalPrice,
                    };

                    _context.Rentals.Add(rental);

                    await _context.SaveChangesAsync();

                    var payment = new Payment
                    {
                        RentalId = rental.Id,
                        PaymentDate = DateTime.Now,
                        PaymentMethod = rentalViewModel.PaymentMethod,
                        Amount = 0
                    };
                    
                    _context.Add(payment);

                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();

                    ViewData["PaymentMethod"] = PaymentMethod.GetAllPaymentMethods().Find(item => item.Value == rentalViewModel.PaymentMethod);
                    ViewData["PaymentId"] = payment.Id;
                    
                    int rentalId = rental.Id;
                    rental = await _context.Rentals.Include(r => r.Inventory)
                        .ThenInclude(i => i.Equipment)
                        .FirstOrDefaultAsync(r => r.Id == rentalId);

                    return View("Payment", rental);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    ModelState.AddModelError("", "An error occurred while processing your request.");
                    
                }
            }
        }

        return RedirectToAction(nameof(Index)); 
    }

    [HttpPost]
    public async Task<IActionResult> ApprovePayment(int RentalId)
    {
        Rental rental = await _context.Rentals.FindAsync(RentalId);
        rental.Status = "active";

        _context.Update(rental);
        await _context.SaveChangesAsync();
        
        return RedirectToAction(nameof(Index), "Rentals");
    }
}
