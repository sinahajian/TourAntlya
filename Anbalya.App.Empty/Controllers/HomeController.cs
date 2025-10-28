using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
        private readonly IPayPalSettingsRepository _payPalSettingsRepository;
        private readonly IRoyalFacilityRepository _facilityRepository;
        private readonly IAboutContentRepository _aboutRepository;
        private readonly IContactMessageRepository _contactMessageRepository;
        private readonly IEmailConfigurationRepository _emailConfigRepository;
        private readonly IEmailSender _emailSender;
        public HomeController(
            ITourRepository tourRepository,
            ILanguageResolver langResolver,
            ILandingContentRepository landingContentRepository,
            IReservationRepository reservationRepository,
            IPaymentOptionRepository paymentOptionRepository,
            IPayPalSettingsRepository payPalSettingsRepository,
            IRoyalFacilityRepository facilityRepository,
            IAboutContentRepository aboutRepository,
            IContactMessageRepository contactMessageRepository,
            IEmailConfigurationRepository emailConfigRepository,
            IEmailSender emailSender)
        {
            _tourRepository = tourRepository;
            _langResolver = langResolver;
            _landingContentRepository = landingContentRepository;
            _reservationRepository = reservationRepository;
            _paymentOptionRepository = paymentOptionRepository;
            _payPalSettingsRepository = payPalSettingsRepository;
            _facilityRepository = facilityRepository;
            _aboutRepository = aboutRepository;
            _contactMessageRepository = contactMessageRepository;
            _emailConfigRepository = emailConfigRepository;
            _emailSender = emailSender;
        }

        public async Task<IActionResult> Index(CancellationToken ct)
        {

            var lang = _langResolver.Resolve(HttpContext);
            var landing = await _landingContentRepository.GetAsync(lang, ct);
            var tours = await _tourRepository.ListAsync(lang, ct);
            var facilities = await _facilityRepository.ListAsync(lang, ct);
            var about = await _aboutRepository.GetAsync(lang, ct);

            var model = new HomePageViewModel
            {
                Hero = landing,
                Tours = tours,
                Language = lang,
                Facilities = facilities,
                About = about
            };

            ViewData["Title"] = "Antalya Tours & Excursions";
            return View(model);
        }

        [HttpGet]
        public IActionResult Contact()
        {
            var lang = _langResolver.Resolve(HttpContext);
            var model = new ContactRequestInputModel { Language = lang };
            ViewData["Title"] = "Contact Antalya Tours";
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Contact(ContactRequestInputModel form, CancellationToken ct)
        {
            var lang = _langResolver.Resolve(HttpContext);
            form.Language ??= lang;

            if (!ModelState.IsValid)
            {
                ViewData["Title"] = "Contact Antalya Tours";
                return View(form);
            }

            var normalizedLang = LanguageCatalog.Normalize(form.Language ?? lang);
            var message = new ContactMessage
            {
                FullName = form.FullName.Trim(),
                Email = form.Email.Trim(),
                Message = form.Message.Trim(),
                Language = normalizedLang
            };

            await _contactMessageRepository.CreateAsync(message, ct);
            await SendContactNotificationsAsync(message, normalizedLang, ct);

            TempData["ContactSuccess"] = true;
            return RedirectToAction(nameof(Contact));
        }

        [HttpGet]
        public async Task<IActionResult> About(CancellationToken ct)
        {
            var lang = _langResolver.Resolve(HttpContext);
            var about = await _aboutRepository.GetAsync(lang, ct);
            ViewData["Title"] = "About Antalya Tours";
            return View(about);
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
            ViewData["Title"] = $"{viewModel.DisplayName} Antalya Tours";
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
            var payPalSettings = await _payPalSettingsRepository.GetAsync(ct);
            var payPalInfo = string.IsNullOrWhiteSpace(payPalSettings.BusinessEmail)
                ? PayPalCheckoutInfo.Disabled
                : new PayPalCheckoutInfo(
                    true,
                    payPalSettings.BusinessEmail,
                    string.IsNullOrWhiteSpace(payPalSettings.Currency) ? "EUR" : payPalSettings.Currency,
                    payPalSettings.ReturnUrl ?? string.Empty,
                    payPalSettings.CancelUrl ?? string.Empty,
                    payPalSettings.UseSandbox);
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
                AccommodationOptions = AccommodationCatalog.List(),
                PayPal = payPalInfo
            };

            ViewData["Title"] = $"{tour.TourName} Antalya Tour";
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
            var payPalSettings = await _payPalSettingsRepository.GetAsync(ct);
            var payPalInfo = string.IsNullOrWhiteSpace(payPalSettings.BusinessEmail)
                ? PayPalCheckoutInfo.Disabled
                : new PayPalCheckoutInfo(
                    true,
                    payPalSettings.BusinessEmail,
                    string.IsNullOrWhiteSpace(payPalSettings.Currency) ? "EUR" : payPalSettings.Currency,
                    payPalSettings.ReturnUrl ?? string.Empty,
                    payPalSettings.CancelUrl ?? string.Empty,
                    payPalSettings.UseSandbox);
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
                    AccommodationOptions = AccommodationCatalog.List(),
                    PayPal = payPalInfo
                };

                ViewData["Title"] = $"{tour.TourName} Antalya Tour";
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

            await SendReservationNotificationsAsync(tour, reservationDto, lang, ct);

            ViewData["Title"] = $"{tour.TourName} Antalya Tour Confirmation";
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

        private async Task SendContactNotificationsAsync(ContactMessage message, string language, CancellationToken ct)
        {
            var smtp = await _emailConfigRepository.GetSmtpSettingsAsync(ct);
            var createdAt = message.CreationTime > 0
                ? DateTimeOffset.FromUnixTimeSeconds(message.CreationTime)
                : DateTimeOffset.UtcNow;

            var tokens = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["FullName"] = string.IsNullOrWhiteSpace(message.FullName) ? "Guest" : message.FullName,
                ["Email"] = message.Email,
                ["Message"] = message.Message,
                ["Language"] = string.IsNullOrWhiteSpace(message.Language) ? LanguageCatalog.Normalize(language) : message.Language!,
                ["CreatedAt"] = createdAt.ToLocalTime().ToString("f")
            };

            var userTemplate = await ResolveTemplateAsync(EmailTemplateKeys.ContactUser, language, ct);
            var (userSubject, userBody) = RenderTemplate(
                userTemplate,
                tokens,
                "Thank you for contacting Tour Antalya",
                $"Hello {tokens["FullName"]},\n\nThank you for reaching out to Tour Antalya. Our team will respond shortly.\n\nYour message:\n{tokens["Message"]}\n\nBest regards,\nTour Antalya");

            await _emailSender.SendEmailAsync(message.Email, userSubject, userBody, tokens["FullName"], ct);

            var adminRecipients = ParseRecipients(smtp.NotificationEmail);
            if (adminRecipients.Count > 0)
            {
                var adminTemplate = await ResolveTemplateAsync(EmailTemplateKeys.ContactAdmin, language, ct);
                var (adminSubject, adminBody) = RenderTemplate(
                    adminTemplate,
                    tokens,
                    $"New contact request from {tokens["FullName"]}",
                    $"A new contact request has been submitted.\n\nName: {tokens["FullName"]}\nEmail: {tokens["Email"]}\nLanguage: {tokens["Language"]}\nReceived: {tokens["CreatedAt"]}\n\nMessage:\n{tokens["Message"]}");

                await _emailSender.SendEmailAsync(adminRecipients, adminSubject, adminBody, ct);
            }
        }

        private async Task SendReservationNotificationsAsync(TourDto tour, ReservationDetailsDto reservation, string language, CancellationToken ct)
        {
            var smtp = await _emailConfigRepository.GetSmtpSettingsAsync(ct);
            var tokens = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["FullName"] = string.IsNullOrWhiteSpace(reservation.FullName) ? "Guest" : reservation.FullName,
                ["Email"] = reservation.CustomerEmail,
                ["Phone"] = reservation.CustomerPhone ?? string.Empty,
                ["TourName"] = tour.TourName,
                ["ReservationId"] = reservation.Id.ToString(CultureInfo.InvariantCulture),
                ["PreferredDate"] = reservation.PreferredDate?.ToString("D", CultureInfo.InvariantCulture) ?? "Not specified",
                ["Adults"] = reservation.Adults.ToString(CultureInfo.InvariantCulture),
                ["Children"] = reservation.Children.ToString(CultureInfo.InvariantCulture),
                ["Infants"] = reservation.Infants.ToString(CultureInfo.InvariantCulture),
                ["TotalPrice"] = reservation.TotalPrice.ToString("N0", CultureInfo.InvariantCulture),
                ["PaymentMethod"] = reservation.PaymentMethod.ToString(),
                ["PaymentReference"] = reservation.PaymentReference ?? string.Empty,
                ["PickupLocation"] = reservation.PickupLocation,
                ["HotelName"] = reservation.HotelName ?? string.Empty,
                ["RoomNumber"] = reservation.RoomNumber ?? string.Empty,
                ["Notes"] = reservation.Notes ?? string.Empty,
                ["Language"] = reservation.Language ?? LanguageCatalog.Normalize(language),
                ["CreatedAt"] = reservation.CreatedAt.ToLocalTime().ToString("f")
            };

            var userTemplate = await ResolveTemplateAsync(EmailTemplateKeys.ReservationUser, language, ct);
            var (userSubject, userBody) = RenderTemplate(
                userTemplate,
                tokens,
                $"Reservation received – {tour.TourName}",
                $"Hello {tokens["FullName"]},\n\nThank you for booking {tour.TourName} with Tour Antalya.\n\nReservation ID: {tokens["ReservationId"]}\nPreferred date: {tokens["PreferredDate"]}\nGuests: Adults {tokens["Adults"]}, Children {tokens["Children"]}, Infants {tokens["Infants"]}\nPayment method: {tokens["PaymentMethod"]}\n\nWe will contact you shortly with confirmation details.\n\nBest regards,\nTour Antalya");

            await _emailSender.SendEmailAsync(reservation.CustomerEmail, userSubject, userBody, tokens["FullName"], ct);

            var adminRecipients = ParseRecipients(smtp.NotificationEmail);
            if (adminRecipients.Count > 0)
            {
                var adminTemplate = await ResolveTemplateAsync(EmailTemplateKeys.ReservationAdmin, language, ct);
                var (adminSubject, adminBody) = RenderTemplate(
                    adminTemplate,
                    tokens,
                    $"New reservation – {tour.TourName}",
                    $"A new reservation has been placed.\n\nTour: {tokens["TourName"]}\nReservation ID: {tokens["ReservationId"]}\nGuest: {tokens["FullName"]}\nEmail: {tokens["Email"]}\nPhone: {tokens["Phone"]}\nPreferred date: {tokens["PreferredDate"]}\nAdults: {tokens["Adults"]} · Children: {tokens["Children"]} · Infants: {tokens["Infants"]}\nPickup/location: {tokens["PickupLocation"]}\nHotel: {tokens["HotelName"]} {tokens["RoomNumber"]}\nNotes: {tokens["Notes"]}\nPayment method: {tokens["PaymentMethod"]}\nPayment reference: {tokens["PaymentReference"]}\nCreated: {tokens["CreatedAt"]}");

                await _emailSender.SendEmailAsync(adminRecipients, adminSubject, adminBody, ct);
            }
        }

        private async Task<EmailTemplate?> ResolveTemplateAsync(string templateKey, string language, CancellationToken ct)
        {
            var normalized = LanguageCatalog.Normalize(language);
            var template = await _emailConfigRepository.GetTemplateAsync(templateKey, normalized, ct);
            if (template is null && !string.Equals(normalized, "en", StringComparison.OrdinalIgnoreCase))
            {
                template = await _emailConfigRepository.GetTemplateAsync(templateKey, "en", ct);
            }

            if (template is null)
            {
                template = await _emailConfigRepository.GetTemplateAsync(templateKey, null, ct);
            }

            return template;
        }

        private static (string Subject, string Body) RenderTemplate(EmailTemplate? template, IDictionary<string, string> tokens, string defaultSubject, string defaultBody)
        {
            var subject = template?.Subject ?? defaultSubject;
            var body = template?.Body ?? defaultBody;

            foreach (var kvp in tokens)
            {
                var placeholder = "{" + kvp.Key + "}";
                var replacement = kvp.Value ?? string.Empty;
                subject = subject.Replace(placeholder, replacement, StringComparison.OrdinalIgnoreCase);
                body = body.Replace(placeholder, replacement, StringComparison.OrdinalIgnoreCase);
            }

            return (subject, body);
        }

        private static List<string> ParseRecipients(string? addresses)
        {
            if (string.IsNullOrWhiteSpace(addresses))
            {
                return new List<string>();
            }

            var separators = new[] { ',', ';', ' ' };
            return addresses
                .Split(separators, StringSplitOptions.RemoveEmptyEntries)
                .Select(address => address.Trim())
                .Where(address => !string.IsNullOrWhiteSpace(address))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();
        }
    }
}
