using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Helper;

namespace Controllers
{
    public class LocalizationController : Controller
    {
        [HttpGet]
        public IActionResult SetLanguage(string culture, string? returnUrl = null)
        {
            var normalized = LanguageCatalog.Normalize(culture);
            var options = new CookieOptions
            {
                IsEssential = true,
                SameSite = SameSiteMode.Lax,
                Expires = DateTimeOffset.UtcNow.AddYears(1)
            };

            Response.Cookies.Append(LanguageResolver.LanguageCookieName, normalized, options);

            if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
