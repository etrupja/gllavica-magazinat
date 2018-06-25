using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GllavicaInventari.Data;
using GllavicaInventari.Models;
using GllavicaInventari.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace GllavicaInventari.Controllers
{
    public static class DateExtensions
    {
        public static DateTime TrimMilliseconds(this DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second, 0);
        }
    }

    [Authorize(Roles = "Admin,Manager")]
    public class TransfersController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public TransfersController(
            AppDbContext context,
            UserManager<ApplicationUser> userManager
            )
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Transfers
        public async Task<IActionResult> Index()
        {
            ApplicationUser loggedInUser = GetSignedInUser();
            var loggedInUserRole = await _userManager.GetRolesAsync(loggedInUser);

            if ("manager".Equals(loggedInUserRole.First(), StringComparison.InvariantCultureIgnoreCase))
            {
                var transfers = _context
                    .Transfers
                    .Where(n => n.LoggedInUserId == loggedInUser.Id & n.IsActive)
                .Include(t => t.Product)
                .Include(n => n.FromWareHouse)
                .Include(n => n.ToWareHouse);

                return View(await transfers.ToListAsync());
            }
            else
            {
                var transfers = _context
                   .Transfers
                   .Where(n => n.IsActive)
               .Include(t => t.Product)
               .Include(n => n.FromWareHouse)
               .Include(n => n.ToWareHouse);

                return View(await transfers.ToListAsync());
            }
        }

        // GET: Transfers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var transfer = await _context.Transfers
                .Where(n => n.IsActive)
                .Include(t => t.Product)
                .Include(t => t.ToWareHouse)
                .Include(t => t.FromWareHouse)
                .SingleOrDefaultAsync(m => m.Id == id);

            if (transfer == null) return NotFound();

            return View(transfer);
        }

        // GET: Transfers/Create
        public async  Task<IActionResult> Create()
        {
            ApplicationUser loggedInUser = GetSignedInUser();
            var loggedInUserRole = await _userManager.GetRolesAsync(loggedInUser);

            ViewData["Products"] = new SelectList(_context.Products.Where(n => n.IsActive), "Id", "Title");
            ViewData["FromWareHouses"] = null;
            ViewData["ToWareHouses"] = new SelectList(_context.Warehouses.Where(n => n.IsActive), "Id", "Name");
            
            if ("manager".Equals(loggedInUserRole.First(), StringComparison.InvariantCultureIgnoreCase))
            {
                ViewData["FromWareHouses"] = new SelectList(_context.Warehouses.Where(n => n.ApplicationUserId == loggedInUser.Id & n.IsActive), "Id", "Name");
            }
            else
            {
                ViewData["FromWareHouses"] = new SelectList(_context.Warehouses.Where(n => n.IsActive), "Id", "Name");
            }
            return View();
        }

        // POST: Transfers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TransferCode,Amount,DateTranfer,FromWareHouseId,ToWareHouseId,ProductId")] Transfer transfer)
        {
            if (ModelState.IsValid)
            {
                //find the product being transfered
                var product = _context.Entries
                    .Where(n => n.ProductId == transfer.ProductId & n.WareHouseId == transfer.FromWareHouseId & n.IsActive)
                    .OrderByDescending(n => n.DateEntry)
                    .Select(n => new { n.Price, n.SupplierId }).FirstOrDefault();

                var loggedInUser = GetSignedInUser();
                DateTime transferTime = DateTime.UtcNow.AddHours(2);

                transfer.Price = product.Price;
                transfer.TotalValue = product.Price * transfer.Amount;
                transfer.DateTranfer = transferTime;
                transfer.LoggedInUserId = loggedInUser.Id;
                transfer.LoggedInUserFullName = loggedInUser.FullName;
                transfer.IsActive = true;

                _context.Add(transfer);
                await _context.SaveChangesAsync();

                //Create an Exit and an Entry
                Entry newEntry = new Entry()
                {
                    SerialNumber = transfer.TransferCode,
                    Amount = transfer.Amount,
                    Price = product.Price,
                    TotalValue = transfer.Amount * product.Price,
                    DateEntry = transferTime,
                    LoggedInUserId = loggedInUser.Id,
                    LoggedInUserFullName = loggedInUser.FullName,
                    IsActive = true,
                    WareHouseId = transfer.ToWareHouseId.Value,
                    ProductId = transfer.ProductId,
                    SupplierId = product.SupplierId,
                    IsTransfer = true
                };
                _context.Entries.Add(newEntry);

                Exit newExit = new Exit()
                {
                    SerialNumber = transfer.TransferCode,
                    Amount = transfer.Amount,
                    Price = product.Price,
                    TotalValue = transfer.Amount * product.Price,
                    DateExit = transferTime,
                    LoggedInUserId = loggedInUser.Id,
                    LoggedInUserFullName = loggedInUser.FullName,
                    WareHouseId = transfer.FromWareHouseId.Value,
                    ProductId = transfer.ProductId,
                    SupplierId = product.SupplierId,
                    IsTransfer = true,
                };
                _context.Exits.Add(newExit);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            ViewData["Products"] = new SelectList(_context.Products.Where(n => n.IsActive), "Id", "Title", transfer.ProductId);
            ViewData["FromWareHouses"] = new SelectList(_context.Warehouses.Where(n => n.IsActive), "Id", "Name", transfer.FromWareHouseId);
            ViewData["ToWareHouses"] = new SelectList(_context.Warehouses.Where(n => n.IsActive), "Id", "Name", transfer.ToWareHouseId);
            return View(transfer);
        }

        // GET: Transfers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transfer = await _context.Transfers.Where(n => n.IsActive).SingleOrDefaultAsync(m => m.Id == id);
            if (transfer == null)
            {
                return NotFound();
            }
            ViewData["Products"] = new SelectList(_context.Products.Where(n => n.IsActive), "Id", "Title", transfer.ProductId);
            ViewData["FromWareHouses"] = new SelectList(_context.Warehouses.Where(n => n.IsActive), "Id", "Name", transfer.FromWareHouseId);
            ViewData["ToWareHouses"] = new SelectList(_context.Warehouses.Where(n => n.IsActive), "Id", "Name", transfer.ToWareHouseId);
            return View(transfer);
        }

        // POST: Transfers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DateTranfer,TransferCode,Amount,FromWareHouseId,ToWareHouseId,ProductId")] Transfer transfer)
        {
            if (id != transfer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //find the product if it has changed
                    double productPrice = _context.Entries
                        .Where(n => n.ProductId == transfer.ProductId & n.WareHouseId == transfer.FromWareHouseId && n.IsActive)
                        .OrderByDescending(n => n.DateEntry).Select(n => n.Price).FirstOrDefault();


                    transfer.Price = productPrice;
                    transfer.TotalValue = productPrice * transfer.Amount;
                    transfer.LoggedInUserId = GetSignedInUser().Id;
                    transfer.LoggedInUserFullName = GetSignedInUser().FullName;
                    transfer.IsActive = true;
                    _context.SaveChanges();


                    //update the entries and exits
                    Entry entry = _context
                        .Entries
                        .Where(n => n.DateEntry.TrimMilliseconds() == transfer.DateTranfer.TrimMilliseconds() && n.IsTransfer && n.IsActive)
                        .FirstOrDefault();
                    entry.SerialNumber = transfer.TransferCode;
                    entry.Amount = transfer.Amount;
                    entry.Price = productPrice;
                    entry.TotalValue = transfer.Amount * productPrice;
                    entry.WareHouseId = transfer.ToWareHouseId.Value;
                    entry.ProductId = transfer.ProductId;

                    Exit exit = _context
                        .Exits
                        .Where(n => n.DateExit.TrimMilliseconds() == transfer.DateTranfer.TrimMilliseconds() && n.IsTransfer && n.IsActive)
                        .FirstOrDefault();
                    exit.SerialNumber = transfer.TransferCode;
                    exit.Amount = transfer.Amount;
                    exit.Price = productPrice;
                    exit.TotalValue = transfer.Amount * productPrice;
                    exit.WareHouseId = transfer.FromWareHouseId.Value;
                    exit.ProductId = transfer.ProductId;
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TransferExists(transfer.Id))
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
            ViewData["Products"] = new SelectList(_context.Products.Where(n => n.IsActive), "Id", "Title", transfer.ProductId);
            ViewData["FromWareHouses"] = new SelectList(_context.Warehouses.Where(n => n.IsActive), "Id", "Name", transfer.FromWareHouseId);
            ViewData["ToWareHouses"] = new SelectList(_context.Warehouses.Where(n => n.IsActive), "Id", "Name", transfer.ToWareHouseId);
            return View(transfer);
        }



        // GET: Transfers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transfer = await _context.Transfers
                .Where(n => n.IsActive)
                .Include(t => t.Product)
                .Include(t => t.ToWareHouse)
                .Include(t => t.FromWareHouse)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (transfer == null)
            {
                return NotFound();
            }

            return View(transfer);
        }

        // POST: Transfers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var transfer = await _context.Transfers.Where(n => n.IsActive).SingleOrDefaultAsync(m => m.Id == id);

            //Find Entries and Exits
            Entry entry = _context
                        .Entries
                        .Where(n => n.DateEntry.TrimMilliseconds() == transfer.DateTranfer.TrimMilliseconds() && n.IsTransfer && n.IsActive)
                        .FirstOrDefault();
            Exit exit = _context
                        .Exits
                        .Where(n => n.DateExit.TrimMilliseconds() == transfer.DateTranfer.TrimMilliseconds() && n.IsTransfer && n.IsActive)
                        .FirstOrDefault();


            //_context.Entries.Remove(entry);
            //_context.Exits.Remove(exit);
            //_context.Transfers.Remove(transfer);
            entry.IsActive = false;
            exit.IsActive = false;
            transfer.IsActive = false;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TransferExists(int id)
        {
            return _context.Transfers.Where(n => n.IsActive).Any(e => e.Id == id);
        }

        [HttpPost]
        public ActionResult ConfirmTransfer(TransferViewModel model) => Ok(GetStateAmount(model));

        [HttpPost]
        public ActionResult ConfirmAmount(TransferViewModel model) => Ok(GetStateAmount(model) - model.Amount);


        public double GetStateAmount(TransferViewModel model)
        {
            var entriesSum = _context.Entries
                .Where(n => n.ProductId == model.ProductId & n.WareHouseId == model.FromWareHouseId && n.IsActive)
                .Select(n => n.Amount)
                .Sum();
            var exitsSum = _context.Exits
                .Where(n => n.ProductId == model.ProductId & n.WareHouseId == model.FromWareHouseId && n.IsActive)
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
}
