using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GllavicaInventari.Data;
using GllavicaInventari.Models;
using Microsoft.AspNetCore.Identity;

namespace GllavicaInventari.Controllers
{
    public class SuppliersController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public SuppliersController(
            AppDbContext context,
            UserManager<ApplicationUser> userManager
            )
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Suppliers
        public async Task<IActionResult> Index()
        {
            return View(await _context
                .Suppliers
                .Where(n => n.IsActive)
                .Include(n => n.Entries)
                .ToListAsync()
                );
        }

        // GET: Suppliers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var supplier = await _context
                .Suppliers
                .Where(n => n.IsActive)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (supplier == null)
            {
                return NotFound();
            }

            return View(supplier);
        }

        // GET: Suppliers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Suppliers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Contact,Address,Description")] Supplier supplier)
        {
            if (ModelState.IsValid)
            {
                supplier.DateCreated = DateTime.UtcNow.AddHours(2);
                supplier.LoggedInUserId = GetSignedInUser().FullName;

                _context.Add(supplier);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(supplier);
        }

        // GET: Suppliers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var supplier = await _context
                .Suppliers
                .Where(n => n.IsActive)
                .SingleOrDefaultAsync(m => m.Id == id);

            if (supplier == null) return NotFound();

            return View(supplier);
        }

        // POST: Suppliers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Contact,Address,Description")] Supplier supplier)
        {
            if (id != supplier.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(supplier);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SupplierExists(supplier.Id))
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
            return View(supplier);
        }

        // GET: Suppliers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var supplier = await _context
                .Suppliers
                .Where(n => n.IsActive)
                .SingleOrDefaultAsync(m => m.Id == id);

            if (supplier == null) return NotFound();

            return View(supplier);
        }

        // POST: Suppliers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var supplier = await _context.Suppliers.Where(n => n.IsActive).SingleOrDefaultAsync(m => m.Id == id);
            //_context.Suppliers.Remove(supplier);
            supplier.IsActive = false;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SupplierExists(int id)
        {
            return _context.Suppliers.Where(n => n.IsActive).Any(e => e.Id == id);
        }

        private ApplicationUser GetSignedInUser()
            => _userManager
            .Users
            .FirstOrDefault(a => a.Email == User.Identity.Name);
    }
}
