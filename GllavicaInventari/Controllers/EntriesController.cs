using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GllavicaInventari.Data;
using GllavicaInventari.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using GllavicaInventari.ViewModels;

namespace GllavicaInventari.Controllers
{
    [Authorize(Roles = "Admin,Manager")]
    public class EntriesController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public EntriesController(
            AppDbContext context,
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager
            )
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Entries
        public async Task<IActionResult> Index(DateTime? dateStart, DateTime? dateEnd)
        {
            ApplicationUser loggedInUser = GetSignedInUser();
            var loggedInUserRole = await _userManager.GetRolesAsync(loggedInUser);

            if (!dateStart.HasValue)
                dateStart = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            if (!dateEnd.HasValue)
                dateEnd = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

            ViewBag.DateStart = dateStart;
            ViewBag.DateEnd = dateEnd;

            if ("manager".Equals(loggedInUserRole.First(), StringComparison.InvariantCultureIgnoreCase))
            {
                var entries = _context
                    .Entries
                    .Where(n => n.LoggedInUserId == loggedInUser.Id && n.IsActive && n.DateEntry >= dateStart && n.DateEntry <= dateEnd.Value.AddHours(23).AddMinutes(59).AddSeconds(59))
                    .Include(e => e.Supplier)
                    .Include(n => n.Product)
                    .Include(e => e.WareHouse);
                return View(await entries.ToListAsync());
            }
            else
            {
                var entries = _context
                    .Entries
                    .Where(n => n.IsActive && n.DateEntry >= dateStart && n.DateEntry <= dateEnd.Value.AddHours(23).AddMinutes(59).AddSeconds(59))
                    .Include(e => e.Supplier)
                    .Include(e => e.WareHouse)
                    .Include(n => n.Product);
                return View(await entries.ToListAsync());
            }
        }



        // GET: Entries/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entry = await _context.Entries
                .Include(e => e.Product)
                .Include(e => e.Supplier)
                .Include(e => e.WareHouse)
                .SingleOrDefaultAsync(m => m.Id == id & m.IsActive);
            if (entry == null)
            {
                return NotFound();
            }

