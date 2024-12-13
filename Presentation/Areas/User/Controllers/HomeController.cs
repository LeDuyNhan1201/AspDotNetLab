using Application;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System;
using System.Linq;

namespace Presentation.Areas.User.Controllers
{
    [Area("User")]
    public class HomeController(
        IStringLocalizer<SharedResource> localizer, 
        IOptions<RequestLocalizationOptions> localizationOptions) : Controller
    {
        [HttpGet]
        [Authorize(Roles = "User")]
        public IActionResult Index()
        {
            ViewData["Message"] = localizer["Hello", "User"];
            return View();
        }

        [HttpGet]
        public IActionResult ChangeLanguage(string lang)
        {
            // Kiểm tra ngôn ngữ hợp lệ, nếu không hợp lệ thì chọn mặc định "en-US"
            var culture = string.IsNullOrEmpty(lang) || !localizationOptions.Value.SupportedCultures.Any(c => c.Name == lang)
                ? "en-US"
                : lang;

            // Cập nhật culture vào cookie
            Response.Cookies.Append(
                ".AspNetCore.Culture",
                $"c={culture}|uic={culture}",
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            // Trả về trang trước đó, nếu có
            var refererUrl = Request.Headers["Referer"].ToString();
            if (!string.IsNullOrEmpty(refererUrl))
            {
                return Redirect(refererUrl);
            }

            // Nếu không có URL trước đó, trả về trang mặc định
            return RedirectToAction("Index", "Home", new { area = "User" });
        }

    }
}
