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
using Microsoft.AspNetCore.Authorization;
using GllavicaInventari.ViewModels;

namespace GllavicaInventari.Controllers
{
    [Authorize(Roles = "Admin,Manager")]
    public class ExitsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ExitsController(
            AppDbContext context,
            UserManager<ApplicationUser> userManager
            )
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Exits
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
                var exits = _context
                .Exits
                .Where(n => n.LoggedInUserId == loggedInUser.Id & n.IsActive && n.DateExit >= dateStart && n.DateExit <= dateEnd.Value.AddHours(23).AddMinutes(59).AddSeconds(59))
                .Include(e => e.Product)
                .Include(e => e.Supplier)
                .Include(e => e.WareHouse);
                return View(await exits.ToListAsync());
            }
            else
            {
                var exits = _context
                .Exits
                .Where(n => n.IsActive && n.DateExit >= dateStart && n.DateExit <= dateEnd.Value.AddHours(23).AddMinutes(59).AddSeconds(59))
                .Include(e => e.Product)
                .Include(e => e.Supplier)
                .Include(e => e.WareHouse);
                return View(await exits.ToListAsync());
            }
        }

        public IActionResult BillDetails(string id)
        {
            string serialNumber = id;
            if (string.IsNullOrEmpty(serialNumber))
            {
                return NotFound();
            }

            var exits = _context
                .Exits
                .Where(n => n.SerialNumber == serialNumber & n.IsActive)
                .Include(e => e.Product)
                .Include(e => e.Supplier)
                .Include(e => e.WareHouse)
                .ToList();

            return View(exits);
        }

        // GET: Exits/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var exit = await _context.Exits
                .Include(e => e.Product)
                .Include(e => e.Supplier)
                .Include(e => e.WareHouse)
                .SingleOrDefaultAsync(m => m.Id == id & m.IsActive);

            if (exit == null)
                return NotFound();

            return View(exit);
        }

        // GET: Exits/Create
        public async Task<IActionResult> Create()
        {
            ExitsViewModel exitVM = new ExitsViewModel()
            {
                Products = _context.Products.Where(n => n.IsActive).ToList(),
                SerialNumber = DateTime.Now.ToString("yyyyMMddHHmmss")
            };

            ApplicationUser loggedInUser = GetSignedInUser();
            var loggedInUserRole = await _userManager.GetRolesAsync(loggedInUser);

            if ("manager".Equals(loggedInUserRole.First(), StringComparison.InvariantCultureIgnoreCase))
            {
                exitVM.Warehouses = _context.Warehouses.Where(n => n.ApplicationUserId == loggedInUser.Id & n.IsActive);
            }
            else
            {
                exitVM.Warehouses = _context.Warehouses.Where(n => n.IsActive);
            }

            return View(exitVM);
        }

        // POST: Entries/SaveExit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveExit()
        {
            int nrproduct = int.Parse(Request.Form["nrproduct"]);
            if (nrproduct > 0)
            {
                string[] quantities = Request.Form["Quantity[]"];
                string[] idproduct = Request.Form["Product[]"];
                string[] prices = Request.Form["Price[]"];
                string[] TVSHs = Request.Form["TVSH[]"];
                var warehouseId = int.Parse(Request.Form["warehouseid"]);
                //int supplierId = int.Parse(Request.Form["supplierid"]);
                var documentnumber = Request.Form["documentnumber"].First().ToString();
                bool hasTVSH = Convert.ToBoolean(Request.Form["hastvsh"].FirstOrDefault());

                for (int i = 0; i < nrproduct; i++)
                {
                    double amount = double.Parse(quantities[i]);
                    double price = double.Parse(prices[i]);
                    int productId = Int32.Parse(idproduct[i]);

                    int supplierId = _context.Entries
                       .Where(n => n.ProductId == productId)
                       .Include(n => n.Supplier)
                       .Select(n => n.SupplierId)
                       .FirstOrDefault();

                    Product product = _context.Products.FirstOrDefault(n => n.Id == productId);

                    Exit newExit = new Exit()
                    {
                        SerialNumber = documentnumber,
                        WareHouseId = warehouseId,
                        ProductId = productId,
                        SupplierId = supplierId,
                        Amount = amount,
                        Price = price,
                        TotalValue = amount * price,
                        HasTVSH = hasTVSH,
                        TotalValueWithTVSH = (hasTVSH) ? Math.Round(amount * price + amount * price * .20, 2) : Math.Round(amount * price, 2),
                        LoggedInUserId = GetSignedInUser().Id,
                        LoggedInUserFullName = GetSignedInUser().FullName,
                        DateExit = DateTime.UtcNow.AddHours(2)
                    };
                    _context.Exits.Add(newExit);
                    await _context.SaveChangesAsync();
                }
            }
            return RedirectToAction("Index");
        }
       
        // GET: Exits/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exit = await _context.Exits.Where(n => n.IsActive).SingleOrDefaultAsync(m => m.Id == id);
            if (exit == null)
            {
                return NotFound();
            }
            ViewData["Products"] = new SelectList(_context.Products.Where(n => n.IsActive), "Id", "Title", exit.ProductId);
            ViewData["Suppliers"] = new SelectList(_context.Suppliers.Where(n => n.IsActive), "Id", "Name", exit.SupplierId);
            ViewData["WareHouses"] = null;

            ApplicationUser loggedInUser = GetSignedInUser();
            var loggedInUserRole = await _userManager.GetRolesAsync(loggedInUser);

            if ("manager".Equals(loggedInUserRole.FirstOrDefault(), StringComparison.InvariantCultureIgnoreCase))
            {
                ViewData["WareHouses"] = new SelectList(_context.Warehouses.Where(n => n.ApplicationUserId == loggedInUser.Id & n.IsActive), "Id", "Name");
            }
            else
            {
                ViewData["WareHouses"] = new SelectList(_context.Warehouses.Where(n => n.IsActive), "Id", "Name");
            }
            return View(exit);
        }

        // POST: Exits/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Amount,Price,ProductId,HasTVSH")] Exit exit)
        {
            if (id != exit.Id) return NotFound();
            ApplicationUser loggedInUser = GetSignedInUser();
            var loggedInUserRole = await _userManager.GetRolesAsync(loggedInUser);

            if (ModelState.IsValid)
            {
                try
                {
                    //Find the old exit
                    Exit oldExit = _context.Exits.Where(n => n.Id == exit.Id & n.IsActive).FirstOrDefault();
                    oldExit.ProductId = exit.ProductId;
                    oldExit.Amount = exit.Amount;
                    oldExit.Price = exit.Price;
                    oldExit.TotalValue = Math.Round(exit.Amount * exit.Price, 2);

                    Product product = _context.Products.FirstOrDefault(n => n.Id == exit.ProductId);

                    if (oldExit.HasTVSH)
                        oldExit.TotalValueWithTVSH = Math.Round(oldExit.TotalValue + oldExit.TotalValue * 0.20, 2);
                    else
                        oldExit.TotalValueWithTVSH = oldExit.TotalValue;

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExitExists(exit.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["Products"] = new SelectList(_context.Products.Where(n => n.IsActive), "Id", "Title");
            ViewData["Suppliers"] = new SelectList(_context.Suppliers.Where(n => n.IsActive), "Id", "Name");
            ViewData["WareHouses"] = null;

            if ("manager".Equals(loggedInUserRole.First(), StringComparison.InvariantCultureIgnoreCase))
            {
                ViewData["WareHouses"] = new SelectList(_context.Warehouses.Where(n => n.ApplicationUserId == loggedInUser.Id & n.IsActive), "Id", "Name");
            }
            else
            {
                ViewData["WareHouses"] = new SelectList(_context.Warehouses.Where(n => n.IsActive), "Id", "Name");
            }

            return View(exit);
        }

        // GET: Exits/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exit = await _context.Exits
                .Include(e => e.Product)
                .Include(e => e.Supplier)
                .Include(e => e.WareHouse)
                .SingleOrDefaultAsync(m => m.Id == id & m.IsActive);
            if (exit == null)
            {
                return NotFound();
            }

            return View(exit);
        }

        // POST: Exits/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var exit = await _context.Exits.SingleOrDefaultAsync(m => m.Id == id);
            //_context.Exits.Remove(exit);
            exit.IsActive = false;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ExitExists(int id)
        {
            return _context.Exits.Where(n => n.IsActive).Any(e => e.Id == id);
        }

        [HttpPost]
        public ActionResult ConfirmExit(ExitViewModel model) => Ok(GetStateAmount(model));

        [HttpPost]
        public ActionResult ConfirmAmount(ExitViewModel model) => Ok(GetStateAmount(model) - model.amount);


        public double GetStateAmount(ExitViewModel model)
        {
            var entriesSum = _context.Entries
                .Where(n => n.ProductId == model.productId & n.WareHouseId == model.warehouseId & n.IsActive)
                .Select(n => n.Amount)
                .Sum();
            var exitsSum = _context.Exits
                .Where(n => n.ProductId == model.productId & n.WareHouseId == model.warehouseId & n.IsActive)
                .Select(n => n.Amount)
                .Sum();

            var state = entriesSum - exitsSum;
            return state;
        }

        public double GetAvailableAmount(int productId, int warehouseId)
        {
            var entriesSum = _context.Entries
               .Where(n => n.ProductId == productId & n.WareHouseId == warehouseId & n.IsActive)
               .Select(n => n.Amount)
               .Sum();
            var exitsSum = _context.Exits
                .Where(n => n.ProductId == productId & n.WareHouseId == warehouseId & n.IsActive)
                .Select(n => n.Amount)
                .Sum();

            var state = entriesSum - exitsSum;
            return state;
        }

        private ApplicationUser GetSignedInUser()
            => _userManager
            .Users
            .FirstOrDefault(a => a.Email == User.Identity.Name);
    }

    public class ExitViewModel
    {
        public int productId { get; set; }
        public int warehouseId { get; set; }
        public int amount { get; set; }

    }
}
