using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GllavicaInventari.Data;
using GllavicaInventari.Models;
using GllavicaInventari.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace GllavicaInventari.Controllers
{
    [Authorize(Roles = "Admin,Manager")]
    public class StockController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        public StockController(
            AppDbContext context,
            UserManager<ApplicationUser> userManager,
            IConfiguration configuration
            )
        {
            _context = context;
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<IActionResult> Index()
        {
            var startOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            List<Warehouse> wareHouses = null;

            ApplicationUser loggedInUser = GetSignedInUser();
            var loggedInUserRole = await _userManager.GetRolesAsync(loggedInUser);

            if ("manager".Equals(loggedInUserRole.First(), StringComparison.InvariantCultureIgnoreCase))
            {
                wareHouses = _context
                             .Warehouses
                             .Where(n => n.ApplicationUserId == loggedInUser.Id & n.IsActive)
                             .Include(n => n.ApplicationUser)
                             .Include(n => n.Entries)
                             .Include(n => n.Exits)
                             .ToList();
            }
            else
            {
                wareHouses = _context
                             .Warehouses
                             .Where(n => n.IsActive & n.IsActive)
                             .Include(n => n.ApplicationUser)
                             .Include(n => n.Entries)
                             .Include(n => n.Exits)
                             .ToList();
            }



            List<StockViewModel> stockVM = new List<StockViewModel>();

            wareHouses.ForEach(n => stockVM.Add(
                   new StockViewModel()
                   {
                       Id = n.Id,
                       Name = n.Name,
                       ManagerFullName = (n.ApplicationUser == null) ? "Nuk ka magazinier" : n.ApplicationUser.FullName,
                       Description = n.Description,
                       Address = n.Address,
                       EntriesCount = n.Entries.Where(m => m.DateEntry > startOfMonth.AddHours(23).AddMinutes(59).AddSeconds(59) & m.IsActive).GroupBy(m => m.SerialNumber).ToList().Count,
                       ExitsCount = n.Exits.Where(m => m.DateExit > startOfMonth.AddHours(23).AddMinutes(59).AddSeconds(59) & m.IsActive).GroupBy(m => m.SerialNumber).ToList().Count
                   }));

            wareHouses = null;

            return View(stockVM);
        }

        public IActionResult Details(int id, DateTime? dateStart, DateTime? dateEnd)
        {
            if (!dateStart.HasValue)
                dateStart = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            if (!dateEnd.HasValue)
                dateEnd = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

            ViewBag.DateStart = dateStart;
            ViewBag.DateEnd = dateEnd;

            var warehouse = _context
                .Warehouses
                .Include(n => n.Entries)
                .Include(n => n.Exits)
                .FirstOrDefault(n => n.Id == id & n.IsActive);

            var wVM = new WarehouseViewModel()
            {
                Id = id,
                Name = warehouse.Name,
                Entries = _context
                                .Entries
                                .Where(n => n.WareHouseId == id && n.DateEntry >= dateStart && n.DateEntry <= dateEnd.Value.AddHours(23).AddMinutes(59).AddSeconds(59) & n.IsActive)
                                .Include(n => n.Product)
                                .Include(n => n.Supplier)
                                .ToList(),
                Exits = _context
                                .Exits
                                .Where(n => n.WareHouseId == id && n.DateExit >= dateStart && n.DateExit <= dateEnd.Value.AddHours(23).AddMinutes(59).AddSeconds(59) & n.IsActive)
                                .Include(n => n.Product)
                                .Include(n => n.Supplier)
                                .ToList(),
                Stocks = new List<Stock>()
            };

            List<Stock> resultsEntries = wVM.Entries.GroupBy(d => d.ProductId)
                        .Select(
                            p => new Stock()
                            {
                                ProductId = p.First().ProductId,
                                ProductName = p.First().Product.Title,
                                SupplierName = p.First().Supplier.Name,
                                Unit = p.First().Product.Unit,
                                Amount = p.Sum(n => n.Amount),
                                Price = p.LastOrDefault().Price,
                                TotalValue = p.Sum(n => n.TotalValue),
                            }).ToList();

            List<Stock> resultsExits = wVM.Exits.GroupBy(d => d.ProductId)
                        .Select(
                            p => new Stock()
                            {
                                ProductId = p.First().ProductId,
                                ProductName = p.First().Product.Title,
                                SupplierName = p.First().Supplier.Name,
                                Unit = p.First().Product.Unit,
                                Amount = p.Sum(n => n.Amount),
                                Price = p.LastOrDefault().Price,
                                TotalValue = p.Sum(n => n.TotalValue),
                            }).ToList();

            foreach (Stock stock in resultsEntries)
            {
                var product = resultsExits.Where(n => n.ProductId == stock.ProductId).FirstOrDefault();
                stock.Amount -= (product == null) ? 0 : product.Amount;
                stock.TotalValue -= (product == null) ? 0 : product.TotalValue;
            }
            wVM.Stocks.AddRange(resultsEntries);


            return View(wVM);
        }

        private ApplicationUser GetSignedInUser()
            => _userManager
            .Users
            .FirstOrDefault(a => a.Email == User.Identity.Name);
    }
}