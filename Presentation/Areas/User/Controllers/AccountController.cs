using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Presentation.Areas.User.Controllers
{
    [Area("User")]
    public class AccountController(SignInManager<Domain.Entities.User> signInManager, UserManager<Domain.Entities.User> userManager) : Controller
    {

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login([Required] string username, [Required] string password)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Username and password are required.");
                return View();
            }

            var user = await userManager.FindByNameAsync(username);
            if (user == null)
            {
                ModelState.AddModelError("", "Invalid login attempt.");
                return View();
            }

            var result = await signInManager.PasswordSignInAsync(user, password, true, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                var roles = await userManager.GetRolesAsync(user);
                if (roles.Contains("Admin"))
                    return RedirectToAction("Index", "Home", new { area = "Admin" });

                return RedirectToAction("Index", "Home", new { area = "User" });
            }

            // Thêm thông báo lỗi cho trường hợp mật khẩu sai
            ModelState.AddModelError("", "Invalid username or password.");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            Console.WriteLine("Logout");
            await signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account", new { area = "User" });
        }
    }
}
