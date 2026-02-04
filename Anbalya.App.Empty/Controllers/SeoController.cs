using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Models.Entities;
using Models.Interface;

namespace Controllers
{
    public class SeoController : Controller
    {
        private readonly ITourRepository _tourRepository;
        private readonly IConfiguration _configuration;

        public SeoController(ITourRepository tourRepository, IConfiguration configuration)
        {
            _tourRepository = tourRepository;
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult Robots()
        {
            var baseUrl = ResolveBaseUrl();
            var sitemapUrl = $"{baseUrl}/sitemap.xml";
            var payload = $"User-agent: *\nAllow: /\nSitemap: {sitemapUrl}\n";
            return Content(payload, "text/plain");
        }

        [HttpGet]
        public async Task<IActionResult> Sitemap(CancellationToken ct)
        {
            var baseUrl = ResolveBaseUrl();
            var urls = new List<string>
            {
                Url.Action("Index", "Home") ?? "/",
                Url.Action("About", "Home") ?? "/Home/About",
                Url.Action("Contact", "Home") ?? "/Home/Contact"
            };

            foreach (Category category in Enum.GetValues(typeof(Category)))
            {
                var path = Url.Action("Category", "Home", new { category = category.ToString() });
                if (!string.IsNullOrWhiteSpace(path))
                {
                    urls.Add(path);
                }
            }

            var tours = await _tourRepository.ListAsync("en", ct);
            foreach (var tour in tours)
            {
                var path = Url.Action("Tour", "Home", new { id = tour.Id, slug = tour.Slug });
                if (!string.IsNullOrWhiteSpace(path))
                {
                    urls.Add(path);
                }
            }

            var ns = XNamespace.Get("http://www.sitemaps.org/schemas/sitemap/0.9");
            var urlset = new XElement(ns + "urlset",
                urls
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .Select(path => new XElement(ns + "url",
                        new XElement(ns + "loc", $"{baseUrl}{EnsureLeadingSlash(path)}"))));

            var doc = new XDocument(urlset);
            return Content(doc.ToString(SaveOptions.DisableFormatting), "application/xml");
        }

        private string ResolveBaseUrl()
        {
            var configured = _configuration["Site:BaseUrl"];
            if (!string.IsNullOrWhiteSpace(configured))
            {
                return configured.TrimEnd('/');
            }

            return $"{Request.Scheme}://{Request.Host}".TrimEnd('/');
        }

        private static string EnsureLeadingSlash(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return "/";
            }

            return path.StartsWith("/", StringComparison.Ordinal) ? path : "/" + path;
        }
    }
}
