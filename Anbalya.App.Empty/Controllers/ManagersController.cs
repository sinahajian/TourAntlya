using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Helper;
using Models.Interface;
using Models.Entities;

namespace Controllers
{
    public class ManagersController : Controller
    {
        private readonly IManagerRepository _managerRepository;
        private readonly IManagerTourRepository _managerTourRepository;
        private readonly ILandingContentRepository _landingContentRepository;
        private readonly IWebHostEnvironment _environment;
        private readonly IReservationRepository _reservationRepository;
        private readonly IPaymentOptionRepository _paymentOptionRepository;

        public ManagersController(
            IManagerRepository managerRepository,
            IManagerTourRepository managerTourRepository,
            ILandingContentRepository landingContentRepository,
            IWebHostEnvironment environment,
            IReservationRepository reservationRepository,
            IPaymentOptionRepository paymentOptionRepository)
        {
            _managerRepository = managerRepository;
            _managerTourRepository = managerTourRepository;
            _landingContentRepository = landingContentRepository;
            _environment = environment;
            _reservationRepository = reservationRepository;
            _paymentOptionRepository = paymentOptionRepository;
        }

        [HttpGet]
        public IActionResult LoginManagers()
        {
            var model = new ManagerLoginViewModel();

            if (TempData.TryGetValue("ManagerLoginError", out var error) && error is string errorMessage)
            {
                model.ErrorMessage = errorMessage;
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginManagers(ManagerLoginViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.ErrorMessage = "Please correct the highlighted errors.";
                return View(viewModel);
            }

            var manager = await _managerRepository.AuthenticateAsync(
                viewModel.Input.UserName,
                viewModel.Input.Password);

            if (manager is null)
            {
                viewModel.ErrorMessage = "Invalid username or password.";
                viewModel.Input.Password = string.Empty;
                ModelState.Remove("Input.Password");
                return View(viewModel);
            }

            return RedirectToAction(nameof(Index), new { userName = manager.UserName });
        }

        [HttpGet]
        public async Task<IActionResult> Index(string userName, CancellationToken ct)
        {
            var (manager, redirect) = await ResolveManagerAsync(userName, ct);
            if (redirect is not null) return redirect;

            var dashboardModel = ManagerDashboardViewModel.Create(manager!);
            ViewData["Manager"] = manager;

            return View(dashboardModel);
        }

        [HttpGet]
        public async Task<IActionResult> Tours(string userName, CancellationToken ct)
        {
            var (manager, redirect) = await ResolveManagerAsync(userName, ct);
            if (redirect is not null) return redirect;

            var tours = await _managerTourRepository.ListAsync(ct);
            string? feedback = TempData.TryGetValue("ManagerToursFeedback", out var message)
                ? message as string
                : null;

            var viewModel = new ManagerTourListViewModel(manager!, tours)
            {
                FeedbackMessage = feedback
            };

            ViewData["Manager"] = manager;
            ViewData["Title"] = "Manage Tours";

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteTour(int tourId, string userName, CancellationToken ct)
        {
            var (manager, redirect) = await ResolveManagerAsync(userName, ct);
            if (redirect is not null) return redirect;

            await _managerTourRepository.DeleteAsync(tourId, ct);
            TempData["ManagerToursFeedback"] = "Tour deleted successfully.";

            return RedirectToAction(nameof(Tours), new { userName = manager!.UserName });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetPrimaryPhoto(int tourId, string photoAddress, string userName, CancellationToken ct)
        {
            var (manager, redirect) = await ResolveManagerAsync(userName, ct);
            if (redirect is not null) return redirect;

            await _managerTourRepository.SetPrimaryPhotoAsync(tourId, photoAddress, ct);
            TempData["ManagerToursFeedback"] = "Primary photo updated.";

            return RedirectToAction(nameof(Tours), new { userName = manager!.UserName });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadTourPhotos(int tourId, string userName, List<IFormFile> photos, CancellationToken ct)
        {
            var (manager, redirect) = await ResolveManagerAsync(userName, ct);
            if (redirect is not null) return redirect;

            var savedPhotos = await SavePhotosAsync(photos);

            if (savedPhotos.Count > 0)
            {
                await _managerTourRepository.AddPhotosAsync(tourId, savedPhotos, ct);
                TempData["ManagerToursFeedback"] = "Photos uploaded successfully.";
            }
            else
            {
                TempData["ManagerToursFeedback"] = "No valid photos were uploaded.";
            }

            return RedirectToAction(nameof(Tours), new { userName = manager!.UserName });
        }

        [HttpGet]
        public async Task<IActionResult> AddTour(string userName, CancellationToken ct)
        {
            var (manager, redirect) = await ResolveManagerAsync(userName, ct);
            if (redirect is not null) return redirect;

            var model = new ManagerTourCreateViewModel
            {
                UserName = manager!.UserName
            };

            ViewData["Manager"] = manager;
            ViewData["Title"] = "Add Tour";

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> LandingContent(string userName, string? lang, CancellationToken ct)
        {
            var (manager, redirect) = await ResolveManagerAsync(userName, ct);
            if (redirect is not null) return redirect;

            var dtos = await _landingContentRepository.ListAllAsync(ct);
            var model = LandingContentEditViewModel.FromDtos(dtos, manager!.UserName, lang);

            if (TempData.TryGetValue("ManagerLandingContentFeedback", out var feedback) && feedback is string message)
            {
                ViewData["Feedback"] = message;
            }

            ViewData["Manager"] = manager;
            ViewData["Title"] = "Landing Content";

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LandingContent(LandingContentEditViewModel model, CancellationToken ct)
        {
            var (manager, redirect) = await ResolveManagerAsync(model.UserName, ct);
            if (redirect is not null) return redirect;

            model.ActiveLanguage = LanguageCatalog.Normalize(model.ActiveLanguage);
            model.EnsureLanguageMetadata();

            if (model.BackgroundImageFile is not null && model.BackgroundImageFile.Length > 0)
            {
                if (model.BackgroundImageFile.Length > 5 * 1024 * 1024)
                {
                    ModelState.AddModelError("BackgroundImageFile", "Image must be 5 MB or smaller.");
                }

                var extension = Path.GetExtension(model.BackgroundImageFile.FileName)?.ToLowerInvariant() ?? string.Empty;
                if (!IsSupportedImageExtension(extension))
                {
                    ModelState.AddModelError("BackgroundImageFile", "Only JPG, JPEG, PNG, WEBP, or GIF files are allowed.");
                }
            }

            if (!ModelState.IsValid)
            {
                ViewData["Manager"] = manager;
                ViewData["Title"] = "Landing Content";
                return View(model);
            }

            if (model.BackgroundImageFile is not null && model.BackgroundImageFile.Length > 0)
            {
                var savedPath = await SaveHeroImageAsync(model.BackgroundImageFile, model.BackgroundImage);
                model.BackgroundImage = savedPath;
            }

            var dtos = model.ToDtos();
            await _landingContentRepository.UpdateAllAsync(dtos, ct);

            TempData["ManagerLandingContentFeedback"] = "Landing page content updated.";
            return RedirectToAction(nameof(LandingContent), new { userName = manager!.UserName, lang = model.ActiveLanguage });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddTour(ManagerTourCreateViewModel model, CancellationToken ct)
        {
            var (manager, redirect) = await ResolveManagerAsync(model.UserName, ct);
            if (redirect is not null) return redirect;

            ValidateTourModel(model);
            model.SyncServices();
            model.SyncSelectedDays();

            if (!ModelState.IsValid)
            {
                ViewData["Manager"] = manager;
                ViewData["Title"] = "Add Tour";
                return View(model);
            }

            var savedPhotos = await SavePhotosAsync(model.Photos);
            var dto = model.ToDto();

            await _managerTourRepository.AddTourAsync(dto, savedPhotos, ct);

            TempData["ManagerToursFeedback"] = "Tour created successfully.";
            return RedirectToAction(nameof(Tours), new { userName = manager!.UserName });
        }

        [HttpGet]
        public async Task<IActionResult> EditTour(int tourId, string userName, CancellationToken ct)
        {
            var (manager, redirect) = await ResolveManagerAsync(userName, ct);
            if (redirect is not null) return redirect;

            var editDto = await _managerTourRepository.GetForEditAsync(tourId, ct);
            if (editDto is null)
            {
                TempData["ManagerToursFeedback"] = "Tour not found.";
                return RedirectToAction(nameof(Tours), new { userName = manager!.UserName });
            }

            var viewModel = new ManagerTourEditViewModel(editDto, manager!.UserName);
            ViewData["Manager"] = manager;
            ViewData["Title"] = $"Edit {editDto.TourName}";

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditTour(ManagerTourEditViewModel model, CancellationToken ct)
        {
            var (manager, redirect) = await ResolveManagerAsync(model.UserName, ct);
            if (redirect is not null) return redirect;

            model.SyncServices();
            model.SyncSelectedDays();
            model.ApplyActiveDay();

            if (!ModelState.IsValid)
            {
                ViewData["Manager"] = manager;
                ViewData["Title"] = $"Edit {model.Tour?.TourName}";
                return View(model);
            }
            await _managerTourRepository.UpdateAsync(model.Tour, ct);

            var newPhotos = await SavePhotosAsync(model.NewPhotos);
            if (newPhotos.Count > 0)
            {
                await _managerTourRepository.AddPhotosAsync(model.Tour.Id, newPhotos, ct);
                TempData["ManagerToursFeedback"] = "Tour updated successfully. New photos uploaded.";
            }
            else
            {
                TempData["ManagerToursFeedback"] = "Tour updated successfully.";
            }

            return RedirectToAction(nameof(Tours), new { userName = manager!.UserName });
        }

        [HttpGet]
        public async Task<IActionResult> PaymentOptions(string userName, CancellationToken ct)
        {
            var (manager, redirect) = await ResolveManagerAsync(userName, ct);
            if (redirect is not null) return redirect;
            if (manager is null)
            {
                TempData["ManagerLoginError"] = "Please login to access the dashboard.";
                return RedirectToAction(nameof(LoginManagers));
            }

            var options = await _paymentOptionRepository.ListAsync(ct);
            var viewModel = PaymentOptionsViewModel.FromOptions(manager.UserName, options);

            if (TempData.TryGetValue("ManagerPaymentFeedback", out var feedback) && feedback is string feedbackMessage)
            {
                viewModel.FeedbackMessage = feedbackMessage;
            }

            ViewData["Manager"] = manager;
            ViewData["Title"] = "Payment Settings";

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PaymentOptions(PaymentOptionsViewModel model, CancellationToken ct)
        {
            var (manager, redirect) = await ResolveManagerAsync(model.UserName, ct);
            if (redirect is not null) return redirect;
            if (manager is null)
            {
                TempData["ManagerLoginError"] = "Please login to access the dashboard.";
                return RedirectToAction(nameof(LoginManagers));
            }

            if (model.Options is null || model.Options.Count == 0)
            {
                ModelState.AddModelError(string.Empty, "At least one payment method must be provided.");
            }

            if (!ModelState.IsValid)
            {
                ViewData["Manager"] = manager;
                ViewData["Title"] = "Payment Settings";
                return View(model);
            }

            var optionsToPersist = model.Options ?? new List<PaymentOptionInputModel>();

            foreach (var option in optionsToPersist)
            {
                var entity = new PaymentOption
                {
                    Method = option.Method,
                    DisplayName = option.DisplayName.Trim(),
                    AccountIdentifier = option.AccountIdentifier.Trim(),
                    Instructions = option.Instructions.Trim(),
                    IsEnabled = option.IsEnabled
                };

                await _paymentOptionRepository.UpdateAsync(entity, ct);
            }

            TempData["ManagerPaymentFeedback"] = "Payment methods updated.";
            return RedirectToAction(nameof(PaymentOptions), new { userName = manager.UserName });
        }

        [HttpGet]
        public async Task<IActionResult> Reservations(string userName, int? tourId, CancellationToken ct)
        {
            var (manager, redirect) = await ResolveManagerAsync(userName, ct);
            if (redirect is not null) return redirect;
            if (manager is null)
            {
                TempData["ManagerLoginError"] = "Please login to access the dashboard.";
                return RedirectToAction(nameof(LoginManagers));
            }

            var reservations = await _reservationRepository.ListAsync(tourId, ct);
            var reservationDtos = reservations
                .Select(ReservationDetailsDto.FromEntity)
                .ToList();

            var tours = await _managerTourRepository.ListAsync(ct);
            var viewModel = new ManagerReservationListViewModel(manager, reservationDtos, tours.ToList(), tourId);

            if (TempData.TryGetValue("ManagerReservationsFeedback", out var feedback) && feedback is string message)
            {
                viewModel.FeedbackMessage = message;
            }

            ViewData["Manager"] = manager;
            ViewData["Title"] = "Reservations";

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateReservationStatus(string userName, int reservationId, ReservationStatus status, PaymentStatus paymentStatus, string? paymentReference, int? tourId, CancellationToken ct)
        {
            var (manager, redirect) = await ResolveManagerAsync(userName, ct);
            if (redirect is not null) return redirect;
            if (manager is null)
            {
                TempData["ManagerLoginError"] = "Please login to access the dashboard.";
                return RedirectToAction(nameof(LoginManagers));
            }

            await _reservationRepository.UpdateStatusAsync(reservationId, status, paymentStatus, string.IsNullOrWhiteSpace(paymentReference) ? null : paymentReference.Trim(), ct);
            TempData["ManagerReservationsFeedback"] = "Reservation updated.";

            return RedirectToAction(nameof(Reservations), new { userName = manager.UserName, tourId });
        }

        private async Task<(ManagerDto? manager, IActionResult? redirect)> ResolveManagerAsync(string userName, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                TempData["ManagerLoginError"] = "Please login to access the dashboard.";
                return (null, RedirectToAction(nameof(LoginManagers)));
            }

            var manager = await _managerRepository.GetByUserNameAsync(userName, ct);

            if (manager is null)
            {
                TempData["ManagerLoginError"] = "Manager not found. Please login again.";
                return (null, RedirectToAction(nameof(LoginManagers)));
            }

            return (manager, null);
        }

        private async Task<List<string>> SavePhotosAsync(IEnumerable<IFormFile>? files)
        {
            var saved = new List<string>();
            if (files is null)
            {
                return saved;
            }

            var webRoot = _environment.WebRootPath;
            if (string.IsNullOrWhiteSpace(webRoot))
            {
                webRoot = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            }

            var targetFolder = Path.Combine(webRoot, "tourimage");
            Directory.CreateDirectory(targetFolder);

            foreach (var file in files)
            {
                if (file is null || file.Length == 0)
                {
                    continue;
                }

                if (file.Length > 5 * 1024 * 1024)
                {
                    continue;
                }

                var extension = Path.GetExtension(file.FileName)?.ToLowerInvariant() ?? string.Empty;
                if (!IsSupportedImageExtension(extension))
                {
                    continue;
                }

                var fileName = $"{Guid.NewGuid():N}{extension}";
                var physicalPath = Path.Combine(targetFolder, fileName);

                await using (var stream = System.IO.File.Create(physicalPath))
                {
                    await file.CopyToAsync(stream);
                }

                var relativePath = $"/tourimage/{fileName}";
                saved.Add(relativePath);
            }

            return saved;
        }

        private async Task<string?> SaveHeroImageAsync(IFormFile file, string? currentPath)
        {
            var webRoot = _environment.WebRootPath;
            if (string.IsNullOrWhiteSpace(webRoot))
            {
                webRoot = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            }

            var targetFolder = Path.Combine(webRoot, "hero");
            Directory.CreateDirectory(targetFolder);

            var extension = Path.GetExtension(file.FileName)?.ToLowerInvariant() ?? string.Empty;
            var fileName = $"hero-{DateTimeOffset.UtcNow:yyyyMMddHHmmssfff}-{Guid.NewGuid():N}{extension}";
            var physicalPath = Path.Combine(targetFolder, fileName);

            await using (var stream = System.IO.File.Create(physicalPath))
            {
                await file.CopyToAsync(stream);
            }

            if (!string.IsNullOrWhiteSpace(currentPath) && currentPath.StartsWith("/hero/", StringComparison.OrdinalIgnoreCase))
            {
                var trimmed = currentPath.TrimStart('/')
                    .Replace('/', Path.DirectorySeparatorChar);
                var existingPath = Path.Combine(webRoot, trimmed);
                if (System.IO.File.Exists(existingPath) && !string.Equals(existingPath, physicalPath, StringComparison.OrdinalIgnoreCase))
                {
                    System.IO.File.Delete(existingPath);
                }
            }

            return $"/hero/{fileName}";
        }

        private static bool IsSupportedImageExtension(string extension)
        {
            return extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".webp" || extension == ".gif";
        }

        private void ValidateTourModel(ManagerTourCreateViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.TourName))
            {
                ModelState.AddModelError(nameof(model.TourName), "Tour name is required.");
            }

            if (model.Price < 0)
            {
                ModelState.AddModelError(nameof(model.Price), "Adult price must be zero or positive.");
            }

            if (model.KinderPrice < 0)
            {
                ModelState.AddModelError(nameof(model.KinderPrice), "Child price must be zero or positive.");
            }

            if (model.InfantPrice < 0)
            {
                ModelState.AddModelError(nameof(model.InfantPrice), "Infant price must be zero or positive.");
            }

            if (model.DurationHours < 0)
            {
                ModelState.AddModelError(nameof(model.DurationHours), "Duration must be zero or positive.");
            }
        }
    }
}
