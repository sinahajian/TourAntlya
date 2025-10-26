
using System.Threading;
using Microsoft.AspNetCore.Mvc;
using Models.Interface;
using Models.Entities;
using Models.Helper;
namespace Controllers
{
    public class HomeController : Controller
    {

        private readonly ITourRepository _tourRepository;
        private readonly ILanguageResolver _langResolver;
        private readonly ILandingContentRepository _landingContentRepository;
        public HomeController(
            ITourRepository tourRepository,
            ILanguageResolver langResolver,
            ILandingContentRepository landingContentRepository)
        {
            _tourRepository = tourRepository;
            _langResolver = langResolver;
            _landingContentRepository = landingContentRepository;
        }

        public async Task<IActionResult> Index(CancellationToken ct)
        {

            var lang = _langResolver.Resolve(HttpContext);
            var landing = await _landingContentRepository.GetAsync(lang, ct);
            var tours = await _tourRepository.ListAsync(lang, ct);

            var model = new HomePageViewModel
            {
                Hero = landing,
                Tours = tours,
                Language = lang
            };

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Category(string category, CancellationToken ct)
        {
            if (!Enum.TryParse<Category>(category, true, out var parsedCategory))
            {
                return NotFound();
            }

            var lang = _langResolver.Resolve(HttpContext);
            var tours = await _tourRepository.ListByCategoryAsync(parsedCategory, lang, ct);
            var viewModel = new CategoryPageViewModel(parsedCategory, parsedCategory.GetDisplayName(lang), tours);
            return View("Category", viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Tour(int id, string? slug, CancellationToken ct)
        {
            var lang = _langResolver.Resolve(HttpContext);
            var tour = await _tourRepository.GetByIdAsync(id, lang, ct);
            if (tour is null)
            {
                return NotFound();
            }

            return View("Tour", tour);
        }
    }
}
