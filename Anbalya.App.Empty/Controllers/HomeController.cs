using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly IReservationRepository _reservationRepository;
        private readonly IPaymentOptionRepository _paymentOptionRepository;
        public HomeController(
            ITourRepository tourRepository,
            ILanguageResolver langResolver,
            ILandingContentRepository landingContentRepository,
            IReservationRepository reservationRepository,
            IPaymentOptionRepository paymentOptionRepository)
        {
            _tourRepository = tourRepository;
            _langResolver = langResolver;
            _landingContentRepository = landingContentRepository;
            _reservationRepository = reservationRepository;
            _paymentOptionRepository = paymentOptionRepository;
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

            if (!string.IsNullOrWhiteSpace(slug) && !string.Equals(slug, tour.Slug, StringComparison.OrdinalIgnoreCase))
            {
                return RedirectToActionPermanent(nameof(Tour), new { id, slug = tour.Slug });
            }

            var paymentOptions = await _paymentOptionRepository.ListAsync(ct);
            var enabledOptions = paymentOptions
                .Where(o => o.IsEnabled)
                .Select(PaymentOptionDto.FromEntity)
                .ToList();

            if (enabledOptions.Count == 0)
            {
                enabledOptions = paymentOptions.Select(PaymentOptionDto.FromEntity).ToList();
            }

            var defaultMethod = enabledOptions.FirstOrDefault()?.Method ?? PaymentMethod.PayPal;

            var viewModel = new TourBookingPageViewModel
            {
                Tour = tour,
                Form = new TourReservationInputModel
                {
                    TourId = tour.Id,
                    Language = lang,
                    Adults = 1,
                    Children = 0,
                    Infants = 0,
                    PickupLocation = string.Empty,
                    PaymentMethod = defaultMethod,
                    HotelName = "Select Your Hotel"
                },
                PaymentOptions = enabledOptions,
                AccommodationOptions = AccommodationCatalog.List()
            };

            return View("Tour", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BookTour(TourReservationInputModel form, CancellationToken ct)
        {
            var lang = _langResolver.Resolve(HttpContext);
            var tour = await _tourRepository.GetByIdAsync(form.TourId, lang, ct);
            if (tour is null)
            {
                return NotFound();
            }

            ValidateReservationForm(form);

            var paymentOptions = await _paymentOptionRepository.ListAsync(ct);
            var enabledOptions = paymentOptions
                .Where(o => o.IsEnabled)
                .Select(PaymentOptionDto.FromEntity)
                .ToList();

            if (enabledOptions.All(o => o.Method != form.PaymentMethod))
            {
                ModelState.AddModelError(nameof(form.PaymentMethod), "Selected payment method is not available.");
            }

            if (!ModelState.IsValid)
            {
                var errorViewModel = new TourBookingPageViewModel
                {
                    Tour = tour,
                    Form = form,
                    PaymentOptions = enabledOptions.Count > 0
                        ? enabledOptions
                        : paymentOptions.Select(PaymentOptionDto.FromEntity).ToList(),
                    ErrorMessage = "Please correct the highlighted fields and try again.",
                    AccommodationOptions = AccommodationCatalog.List()
                };

                return View("Tour", errorViewModel);
            }

            var totalPrice = CalculateTotalPrice(tour, form);
            var firstName = form.FirstName?.Trim() ?? string.Empty;
            var lastName = form.LastName?.Trim() ?? string.Empty;
            var fullName = string.Join(" ", new[] { firstName, lastName }
                .Where(part => !string.IsNullOrWhiteSpace(part)));
            var hotelName = form.HotelName;
            if (string.Equals(hotelName, "Select Your Hotel", StringComparison.OrdinalIgnoreCase))
            {
                hotelName = null;
            }

            var pickupLocation = string.IsNullOrWhiteSpace(form.PickupLocation)
                ? hotelName ?? string.Empty
                : form.PickupLocation.Trim();

            var reservation = new Reservation
            {
                TourId = tour.Id,
                CustomerFirstName = firstName,
                CustomerLastName = lastName,
                CustomerName = string.IsNullOrWhiteSpace(fullName)
                    ? firstName
                    : fullName,
                CustomerEmail = form.CustomerEmail.Trim(),
                CustomerPhone = string.IsNullOrWhiteSpace(form.CustomerPhone) ? null : form.CustomerPhone.Trim(),
                PreferredDate = form.PreferredDate,
                Adults = form.Adults,
                Children = form.Children,
                Infants = form.Infants,
                PickupLocation = pickupLocation ?? string.Empty,
                Notes = string.IsNullOrWhiteSpace(form.Notes) ? null : form.Notes.Trim(),
                PaymentMethod = form.PaymentMethod,
                PaymentStatus = PaymentStatus.Pending,
                Status = ReservationStatus.Pending,
                PaymentReference = string.IsNullOrWhiteSpace(form.PaymentReference) ? null : form.PaymentReference.Trim(),
                TotalPrice = totalPrice,
                Language = lang,
                HotelName = hotelName,
                RoomNumber = string.IsNullOrWhiteSpace(form.RoomNumber) ? null : form.RoomNumber.Trim()
            };

            var reservationId = await _reservationRepository.CreateAsync(reservation, ct);
            var storedReservation = await _reservationRepository.GetByIdAsync(reservationId, ct);

            if (storedReservation is null)
            {
                reservation.Tour = new Tour { Id = tour.Id, TourName = tour.TourName };
                storedReservation = reservation;
            }

            var reservationDto = ReservationDetailsDto.FromEntity(storedReservation);
            var paymentOption = paymentOptions.FirstOrDefault(o => o.Method == form.PaymentMethod);

            var confirmationViewModel = new ReservationConfirmationViewModel
            {
                Tour = tour,
                Reservation = reservationDto,
                PaymentOption = paymentOption is null ? null : PaymentOptionDto.FromEntity(paymentOption)
            };

            return View("ReservationConfirmation", confirmationViewModel);
        }

        private void ValidateReservationForm(TourReservationInputModel form)
        {
            if (form.PreferredDate.HasValue && form.PreferredDate.Value.Date < DateTime.UtcNow.Date)
            {
                ModelState.AddModelError(nameof(form.PreferredDate), "Please choose a future date.");
            }

            var totalGuests = Math.Max(0, form.Adults) + Math.Max(0, form.Children) + Math.Max(0, form.Infants);
            if (totalGuests <= 0)
            {
                ModelState.AddModelError(nameof(form.Adults), "At least one guest is required.");
            }

            var hotelValue = form.HotelName?.Trim();
            var hasHotelSelection = !string.IsNullOrWhiteSpace(hotelValue) &&
                                     !string.Equals(hotelValue, "Select Your Hotel", StringComparison.OrdinalIgnoreCase);

            if (!hasHotelSelection && string.IsNullOrWhiteSpace(form.PickupLocation))
            {
                ModelState.AddModelError(nameof(form.HotelName), "Please select your hotel or enter a pickup location.");
            }
        }

        private static int CalculateTotalPrice(TourDto tour, TourReservationInputModel form)
        {
            var adultTotal = Math.Max(0, form.Adults) * tour.Price;
            var childTotal = Math.Max(0, form.Children) * tour.KinderPrice;
            var infantTotal = Math.Max(0, form.Infants) * tour.InfantPrice;
            return adultTotal + childTotal + infantTotal;
        }
    }
}
