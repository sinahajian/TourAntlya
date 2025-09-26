
using Microsoft.AspNetCore.Mvc;
using Models.Interface;
namespace Controllers
{
    public class HomeController : Controller
    {

        private readonly ITourRepository _tourRepository;
        private readonly ILanguageResolver _langResolver;
        public HomeController(ITourRepository tourRepository, ILanguageResolver langResolver)
        {
            _tourRepository = tourRepository;
            _langResolver = langResolver;
        }

        public async Task<IActionResult> Index()
        {

            var lang = _langResolver.Resolve(HttpContext);
            var data = await _tourRepository.ListAsync(lang);
            return View(data);
        }
    }
}
