using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Models.Interface;

namespace Models.Helper
{
    public class LanguageResolver : ILanguageResolver
    {
        public const string LanguageCookieName = "tour-language";

        public string Resolve(HttpContext http)
        {
            // 1) امکان override با ?lang=de
            if (http.Request.Query.TryGetValue("lang", out var qs) && !string.IsNullOrWhiteSpace(qs))
            {
                var normalized = LanguageCatalog.Normalize(qs!);
                WriteCookie(http.Response.Cookies, normalized);
                return normalized;
            }

            // 2) کوکی ذخیره شده
            if (http.Request.Cookies.TryGetValue(LanguageCookieName, out var cookieLang)
                && !string.IsNullOrWhiteSpace(cookieLang))
            {
                return LanguageCatalog.Normalize(cookieLang);
            }

            // 3) از Accept-Language
            var al = http.Request.Headers["Accept-Language"].ToString(); // e.g. "de-DE,de;q=0.9,en;q=0.8"
            if (!string.IsNullOrWhiteSpace(al))
            {
                var primary = al.Split(',').FirstOrDefault()?.Split(';')[0] ?? "";
                var code = LanguageCatalog.Normalize(primary);

                if (!string.IsNullOrEmpty(code))
                {
                    WriteCookie(http.Response.Cookies, code);
                    return code;
                }
            }

            // 4) fallback
            return "en";
        }

        private static void WriteCookie(IResponseCookies cookies, string language)
        {
            var options = new CookieOptions
            {
                IsEssential = true,
                SameSite = SameSiteMode.Lax,
                Expires = DateTimeOffset.UtcNow.AddYears(1)
            };

            cookies.Append(LanguageCookieName, language, options);
        }
    }
}