            return View(entry);
        }


        public IActionResult BillDetails(string id)
        {
            string serialNumber = id;
            if (string.IsNullOrEmpty(serialNumber))
            {
                return NotFound();
            }

            var entries = _context
                .Entries
                .Where(n => n.SerialNumber == serialNumber & n.IsActive)
                .Include(e => e.Product)
                .Include(e => e.Supplier)
                .Include(e => e.WareHouse)
                .ToList();

            return View(entries);
        }


        // GET: Entries/Create
        public async Task<IActionResult> Create()
        {
            EntryViewModel entryVM = new EntryViewModel()
            {
                Suppliers = _context.Suppliers.Where(n => n.IsActive).ToList(),
                Products = _context.Products.Where(n => n.IsActive).ToList(),
                SerialNumber = "",
                BillNumber = ""
            };

            ApplicationUser loggedInUser = GetSignedInUser();
            var loggedInUserRole = await _userManager.GetRolesAsync(loggedInUser);

            if ("manager".Equals(loggedInUserRole.First(), StringComparison.InvariantCultureIgnoreCase))
            {
                entryVM.Warehouses = _context.Warehouses.Where(n => n.ApplicationUserId == loggedInUser.Id & n.IsActive);
            }
            else
            {
                entryVM.Warehouses = _context.Warehouses.Where(n => n.IsActive);
            }


            return View(entryVM);
        }

        // POST: Entries/SaveEntry
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveEntry()
        {
            int nrproduct = int.Parse(Request.Form["nrproduct"]);
            if (nrproduct > 0)
            {
                string[] quantities = Request.Form["Quantity[]"];
                string[] idproduct = Request.Form["Product[]"]; 
                string[] prices = Request.Form["Price[]"]; 
                int supplierId = int.Parse(Request.Form["supplierid"]);
                var warehouseId = int.Parse(Request.Form["warehouseid"]);
                var billNumber = Request.Form["billnumber"].FirstOrDefault().ToString();
                var documentnumber = Request.Form["documentnumber"].FirstOrDefault().ToString();
                bool hasTVSH =Convert.ToBoolean(Request.Form["hastvsh"].FirstOrDefault());

                if (_context.Entries.Any(n=> n.SerialNumber == documentnumber))
                {
                    TempData["Error"] = "Ky numer serial ekziston! Ju lutem kontrollojeni numrin përsëri!";
                    return RedirectToAction("Create");
                }

                for (int i = 0; i < nrproduct; i++)
                {
                    double amount = double.Parse(quantities[i]);
                    double price = double.Parse(prices[i]);
                    int productId = Int32.Parse(idproduct[i]);
                    var product = _context.Products.FirstOrDefault(n => n.Id == productId);

                    Entry entry = new Entry()
                    {
                        SerialNumber = documentnumber,
                        BillNumber = billNumber,
                        SupplierId = supplierId,
                        WareHouseId = warehouseId,
                        ProductId = productId,
                        Amount = amount,
                        Price = price,
                        TotalValue = amount * price,
                        HasTVSH = hasTVSH,
                        TotalValueWithTVSH = (hasTVSH) ? (amount * price + amount * price * .20) : (amount * price),
                        LoggedInUserId = GetSignedInUser().Id,
                        LoggedInUserFullName = GetSignedInUser().FullName,
                        DateEntry = DateTime.UtcNow.AddHours(2)
                    };
                    _context.Entries.Add(entry);
                    await _context.SaveChangesAsync();
                }
            }
            return RedirectToAction("Index");
        }

        // GET: Entries/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (!id.HasValue) return NotFound();

            var entry = await _context.Entries.SingleOrDefaultAsync(m => m.Id == id & m.IsActive);
            if (entry == null)
            {
                return NotFound();
            }

            ViewData["Products"] = new SelectList(_context.Products.Where(n => n.IsActive), "Id", "Title");
            ViewData["Suppliers"] = new SelectList(_context.Suppliers.Where(n => n.IsActive), "Id", "Name");
            ViewData["WareHouses"] = null;

            ApplicationUser loggedInUser = GetSignedInUser();
            var loggedInUserRole = await _userManager.GetRolesAsync(loggedInUser);

            if ("manager".Equals(loggedInUserRole.First(), StringComparison.InvariantCultureIgnoreCase))
            {
                ViewData["WareHouses"] = new SelectList(_context.Warehouses.Where(n => n.ApplicationUserId == loggedInUser.Id & n.IsActive), "Id", "Name");
            }
            else
            {
                ViewData["WareHouses"] = new SelectList(_context.Warehouses.Where(n => n.IsActive), "Id", "Name");
            }

            return View(entry);
        }

        // POST: Entries/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Price,Amount,ProductId,HasTVSH")] Entry entry)
        {
            if (id != entry.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    //Find the old entry
                    Entry oldEntry = _context.Entries.Where(n => n.Id == entry.Id & n.IsActive).FirstOrDefault();
                    oldEntry.ProductId = entry.ProductId;
                    oldEntry.Amount = entry.Amount;
                    oldEntry.Price = entry.Price;
                    oldEntry.TotalValue = Math.Round(entry.Amount * entry.Price, 2);

                    if (oldEntry.HasTVSH)
                        oldEntry.TotalValueWithTVSH = Math.Round(oldEntry.TotalValue + oldEntry.TotalValue * 0.20,2);
                    else
                        oldEntry.TotalValueWithTVSH = oldEntry.TotalValue;

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EntryExists(entry.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["Products"] = new SelectList(_context.Products.Where(n => n.IsActive), "Id", "Title");
            ViewData["Suppliers"] = new SelectList(_context.Suppliers.Where(n => n.IsActive), "Id", "Name");
            ViewData["WareHouses"] = null;

            ApplicationUser loggedInUser = GetSignedInUser();
            var loggedInUserRole = await _userManager.GetRolesAsync(loggedInUser);

            if ("manager".Equals(loggedInUserRole.First(), StringComparison.InvariantCultureIgnoreCase))
            {
                ViewData["WareHouses"] = new SelectList(_context.Warehouses.Where(n => n.ApplicationUserId == loggedInUser.Id & n.IsActive), "Id", "Name");
            }
            else
            {
                ViewData["WareHouses"] = new SelectList(_context.Warehouses.Where(n => n.IsActive), "Id", "Name");
            }

            return View(entry);
        }

        // GET: Entries/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var entry = await _context.Entries
                .Include(e => e.Product)
                .Include(e => e.Supplier)
                .Include(e => e.WareHouse)
                .SingleOrDefaultAsync(m => m.Id == id & m.IsActive);

            if (entry == null) return NotFound();

            return View(entry);
        }

        // POST: Entries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var entry = await _context.Entries.SingleOrDefaultAsync(m => m.Id == id & m.IsActive);
            //_context.Entries.Remove(entry);
            entry.IsActive = false;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EntryExists(int id) => _context.Entries.Where(n => n.IsActive).Any(e => e.Id == id);

        private ApplicationUser GetSignedInUser()
            => _userManager
            .Users
            .FirstOrDefault(a => a.Email == User.Identity.Name);
    }
}
