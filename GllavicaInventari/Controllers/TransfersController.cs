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

        // GET: Transfers/BillDetails/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null) return NotFound();

        //    var transfer = await _context.Transfers
        //        .Where(n => n.IsActive)
        //        .Include(t => t.Product)
        //        .Include(t => t.ToWareHouse)
        //        .Include(t => t.FromWareHouse)
        //        .SingleOrDefaultAsync(m => m.Id == id);

        //    if (transfer == null) return NotFound();

        //    return View(transfer);
        //}

        // GET: Transfers/BillDetails/5
        public async Task<IActionResult> BillDetails(string id)
        {
            string serialNumber = id;
            if (string.IsNullOrEmpty(serialNumber))  return NotFound();

            var transfers =  await _context.Transfers
                .Where(n => n.SerialNumber == serialNumber)
                .Include(t => t.Product)
                .Include(t => t.ToWareHouse)
                .Include(t => t.FromWareHouse)
                .ToListAsync();

            return View(transfers);
        }

        // GET: Transfers/Create
        public async  Task<IActionResult> Create()
        {
            ApplicationUser loggedInUser = GetSignedInUser();
            var loggedInUserRole = await _userManager.GetRolesAsync(loggedInUser);
            
            NewTransferViewModel newTransferVM = new NewTransferViewModel()
            {
                Suppliers = _context.Suppliers.Where(n => n.IsActive).ToList(),
                Products = _context.Products.Where(n => n.IsActive).ToList(),
                ToWarehouses = _context.Warehouses.Where(n => n.IsActive).ToList(),
                SerialNumber = "",
                BillNumber = ""
            };
            

            if ("manager".Equals(loggedInUserRole.First(), StringComparison.InvariantCultureIgnoreCase))
                newTransferVM.FromWarehouses = _context.Warehouses.Where(n => n.ApplicationUserId == loggedInUser.Id & n.IsActive);
            else
                newTransferVM.FromWarehouses = _context.Warehouses.Where(n => n.IsActive);


            return View(newTransferVM);
        }

        // POST: Entries/SaveExit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveTransfer()
        {
            int nrproduct = int.Parse(Request.Form["nrproduct"]);
            if (nrproduct > 0)
            {
                string[] quantities = Request.Form["Quantity[]"];
                string[] idproduct = Request.Form["Product[]"];
                string[] prices = Request.Form["Price[]"];

                //int supplierId = int.Parse(Request.Form["supplierid"]);

                var fromWareHouseId = int.Parse(Request.Form["fromWareHouseId"]);
                var toWareHouseId = int.Parse(Request.Form["toWareHouseId"]);

                var billNumber = Request.Form["billnumber"].FirstOrDefault().ToString();
                var documentnumber = Request.Form["documentnumber"].FirstOrDefault().ToString();

                DateTime dateNow = DateTime.UtcNow.AddHours(2);
                string loggedInUserId = GetSignedInUser().Id;
                string loggedInUserFullName = GetSignedInUser().FullName;

                for (int i = 0; i < nrproduct; i++)
                {
                    double amount = double.Parse(quantities[i]);
                    double price = double.Parse(prices[i]);
                    int productId = Int32.Parse(idproduct[i]);

                    Product product = _context.Products.FirstOrDefault(n => n.Id == productId);

                    int supplierId = _context.Entries
                       .Where(n => n.ProductId == productId)
                       .Include(n => n.Supplier)
                       .Select(n => n.SupplierId)
                       .FirstOrDefault();

                    Exit newExit = new Exit()
                    {
                        SerialNumber = documentnumber,
                        WareHouseId = fromWareHouseId,
                        ProductId = productId,
                        SupplierId = supplierId,
                        Amount = amount,
                        Price = price,
                        TotalValue = amount * price,
                        TotalValueWithTVSH = (product.HasTVSH) ? Math.Round(amount * price + amount * price * .20, 2) : Math.Round(amount * price, 2),
                        LoggedInUserId = loggedInUserId,
                        LoggedInUserFullName = loggedInUserFullName,
                        DateExit = dateNow,
                        IsTransfer = true
                    };
                    _context.Exits.Add(newExit);
                    _context.SaveChanges();

                    Entry entry = new Entry()
                    {
                        SerialNumber = documentnumber,
                        BillNumber = billNumber,
                        SupplierId = supplierId,
                        WareHouseId = toWareHouseId,
                        ProductId = productId,
                        Amount = amount,
                        Price = price,
                        TotalValue = amount * price,
                        TotalValueWithTVSH = (product.HasTVSH) ? (amount * price + amount * price * .20) : (amount * price),
                        LoggedInUserId = loggedInUserId,
                        LoggedInUserFullName = loggedInUserFullName,
                        DateEntry = dateNow,
                        IsTransfer = true
                    };
                    _context.Entries.Add(entry);
                    _context.SaveChanges();


                    Transfer transfer = new Transfer()
                    {
                        SerialNumber = documentnumber,
                        BillNumber = billNumber,
                        Amount = amount,
                        Price = price,
                        TotalValue = Math.Round(amount*price, 2),
                        TotalValueWithTVSH = Math.Round(amount * price + amount * price * .20, 2),
                        DateTranfer = dateNow,
                        LoggedInUserId = loggedInUserId,
                        LoggedInUserFullName = loggedInUserFullName,
                        FromWareHouseId = fromWareHouseId,
                        ToWareHouseId = toWareHouseId,
                        ProductId = productId
                    };
                    _context.Transfers.Add(transfer);
                    _context.SaveChanges();
                }
            }
            return RedirectToAction("Index");
        }
        
        // GET: Transfers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transfer = await _context.Transfers.Include(n => n.FromWareHouse).Include(n => n.ToWareHouse).Where(n => n.IsActive).SingleOrDefaultAsync(m => m.Id == id);
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,Amount,Price,ProductId,SerialNumber")] Transfer model)
        {
            if (id != model.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var transfer = _context.Transfers.FirstOrDefault(n => n.Id == model.Id & n.IsActive);
                    var transferProduct = _context.Products.FirstOrDefault(n => n.Id == model.ProductId & n.IsActive);

                    transfer.ProductId = model.ProductId;
                    transfer.Amount = model.Amount;
                    transfer.Price = model.Price;
                    transfer.TotalValue = Math.Round(model.Price * model.Amount, 2);
                    transfer.TotalValueWithTVSH = (transferProduct.HasTVSH) ? Math.Round(model.Amount * model.Price + model.Amount * model.Price * .20, 2) : transfer.TotalValue;
                    await _context.SaveChangesAsync();

                    //update the entries and exits
                    Entry entry = _context.Entries.Where(n => n.SerialNumber == model.SerialNumber && n.IsActive && n.IsTransfer).FirstOrDefault();
                    entry.ProductId = model.ProductId;
                    entry.Amount = model.Amount;
                    entry.Price = model.Price;
                    entry.TotalValue = Math.Round(transfer.Amount * model.Price, 2);
                    entry.TotalValueWithTVSH = (transferProduct.HasTVSH) ? Math.Round(model.Amount * model.Price + model.Amount*model.Price*.20, 2) : entry.TotalValue;
                    await _context.SaveChangesAsync();

                    Exit exit = _context.Exits.Where(n => n.SerialNumber == model.SerialNumber && n.IsTransfer && n.IsActive).FirstOrDefault();
                    exit.ProductId = model.ProductId;
                    exit.Amount = transfer.Amount;
                    exit.Price = model.Price;
                    exit.TotalValue = Math.Round(model.Amount * model.Price, 2);
                    exit.TotalValueWithTVSH = (transferProduct.HasTVSH) ? Math.Round(model.Amount * model.Price + model.Amount * model.Price * .20, 2) : exit.TotalValue;
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TransferExists(model.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["Products"] = new SelectList(_context.Products.Where(n => n.IsActive), "Id", "Title", model.ProductId);
            ViewData["FromWareHouses"] = new SelectList(_context.Warehouses.Where(n => n.IsActive), "Id", "Name", model.FromWareHouseId);
            ViewData["ToWareHouses"] = new SelectList(_context.Warehouses.Where(n => n.IsActive), "Id", "Name", model.ToWareHouseId);
            return View(model);
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
