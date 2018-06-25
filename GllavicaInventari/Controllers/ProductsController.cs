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
    public class DropDownList
    {
        public int Id { get; set; }
        public string Value { get; set; }
    }
    public class ProductsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProductsController(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            return View(await _context
                .Products
                .Where(n => n.IsActive)
                .Include(n => n.Entries)
                .ToListAsync()
                );
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var product = await _context
                .Products
                .Where(n => n.IsActive)
                .SingleOrDefaultAsync(m => m.Id == id);

            if (product == null) return NotFound();

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Description,HasTVSH,Unit")] Product product)
        {
            if (ModelState.IsValid)
            {
                if (ProductNameExists(product.Title))
                {
                    TempData["Error"] = "Ky product ekziton. Ju lutem percaktoni nje emertim tjeter!";
                    return View();
                }

                product.DateCreated = DateTime.UtcNow.AddHours(2);
                product.LoggedInUserFullName = GetSignedInUser().FullName;

                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }


        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)  return NotFound();

            var product = await _context
                .Products
                .Where(n => n.IsActive)
                .SingleOrDefaultAsync(m => m.Id == id);

            if (product == null) return NotFound();

            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,HasTVSH,Unit")] Product product)
        {
            if (id != product.Id)  return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
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
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var product = await _context.Products
                .Where(n => n.IsActive)
                .SingleOrDefaultAsync(m => m.Id == id);

            if (product == null) return NotFound();

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context
                .Products
                .Where(n => n.IsActive)
                .SingleOrDefaultAsync(m => m.Id == id);

            product.IsActive = false;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id) => _context.Products.Where(n => n.IsActive).Any(e => e.Id == id);
        private bool ProductNameExists(string title) => _context.Products.Any(n => n.IsActive && n.Title.ToLower() == title.ToLower());

        private ApplicationUser GetSignedInUser()
            => _userManager
            .Users
            .FirstOrDefault(a => a.Email == User.Identity.Name);

    }
}
