using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GllavicaInventari.Data;
using GllavicaInventari.Models;
using GllavicaInventari.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GllavicaInventari.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly AppDbContext _context;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, 
            AppDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        public IActionResult Index()
        {
            var users = _context.Users.Where(n => n.IsActive).Include(n=>n.Warehouses).ToList();
            return View(users);
        }

        // GET: Account
        public ActionResult Login() => View(new LoginViewModel());

        [HttpPost]
        public async Task<ActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return RedirectToAction("Index");
            var user = await _userManager.FindByEmailAsync(model.EmailAddress);

            if (user != null)
            {
                var loginCheck = await _userManager.CheckPasswordAsync(user, model.Password);

                if (loginCheck)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Stock");
                    }
                }

                TempData["Error"] = "Te dhenat jane gabim. Provoni perseri!";
                model.Password = null;
                return RedirectToAction("Login", model);
            }

            TempData["Error"] = "Te dhenat jane gabim. Provoni perseri!";
            model.Password = null;
            return RedirectToAction("Login", model);
        }

        // GET: Account
        public ActionResult Create()
        {
            ViewBag.Warehouses = _context.Warehouses.Where(n => n.IsActive).ToList();
            return View(new RegisterViewModel());
        }

        [HttpPost]
        public async Task<ActionResult> Create([Bind("Id,FirstName,LastName,Email,Password,ConfirmPassword,Role")] RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "");
                return View("Create");
            }
            // Check that we don't already have an email account for this user
            if (_context.Users.FirstOrDefault(u => u.UserName == model.Email && u.Email == model.Email) != null)
            {
                TempData["Error"] = "Adresa e emailit eshte perdorur nje here!";
                return View("Create");
            }

            var appUser = new ApplicationUser
            {
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.Email,
                FullName = $"{model.FirstName} {model.LastName}",
            };

            var createUserResponse = await _userManager.CreateAsync(appUser, model.Password);
           
            if (createUserResponse.Succeeded)
            {
                await _userManager.AddToRoleAsync(appUser, model.Role);
                return RedirectToAction("Index");
            }
            else
            {
                //TempData["Error"] = string.Join(", ", createUserResponse.Errors.ToArray());
                return View("Create", model);
            }
        }



        // GET: Account/Edit/{Guid}
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.SingleOrDefaultAsync(m => m.Id == id & m.IsActive);
            if (user == null)
            {
                return NotFound();
            }
            RegisterViewModel rVM = new RegisterViewModel()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email
            };
            return View(rVM);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,FirstName,LastName,Email,Password,ConfirmPassword,Role")] RegisterViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _context.Users.SingleOrDefaultAsync(m => m.Id == model.Id & m.IsActive);
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    user.FullName = $"{model.FirstName} {model.LastName}";
                    user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, model.Password);

                    if (model.Role == "Admin")
                        await _userManager.RemoveFromRoleAsync(user, "Manager");
                    else
                        await _userManager.RemoveFromRoleAsync(user, "Admin");

                    await _userManager.AddToRoleAsync(user, model.Role);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsersExists(model.Id))
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
            return View(model);
        }

        // GET: Account/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var user = await _context
                .Users
                .Include(n => n.Warehouses)
                .SingleOrDefaultAsync(m => m.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            RegisterViewModel rVM = new RegisterViewModel()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Warehouses = user.Warehouses.ToList()
            };
            return View(rVM);
        }

        // POST: Account/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _context.Users.SingleOrDefaultAsync(m => m.Id == id);
            //_context.Users.Remove(user);
            user.IsActive = false;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> LogOff()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Stock");
        }



        [HttpGet]
        public string GetUserName()
        {
            var user = _userManager.Users.FirstOrDefault(a => a.Id == User.Identity.Name & a.IsActive);
            return $"{user.FirstName} {user.LastName}";
        }

        private bool UsersExists(string id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}