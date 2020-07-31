using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeeManager.Bsl.Model;
using EmployeeManager.Bsl.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManager.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        public EmployeeController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpGet]
        [Authorize]
        public IActionResult Welcome()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            model.Email = model.Email.Trim();
            var appUser = await _userManager.FindByEmailAsync(model.Email).ConfigureAwait(false);
            if (appUser == null)
            {
                ModelState.AddModelError("FileNameValidation", "Account does not exist.");
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, true, lockoutOnFailure: false).ConfigureAwait(false);
            if (result.Succeeded)
            {
                return Redirect("/Employee/Welcome");
            }

            ModelState.AddModelError("FileNameValidation", "Password does not match.");
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync().ConfigureAwait(false);
            return Redirect("/");
        }
    }
}