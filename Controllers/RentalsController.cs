using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace MyWebApp.Controllers
{
    public class RentalsController : Controller
    {
        private readonly AppDbContext _context;

        public RentalsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Rentals
        public async Task<IActionResult> Index()
        {
            
            var appDbContext = _context.Rentals
            .Include(r => r.Customer)
            .Include(r => r.Inventory)
            .ThenInclude(i => i.Equipment)
            .Include(r => r.Tariff);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Rentals/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            var rental = await _context.Rentals
                .Include(r => r.Customer)
                .Include(r => r.Inventory)
                .Include(r => r.Tariff)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rental == null)
            {
                return NotFound();
            }

            return View(rental);
        }

        // GET: Rentals/Create
        public IActionResult Create()
        {
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "FullName");
            ViewData["InventoryId"] = new SelectList(_context.Inventory, "Id", "Condition");
            ViewData["TariffId"] = new SelectList(_context.Tariffs, "Id", "TariffName");
            return View();
        }

        // POST: Rentals/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CustomerId,InventoryId,StartDate,EndDate,TariffId,TotalPrice,Status")] Rental rental)
        {
            if (ModelState.IsValid)
            {
                _context.Add(rental);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.CustomerId = new SelectList(_context.Customers, "Id", "FullName", rental.CustomerId);
            ViewBag.InventoryId = new SelectList(_context.Inventory, "Id", "Condition", rental.InventoryId);
            ViewBag.TariffId = new SelectList(_context.Tariffs, "Id", "TariffName", rental.TariffId);
            return View(rental);
        }

        // GET: Rentals/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rental = await _context.Rentals.FindAsync(id);
            if (rental == null)
            {
                return NotFound();
            }
            
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "FullName", rental.CustomerId);
            ViewData["InventoryId"] = new SelectList(_context.Inventory, "Id", "Condition", rental.InventoryId);
            ViewData["TariffId"] = new SelectList(_context.Tariffs, "Id", "TariffName", rental.TariffId);
            return View(rental);
        }

        // POST: Rentals/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CustomerId,InventoryId,StartDate,EndDate,TariffId,TotalPrice,Status")] Rental rental)
        {
            if (id != rental.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(rental);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RentalExists(rental.Id))
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
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "FullName", rental.CustomerId);
            ViewData["InventoryId"] = new SelectList(_context.Inventory, "Id", "Condition", rental.InventoryId);
            ViewData["TariffId"] = new SelectList(_context.Tariffs, "Id", "TariffName", rental.TariffId);
            return View(rental);
        }

        // GET: Rentals/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rental = await _context.Rentals
                .Include(r => r.Customer)
                .Include(r => r.Inventory)
                .Include(r => r.Tariff)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rental == null)
            {
                return NotFound();
            }

            return View(rental);
        }

        // POST: Rentals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var rental = await _context.Rentals.FindAsync(id);
            if (rental != null)
            {
                _context.Rentals.Remove(rental);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RentalExists(int id)
        {
            return _context.Rentals.Any(e => e.Id == id);
        }
    }
}
