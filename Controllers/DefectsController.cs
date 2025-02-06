using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace MyWebApp.Controllers
{
    public class DefectsController : Controller
    {
        private readonly AppDbContext _context;

        public DefectsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Defects
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Defects.Include(d => d.Inventory);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Defects/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var defect = await _context.Defects
                .Include(d => d.Inventory)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (defect == null)
            {
                return NotFound();
            }

            return View(defect);
        }

        // GET: Defects/Create
        public IActionResult Create()
        {
            ViewData["InventoryId"] = new SelectList(_context.Inventory, "InventoryId", "Condition");
            return View();
        }

        // POST: Defects/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,InventoryId,DefectDate,Description,Status,RepairCost")] Defect defect)
        {
            if (ModelState.IsValid)
            {
                _context.Add(defect);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["InventoryId"] = new SelectList(_context.Inventory, "InventoryId", "Condition", defect.InventoryId);
            return View(defect);
        }

        // GET: Defects/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var defect = await _context.Defects.FindAsync(id);
            if (defect == null)
            {
                return NotFound();
            }
            ViewData["InventoryId"] = new SelectList(_context.Inventory, "InventoryId", "Condition", defect.InventoryId);
            return View(defect);
        }

        // POST: Defects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,InventoryId,DefectDate,Description,Status,RepairCost")] Defect defect)
        {
            if (id != defect.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(defect);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DefectExists(defect.Id))
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
            ViewData["InventoryId"] = new SelectList(_context.Inventory, "InventoryId", "Condition", defect.InventoryId);
            return View(defect);
        }

        // GET: Defects/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var defect = await _context.Defects
                .Include(d => d.Inventory)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (defect == null)
            {
                return NotFound();
            }

            return View(defect);
        }

        // POST: Defects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var defect = await _context.Defects.FindAsync(id);
            if (defect != null)
            {
                _context.Defects.Remove(defect);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DefectExists(int id)
        {
            return _context.Defects.Any(e => e.Id == id);
        }
    }
}
