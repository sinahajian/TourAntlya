using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Models.Interface;
using Models.Entities;
using Models.Helper;
using Models.Services;
using PaypalServerSdk.Standard.Exceptions;
using PaypalServerSdk.Standard.Models;
namespace Controllers
{
    public class HomeController : Controller
    {
        private const string DefaultAdminEmail = "anbalya@proton.me";

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
        private readonly IInvoiceSettingsRepository _invoiceSettingsRepository;
        private readonly IInvoiceDocumentService _invoiceDocumentService;
        private readonly IPayPalHelper _payPalHelper;
        private readonly ILogger<HomeController> _logger;
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
            IEmailSender emailSender,
            IInvoiceSettingsRepository invoiceSettingsRepository,
            IInvoiceDocumentService invoiceDocumentService,
            IPayPalHelper payPalHelper,
            ILogger<HomeController> logger)
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
            _invoiceSettingsRepository = invoiceSettingsRepository;
            _invoiceDocumentService = invoiceDocumentService;
            _payPalHelper = payPalHelper;
            _logger = logger;
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
            var payPalOptions = paymentOptions
                .Where(o => o.Method == PaymentMethod.PayPal)
                .ToList();
            if (payPalOptions.Count == 0)
            {
                payPalOptions.Add(new PaymentOption
                {
                    Method = PaymentMethod.PayPal,
                    DisplayName = "PayPal",
                    AccountIdentifier = payPalSettings.BusinessEmail,
                    Instructions = "Complete your reservation and follow the PayPal instructions we send by email.",
                    IsEnabled = true
                });
            }
            var enabledOptions = payPalOptions
                .Where(o => o.IsEnabled)
                .Select(PaymentOptionDto.FromEntity)
                .ToList();

            if (enabledOptions.Count == 0)
            {
                enabledOptions = payPalOptions.Select(PaymentOptionDto.FromEntity).ToList();
            }

            if (enabledOptions.Count == 0)
            {
                enabledOptions.Add(new PaymentOptionDto(
                    0,
                    PaymentMethod.PayPal,
                    "PayPal",
                    payPalSettings.BusinessEmail,
                    "Complete your reservation and follow the PayPal instructions we send by email.",
                    true));
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
                    HotelName = string.Empty
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
        public async Task<IActionResult> BookTour(CancellationToken ct)
        {
            var postedForm = Request.HasFormContentType ? Request.Form : null;
            LogIncomingForm(postedForm);

            var form = MapReservationForm(postedForm);

            if (form.TourId <= 0)
            {
                var rawTourId = Request.Query["tourId"].FirstOrDefault();
                if (int.TryParse(rawTourId, NumberStyles.Integer, CultureInfo.InvariantCulture, out var parsedId))
                {
                    form.TourId = parsedId;
                }
            }

            form.PaymentMethod = form.PaymentMethod == 0 ? PaymentMethod.PayPal : form.PaymentMethod;
            form.PreferredDate = NormalizePreferredDate(form.PreferredDate);

            ModelState.Clear();
            TryValidateModel(form, "Form");

            var lang = _langResolver.Resolve(HttpContext);
            LogFormState(form, postedForm);
            var tour = await _tourRepository.GetByIdAsync(form.TourId, lang, ct);
            if (tour is null)
            {
                TempData["BookingError"] = "We could not find this tour. Please try again from the tour page.";
                return RedirectToAction(nameof(Index));
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
            var payPalOptions = paymentOptions
                .Where(o => o.Method == PaymentMethod.PayPal)
                .ToList();
            if (payPalOptions.Count == 0)
            {
                payPalOptions.Add(new PaymentOption
                {
                    Method = PaymentMethod.PayPal,
                    DisplayName = "PayPal",
                    AccountIdentifier = payPalSettings.BusinessEmail,
                    Instructions = "Complete your reservation and follow the PayPal instructions we send by email.",
                    IsEnabled = true
                });
            }
            var enabledOptions = payPalOptions
                .Where(o => o.IsEnabled)
                .Select(PaymentOptionDto.FromEntity)
                .ToList();

            if (!ModelState.IsValid)
            {
                var allErrors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .Where(m => !string.IsNullOrWhiteSpace(m))
                    .Distinct()
                    .ToList();

                var errorMessage = allErrors.Count switch
                {
                    > 1 => string.Join(" ", allErrors),
                    1 => allErrors[0],
                    _ => "Please correct the highlighted fields and try again."
                };

                var errorViewModel = new TourBookingPageViewModel
                {
                    Tour = tour,
                    Form = form,
                    PaymentOptions = enabledOptions.Count > 0
                        ? enabledOptions
                        : payPalOptions.Select(PaymentOptionDto.FromEntity).ToList(),
                    ErrorMessage = errorMessage,
                    AccommodationOptions = AccommodationCatalog.List(),
                    PayPal = payPalInfo
                };

                ViewData["Title"] = $"{tour.TourName} Antalya Tour";
                return View("Tour", errorViewModel);
            }

            var totalPrice = CalculateTotalPrice(tour, form);
            var paymentReference = string.IsNullOrWhiteSpace(form.PaymentReference)
                ? GeneratePaymentReference(tour.Id)
                : form.PaymentReference.Trim();
            var firstName = form.FirstName?.Trim() ?? string.Empty;
            var lastName = form.LastName?.Trim() ?? string.Empty;
            var fullName = string.Join(" ", new[] { firstName, lastName }
                .Where(part => !string.IsNullOrWhiteSpace(part)));
            var hotelName = string.IsNullOrWhiteSpace(form.HotelName) ? null : form.HotelName.Trim();

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
                PreferredDate = NormalizePreferredDate(form.PreferredDate),
                Adults = form.Adults,
                Children = form.Children,
                Infants = form.Infants,
                PickupLocation = pickupLocation ?? string.Empty,
                Notes = string.IsNullOrWhiteSpace(form.Notes) ? null : form.Notes.Trim(),
                PaymentMethod = form.PaymentMethod,
                PaymentStatus = PaymentStatus.Pending,
                Status = ReservationStatus.Pending,
                PaymentReference = paymentReference,
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
            var paymentOption = payPalOptions.FirstOrDefault();

            var confirmationViewModel = new ReservationConfirmationViewModel
            {
                Tour = tour,
                Reservation = reservationDto,
                PaymentOption = paymentOption is null ? null : PaymentOptionDto.FromEntity(paymentOption)
            };

            await SendReservationNotificationsAsync(tour, reservationDto, lang, ct);

            var wantsPayPalCheckout = (form.PayWithPayPal || form.PaymentMethod == PaymentMethod.PayPal) &&
                                      !string.IsNullOrWhiteSpace(payPalSettings.BusinessEmail);

            if (wantsPayPalCheckout)
            {
                var payPalOrder = await CreatePayPalOrderAsync(tour, reservationDto, payPalSettings, ct);
                if (payPalOrder.Success && !string.IsNullOrWhiteSpace(payPalOrder.ApprovalUrl))
                {
                    return Redirect(payPalOrder.ApprovalUrl);
                }

                var fallbackUrl = BuildLegacyPayPalUrl(payPalSettings, reservationDto, tour);
                if (!string.IsNullOrWhiteSpace(fallbackUrl))
                {
                    _logger.LogWarning("PayPal order API failed; falling back to hosted checkout for reservation {ReservationId}: {Error}", reservationDto.Id, payPalOrder.Error ?? "Unknown error");
                    return Redirect(fallbackUrl);
                }

                ViewBag.PayPalError = "We saved your reservation but could not start PayPal checkout. Please try again or contact us to complete payment.";
                _logger.LogWarning("PayPal order creation failed for reservation {ReservationId}: {Error}", reservationDto.Id, payPalOrder.Error ?? "Unknown error");
            }

            ViewData["Title"] = $"{tour.TourName} Antalya Tour Confirmation";
            return View("ReservationConfirmation", confirmationViewModel);
        }

        [HttpGet]
        public IActionResult BookTour()
        {
            TempData["BookingError"] = "Please start your reservation from a tour page.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet("paypal/success")]
        public async Task<IActionResult> PayPalSuccess(CancellationToken ct)
        {
            var lang = _langResolver.Resolve(HttpContext);
            var invoice = Request.Query["invoice"].ToString();
            var reservationIdRaw = Request.Query["reservationId"].ToString();
            var token = Request.Query["token"].ToString();
            var payerId = Request.Query["PayerID"].ToString();
            var currencyRaw = Request.Query["currency_code"].ToString();
            var amountRaw = Request.Query["amount"].ToString();
            var payPalSettings = await _payPalSettingsRepository.GetAsync(ct);
            var currency = string.IsNullOrWhiteSpace(currencyRaw)
                ? (string.IsNullOrWhiteSpace(payPalSettings.Currency) ? "EUR" : payPalSettings.Currency.ToUpperInvariant())
                : currencyRaw.ToUpperInvariant();
            var amount = decimal.TryParse(amountRaw, NumberStyles.Any, CultureInfo.InvariantCulture, out var parsedAmount)
                ? parsedAmount
                : 0m;

            var reservationEntity = await FindReservationAsync(reservationIdRaw, invoice, ct);
            ReservationDetailsDto? reservation = null;
            TourDto? tour = null;
            var success = false;
            var message = "We could not match this payment to an existing reservation. Please contact our support team.";
            var title = "Payment not found";
            var justMarkedPaid = false;
            var alreadyPaid = false;

            if (reservationEntity is not null)
            {
                var verification = await CapturePayPalOrderAsync(token, reservationEntity, payPalSettings, ct);
                if (verification.Success)
                {
                    if (reservationEntity.PaymentStatus != PaymentStatus.Paid)
                    {
                        await _reservationRepository.UpdateStatusAsync(
                            reservationEntity.Id,
                            ReservationStatus.Confirmed,
                            PaymentStatus.Paid,
                            reservationEntity.PaymentReference ?? invoice,
                            ct);
                        justMarkedPaid = true;
                    }
                    else
                    {
                        alreadyPaid = true;
                    }

                    reservationEntity = await _reservationRepository.GetByIdAsync(reservationEntity.Id, ct) ?? reservationEntity;
                    reservation = ReservationDetailsDto.FromEntity(reservationEntity);
                    var reservationLang = reservationEntity.Language ?? lang;
                    tour = await _tourRepository.GetByIdAsync(reservationEntity.TourId, reservationLang, ct);

                    success = true;
                    title = "Payment successful";
                    currency = verification.Currency ?? currency ?? payPalSettings.Currency ?? "EUR";
                    amount = verification.Amount ?? amount;
                    message = alreadyPaid
                        ? "This payment was already confirmed earlier. We have your reservation on file."
                        : "Thank you! We have confirmed your payment and will send the final instructions shortly.";

                    if (justMarkedPaid && tour is not null && reservation is not null)
                    {
                        await SendPaymentConfirmationEmailsAsync(
                            tour,
                            reservation,
                            verification.Amount ?? amount,
                            currency,
                            reservation.Language ?? lang,
                            ct);
                    }
                }
                else
                {
                    await _reservationRepository.UpdateStatusAsync(
                        reservationEntity.Id,
                        ReservationStatus.Pending,
                        PaymentStatus.Failed,
                        reservationEntity.PaymentReference ?? invoice,
                        ct);

                    reservationEntity = await _reservationRepository.GetByIdAsync(reservationEntity.Id, ct) ?? reservationEntity;
                    reservation = ReservationDetailsDto.FromEntity(reservationEntity);
                    var reservationLang = reservationEntity.Language ?? lang;
                    tour = await _tourRepository.GetByIdAsync(reservationEntity.TourId, reservationLang, ct);

                    title = "Payment not confirmed";
                    currency = verification.Currency ?? currency;
                    amount = verification.Amount ?? amount;
                    message = string.IsNullOrWhiteSpace(verification.Error)
                        ? "We could not confirm this payment with PayPal. Please try again or contact support."
                        : $"We could not confirm this payment with PayPal: {verification.Error}";

                    if (tour is not null && reservation is not null)
                    {
                        await SendPaymentFailedEmailsAsync(tour, reservation, amount, currency, reservationLang, ct);
                    }
                }
            }

            var viewModel = new PayPalResultViewModel
            {
                Success = success,
                Title = title,
                Message = message,
                Invoice = invoice ?? string.Empty,
                Amount = amount,
                Currency = currency,
                PayerId = payerId,
                Token = token,
                Reservation = reservation,
                Tour = tour
            };

            ViewData["Title"] = title;
            return View("PayPalSuccess", viewModel);
        }

        [HttpGet("paypal/cancel")]
        public async Task<IActionResult> PayPalCancel(CancellationToken ct)
        {
            var lang = _langResolver.Resolve(HttpContext);
            var invoice = Request.Query["invoice"].ToString();
            var reservationIdRaw = Request.Query["reservationId"].ToString();
            var token = Request.Query["token"].ToString();
            var payerId = Request.Query["PayerID"].ToString();
            var currencyRaw = Request.Query["currency_code"].ToString();
            var amountRaw = Request.Query["amount"].ToString();
            var payPalSettings = await _payPalSettingsRepository.GetAsync(ct);
            var currency = string.IsNullOrWhiteSpace(currencyRaw)
                ? (string.IsNullOrWhiteSpace(payPalSettings.Currency) ? "EUR" : payPalSettings.Currency.ToUpperInvariant())
                : currencyRaw.ToUpperInvariant();
            var amount = decimal.TryParse(amountRaw, NumberStyles.Any, CultureInfo.InvariantCulture, out var parsedAmount)
                ? parsedAmount
                : 0m;

            var reservationEntity = await FindReservationAsync(reservationIdRaw, invoice, ct);
            ReservationDetailsDto? reservation = null;
            TourDto? tour = null;
            var message = "The payment was cancelled before completion. Your reservation remains pending.";

            if (reservationEntity is not null)
            {
                await _reservationRepository.UpdateStatusAsync(
                    reservationEntity.Id,
                    ReservationStatus.Cancelled,
                    PaymentStatus.Failed,
                    reservationEntity.PaymentReference ?? invoice,
                    ct);

                reservationEntity = await _reservationRepository.GetByIdAsync(reservationEntity.Id, ct) ?? reservationEntity;
                reservation = ReservationDetailsDto.FromEntity(reservationEntity);
                var reservationLang = reservationEntity.Language ?? lang;
                tour = await _tourRepository.GetByIdAsync(reservationEntity.TourId, reservationLang, ct);

                if (tour is not null && reservation is not null)
                {
                    await SendPaymentFailedEmailsAsync(tour, reservation, amount, currency, reservationLang, ct);
                }
            }
            else
            {
                message = string.IsNullOrWhiteSpace(invoice)
                    ? "We did not receive a payment reference from PayPal. Please try again or contact support."
                    : "We did not find a reservation matching this payment reference. If you created a booking, please contact us.";
            }

            var viewModel = new PayPalResultViewModel
            {
                Success = false,
                Title = "Payment cancelled",
                Message = message,
                Invoice = invoice ?? string.Empty,
                Amount = amount,
                Currency = currency,
                PayerId = payerId,
                Token = token,
                Reservation = reservation,
                Tour = tour
            };

            ViewData["Title"] = "Payment cancelled";
            return View("PayPalCancel", viewModel);
        }

        private void ValidateReservationForm(TourReservationInputModel form)
        {
            const string prefix = "Form.";

            if (form.PreferredDate.HasValue && form.PreferredDate.Value.Date <= DateTime.UtcNow.Date)
            {
                ModelState.AddModelError(prefix + nameof(form.PreferredDate), "Please choose a date at least 1 day in advance.");
            }

            var totalGuests = Math.Max(0, form.Adults) + Math.Max(0, form.Children) + Math.Max(0, form.Infants);
            if (totalGuests <= 0)
            {
                ModelState.AddModelError(prefix + nameof(form.Adults), "At least one guest is required.");
            }

            var hotelValue = form.HotelName?.Trim();
            var hasHotelSelection = !string.IsNullOrWhiteSpace(hotelValue);

            if (!hasHotelSelection && string.IsNullOrWhiteSpace(form.PickupLocation))
            {
                var message = "Please select your hotel or enter a pickup location.";
                ModelState.AddModelError(prefix + nameof(form.HotelName), message);
                ModelState.AddModelError(prefix + nameof(form.PickupLocation), message);
            }
        }

        private TourReservationInputModel MapReservationForm(IFormCollection? form)
        {
            if (form is null || form.Count == 0)
            {
                return new TourReservationInputModel();
            }

            string GetValue(params string[] keys)
            {
                foreach (var key in keys)
                {
                    if (string.IsNullOrWhiteSpace(key)) continue;
                    var values = form[key];
                    if (values.Count == 0) continue;

                    var candidate = values.LastOrDefault(v => !string.IsNullOrWhiteSpace(v))
                                   ?? values.LastOrDefault();

                    if (candidate is not null)
                    {
                        return candidate.Trim();
                    }
                }

                return string.Empty;
            }

            int GetInt(int fallback, params string[] keys)
            {
                foreach (var key in keys)
                {
                    if (string.IsNullOrWhiteSpace(key)) continue;
                    var raw = GetValue(key);
                    if (int.TryParse(raw, NumberStyles.Integer, CultureInfo.InvariantCulture, out var parsed))
                    {
                        return parsed;
                    }
                }

                return fallback;
            }

            DateTime? GetDate(params string[] keys)
            {
                foreach (var key in keys)
                {
                    var raw = GetValue(key);
                    if (!string.IsNullOrWhiteSpace(raw) &&
                        DateTime.TryParse(raw, CultureInfo.CurrentCulture, DateTimeStyles.AssumeLocal, out var parsed))
                    {
                        return parsed;
                    }
                }

                return null;
            }

            bool GetBool(params string[] keys)
            {
                foreach (var key in keys)
                {
                    var raw = GetValue(key);
                    if (string.IsNullOrWhiteSpace(raw)) continue;
                    if (string.Equals(raw, "true", StringComparison.OrdinalIgnoreCase) ||
                        string.Equals(raw, "on", StringComparison.OrdinalIgnoreCase) ||
                        raw == "1")
                    {
                        return true;
                    }
                }

                return false;
            }

            var formModel = new TourReservationInputModel
            {
                TourId = GetInt(0, "Form.TourId", "TourId"),
                Language = GetValue("Form.Language", "Language"),
                PaymentReference = GetValue("Form.PaymentReference", "PaymentReference"),
                PayWithPayPal = GetBool("Form.PayWithPayPal", "PayWithPayPal"),
                FirstName = GetValue("Form.FirstName", "FirstName"),
                LastName = GetValue("Form.LastName", "LastName"),
                CustomerEmail = GetValue("Form.CustomerEmail", "CustomerEmail"),
                CustomerPhone = GetValue("Form.CustomerPhone", "CustomerPhone"),
                PreferredDate = GetDate("Form.PreferredDate", "PreferredDate"),
                Adults = GetInt(1, "Form.Adults", "Adults"),
                Children = GetInt(0, "Form.Children", "Children"),
                Infants = GetInt(0, "Form.Infants", "Infants"),
                PickupLocation = GetValue("Form.PickupLocation", "PickupLocation"),
                HotelName = GetValue("Form.HotelName", "HotelName"),
                RoomNumber = GetValue("Form.RoomNumber", "RoomNumber"),
                Notes = GetValue("Form.Notes", "Notes"),
                PaymentMethod = ParsePaymentMethod(GetValue("Form.PaymentMethod", "PaymentMethod"))
            };

            return formModel;
        }

        private PaymentMethod ParsePaymentMethod(string raw)
        {
            if (string.IsNullOrWhiteSpace(raw))
            {
                return PaymentMethod.PayPal;
            }

            if (Enum.TryParse<PaymentMethod>(raw, true, out var parsed) &&
                Enum.IsDefined(typeof(PaymentMethod), parsed))
            {
                return parsed;
            }

            if (int.TryParse(raw, NumberStyles.Integer, CultureInfo.InvariantCulture, out var value) &&
                Enum.IsDefined(typeof(PaymentMethod), value))
            {
                return (PaymentMethod)value;
            }

            return PaymentMethod.PayPal;
        }

        private void LogIncomingForm(IFormCollection? form)
        {
            if (form is null)
            {
                _logger.LogWarning("BookTour: no form content received.");
                return;
            }

            var keyList = string.Join(", ", form.Keys);
            var values = string.Join("; ", form.Keys.Select(k => $"{k}='{string.Join("|", form[k].ToArray())}'"));
            _logger.LogInformation("BookTour raw form keys: {Keys}", string.IsNullOrWhiteSpace(keyList) ? "(none)" : keyList);
            _logger.LogInformation("BookTour raw form values: {Values}", values);
        }

        private void LogFormState(TourReservationInputModel form, IFormCollection? postedForm = null)
        {
            var keys = postedForm is not null ? string.Join(", ", postedForm.Keys) : "(no form)";
            _logger.LogInformation("BookTour form keys: {Keys}", keys);
            _logger.LogInformation("BookTour values: TourId={TourId}, FirstName='{FirstName}', LastName='{LastName}', Email='{Email}', Phone='{Phone}', Hotel='{Hotel}', Pickup='{Pickup}', Adults={Adults}, Children={Children}, Infants={Infants}, PreferredDate='{PreferredDate}'",
                form.TourId,
                form.FirstName,
                form.LastName,
                form.CustomerEmail,
                form.CustomerPhone,
                form.HotelName,
                form.PickupLocation,
                form.Adults,
                form.Children,
                form.Infants,
                form.PreferredDate?.ToString("u") ?? "(null)");
        }

        private static DateTime? NormalizePreferredDate(DateTime? date)
        {
            if (!date.HasValue)
            {
                return null;
            }

            var day = date.Value.Date;
            if (day.Kind == DateTimeKind.Unspecified)
            {
                return DateTime.SpecifyKind(day, DateTimeKind.Utc);
            }

            if (day.Kind == DateTimeKind.Local)
            {
                return day.ToUniversalTime();
            }

            return day;
        }

        private static int CalculateTotalPrice(TourDto tour, TourReservationInputModel form)
        {
            var adultTotal = Math.Max(0, form.Adults) * tour.Price;
            var childTotal = Math.Max(0, form.Children) * tour.KinderPrice;
            var infantTotal = Math.Max(0, form.Infants) * tour.InfantPrice;
            return adultTotal + childTotal + infantTotal;
        }

        private static string GeneratePaymentReference(int tourId) =>
            $"TA-{tourId}-{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}";

        private async Task<PayPalOrderResult> CreatePayPalOrderAsync(
            TourDto tour,
            ReservationDetailsDto reservation,
            PayPalSettings payPalSettings,
            CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(payPalSettings.BusinessEmail) ||
                string.IsNullOrWhiteSpace(payPalSettings.ClientId) ||
                string.IsNullOrWhiteSpace(payPalSettings.ClientSecret))
            {
                return new PayPalOrderResult(false, null, null, null, "PayPal is not configured.");
            }

            var currency = string.IsNullOrWhiteSpace(payPalSettings.Currency) ? "EUR" : payPalSettings.Currency.ToUpperInvariant();
            var invoice = reservation.PaymentReference ?? GeneratePaymentReference(reservation.TourId);
            var returnUrl = BuildPayPalCallbackUrl(payPalSettings.ReturnUrl, nameof(PayPalSuccess), invoice, reservation.Id);
            var cancelUrl = BuildPayPalCallbackUrl(payPalSettings.CancelUrl, nameof(PayPalCancel), invoice, reservation.Id);

            var orderRequest = new OrderRequest
            {
                Intent = CheckoutPaymentIntent.Capture,
                PurchaseUnits = new List<PurchaseUnitRequest>
                {
                    new()
                    {
                        ReferenceId = reservation.Id.ToString(CultureInfo.InvariantCulture),
                        CustomId = reservation.Id.ToString(CultureInfo.InvariantCulture),
                        InvoiceId = invoice,
                        Description = $"Reservation #{reservation.Id} – {tour.TourName}",
                        Amount = new AmountWithBreakdown
                        {
                            CurrencyCode = currency,
                            MValue = Convert.ToDecimal(reservation.TotalPrice).ToString("0.00", CultureInfo.InvariantCulture)
                        }
                    }
                },
                ApplicationContext = new OrderApplicationContext
                {
                    BrandName = "Tour Antalya",
                    UserAction = OrderApplicationContextUserAction.PayNow,
                    ShippingPreference = OrderApplicationContextShippingPreference.NoShipping,
                    Locale = reservation.Language,
                    ReturnUrl = returnUrl,
                    CancelUrl = cancelUrl
                }
            };

            var input = new CreateOrderInput
            {
                Body = orderRequest,
                ContentType = "application/json"
            };

            try
            {
                var orderResponse = await _payPalHelper.Client.OrdersController.CreateOrderAsync(input, ct);
                var order = orderResponse?.Data;
                var approvalUrl = order?.Links?.FirstOrDefault(l => string.Equals(l.Rel, "approve", StringComparison.OrdinalIgnoreCase))?.Href;

                if (!string.IsNullOrWhiteSpace(order?.Id) && !string.IsNullOrWhiteSpace(approvalUrl))
                {
                    return new PayPalOrderResult(true, order.Id, approvalUrl, order.Status?.ToString() ?? "Created", null);
                }

                return new PayPalOrderResult(false, order?.Id, approvalUrl, order?.Status?.ToString() ?? "Unknown", "PayPal did not return an approval URL.");
            }
            catch (ApiException ex)
            {
                _logger.LogError(ex, "PayPal create order failed for reservation {ReservationId}", reservation.Id);
                return new PayPalOrderResult(false, null, null, null, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "PayPal create order failed for reservation {ReservationId}", reservation.Id);
                return new PayPalOrderResult(false, null, null, null, ex.Message);
            }
        }

        private async Task<PayPalVerificationResult> CapturePayPalOrderAsync(
            string orderId,
            Reservation reservation,
            PayPalSettings payPalSettings,
            CancellationToken ct)
        {
            var currency = string.IsNullOrWhiteSpace(payPalSettings.Currency) ? "EUR" : payPalSettings.Currency.ToUpperInvariant();
            var expectedAmount = Convert.ToDecimal(reservation.TotalPrice);

            if (string.IsNullOrWhiteSpace(orderId))
            {
                return new PayPalVerificationResult(false, "missing_token", null, null, expectedAmount, currency, "PayPal did not return a payment token.");
            }

            try
            {
                var ordersController = _payPalHelper.Client.OrdersController;
                var orderDetailsResponse = await ordersController.GetOrderAsync(new GetOrderInput { Id = orderId }, ct);
                var orderDetails = orderDetailsResponse?.Data;
                var initialCapture = ExtractCapture(orderDetails);
                var initialAmount = ParseAmount(initialCapture?.Amount) ?? ParseAmount(orderDetails?.PurchaseUnits?.FirstOrDefault()?.Amount);
                var initialCurrency = initialCapture?.Amount?.CurrencyCode ?? orderDetails?.PurchaseUnits?.FirstOrDefault()?.Amount?.CurrencyCode ?? currency;

                if (orderDetails?.Status == OrderStatus.Completed && IsCaptureValid(initialCapture, reservation, expectedAmount, currency))
                {
                    return new PayPalVerificationResult(true, orderDetails.Status?.ToString() ?? "Completed", orderDetails.Id, initialCapture?.Id, initialAmount ?? expectedAmount, initialCurrency, null);
                }

                var captureResponse = await ordersController.CaptureOrderAsync(new CaptureOrderInput
                {
                    Id = orderId,
                    ContentType = "application/json",
                    Prefer = "return=representation",
                    Body = new OrderCaptureRequest()
                }, ct);

                var capturedOrder = captureResponse?.Data;
                var capture = ExtractCapture(capturedOrder);
                var captureAmount = ParseAmount(capture?.Amount) ?? ParseAmount(capturedOrder?.PurchaseUnits?.FirstOrDefault()?.Amount);
                var captureCurrency = capture?.Amount?.CurrencyCode ?? capturedOrder?.PurchaseUnits?.FirstOrDefault()?.Amount?.CurrencyCode ?? currency;
                var captureStatus = capturedOrder?.Status?.ToString() ?? capture?.Status?.ToString() ?? "unknown";

                if (capturedOrder?.Status == OrderStatus.Completed && IsCaptureValid(capture, reservation, expectedAmount, currency))
                {
                    return new PayPalVerificationResult(true, captureStatus, capturedOrder.Id, capture?.Id, captureAmount ?? expectedAmount, captureCurrency, null);
                }

                var error = capture?.StatusDetails?.Reason?.ToString() ?? "PayPal did not confirm the payment.";
                return new PayPalVerificationResult(false, captureStatus, capturedOrder?.Id ?? orderId, capture?.Id, captureAmount ?? initialAmount, captureCurrency, error);
            }
            catch (ApiException ex)
            {
                _logger.LogError(ex, "PayPal capture failed for reservation {ReservationId}", reservation.Id);
                return new PayPalVerificationResult(false, "error", orderId, null, expectedAmount, currency, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "PayPal capture failed for reservation {ReservationId}", reservation.Id);
                return new PayPalVerificationResult(false, "error", orderId, null, expectedAmount, currency, ex.Message);
            }
        }

        private string BuildPayPalCallbackUrl(string? baseUrl, string actionName, string invoice, int reservationId)
        {
            var target = !string.IsNullOrWhiteSpace(baseUrl)
                ? baseUrl
                : Url.Action(actionName, "Home", null, Request.Scheme, Request.Host.ToString()) ?? string.Empty;

            if (string.IsNullOrWhiteSpace(target))
            {
                return string.Empty;
            }

            var query = new Dictionary<string, string?>
            {
                ["invoice"] = invoice,
                ["reservationId"] = reservationId.ToString(CultureInfo.InvariantCulture)
            };

            return QueryHelpers.AddQueryString(target, query!);
        }

        private string BuildPayPalCallbackUrl(string? baseUrl, string actionName, string invoice, int reservationId, PayPalSettings settings)
        {
            var preferLocal =
                HttpContext?.Request?.Host.HasValue == true &&
                (HttpContext.Request.Host.Host.Contains("localhost", StringComparison.OrdinalIgnoreCase)
                 || HttpContext.Request.Host.Host.StartsWith("127.", StringComparison.OrdinalIgnoreCase)
                 || HttpContext.Request.Host.Host.StartsWith("192.168.", StringComparison.OrdinalIgnoreCase)
                 || settings.UseSandbox);

            var target = (!preferLocal && !string.IsNullOrWhiteSpace(baseUrl))
                ? baseUrl
                : Url.Action(actionName, "Home", null, Request.Scheme, Request.Host.ToString()) ?? string.Empty;

            if (!Uri.TryCreate(target, UriKind.Absolute, out _))
            {
                target = Url.Action(actionName, "Home", null, Request.Scheme, Request.Host.ToString()) ?? string.Empty;
            }

            if (string.IsNullOrWhiteSpace(target))
            {
                return string.Empty;
            }

            var query = new Dictionary<string, string?>
            {
                ["invoice"] = invoice,
                ["reservationId"] = reservationId.ToString(CultureInfo.InvariantCulture)
            };

            return QueryHelpers.AddQueryString(target, query!);
        }

        private string? BuildLegacyPayPalUrl(PayPalSettings settings, ReservationDetailsDto reservation, TourDto tour)
        {
            if (string.IsNullOrWhiteSpace(settings.BusinessEmail))
            {
                return null;
            }

            var baseUrl = settings.BaseUrl;
            var amount = reservation.TotalPrice <= 0 ? 0 : reservation.TotalPrice;
            var invoice = reservation.PaymentReference ?? GeneratePaymentReference(reservation.TourId);
            var returnUrl = BuildPayPalCallbackUrl(settings.ReturnUrl, nameof(PayPalSuccess), invoice, reservation.Id, settings);
            var cancelUrl = BuildPayPalCallbackUrl(settings.CancelUrl, nameof(PayPalCancel), invoice, reservation.Id, settings);

            var query = new Dictionary<string, string?>
            {
                ["cmd"] = "_xclick",
                ["business"] = settings.BusinessEmail,
                ["currency_code"] = string.IsNullOrWhiteSpace(settings.Currency) ? "EUR" : settings.Currency,
                ["amount"] = (amount <= 0 ? 1 : amount).ToString("0.00", CultureInfo.InvariantCulture),
                ["item_name"] = $"{tour.TourName} – Reservation #{reservation.Id}",
                ["invoice"] = invoice
            };

            if (!string.IsNullOrWhiteSpace(returnUrl)) query["return"] = returnUrl;
            if (!string.IsNullOrWhiteSpace(cancelUrl)) query["cancel_return"] = cancelUrl;

            return QueryHelpers.AddQueryString(baseUrl, query);
        }

        private async Task<Reservation?> FindReservationAsync(string reservationIdRaw, string invoice, CancellationToken ct)
        {
            if (int.TryParse(reservationIdRaw, NumberStyles.Integer, CultureInfo.InvariantCulture, out var reservationId))
            {
                var byId = await _reservationRepository.GetByIdAsync(reservationId, ct);
                if (byId is not null)
                {
                    return byId;
                }
            }

            if (!string.IsNullOrWhiteSpace(invoice))
            {
                var byReference = await _reservationRepository.GetByPaymentReferenceAsync(invoice, ct);
                if (byReference is not null)
                {
                    return byReference;
                }
            }

            return null;
        }

        private static OrdersCapture? ExtractCapture(Order? order) =>
            order?.PurchaseUnits?
                .SelectMany(unit => unit?.Payments?.Captures ?? Enumerable.Empty<OrdersCapture>())
                .FirstOrDefault();

        private static bool IsCaptureValid(OrdersCapture? capture, Reservation reservation, decimal expectedAmount, string expectedCurrency)
        {
            if (capture?.Amount is null)
            {
                return false;
            }

            if (!string.Equals(capture.Amount.CurrencyCode, expectedCurrency, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            var paidAmount = ParseAmount(capture.Amount);
            if (!paidAmount.HasValue || Math.Abs(paidAmount.Value - expectedAmount) > 0.01m)
            {
                return false;
            }

            if (!string.IsNullOrWhiteSpace(reservation.PaymentReference))
            {
                var referenceMatches = string.Equals(capture.InvoiceId, reservation.PaymentReference, StringComparison.OrdinalIgnoreCase)
                                       || string.Equals(capture.CustomId, reservation.PaymentReference, StringComparison.OrdinalIgnoreCase);
                if (!referenceMatches)
                {
                    return false;
                }
            }

            return capture.Status == CaptureStatus.Completed;
        }

        private static decimal? ParseAmount(Money? money) => ParseAmount(money?.MValue);

        private static decimal? ParseAmount(AmountWithBreakdown? amount) => ParseAmount(amount?.MValue);

        private static decimal? ParseAmount(string? amountRaw)
        {
            if (string.IsNullOrWhiteSpace(amountRaw))
            {
                return null;
            }

            return decimal.TryParse(amountRaw, NumberStyles.Any, CultureInfo.InvariantCulture, out var value)
                ? value
                : null;
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

            await _emailSender.SendEmailAsync(
                message.Email,
                userSubject,
                userBody,
                toName: tokens["FullName"],
                isBodyHtml: true,
                ct: ct);

            var adminRecipients = ParseRecipients(smtp.NotificationEmail);
            if (adminRecipients.Count > 0)
            {
                var adminTemplate = await ResolveTemplateAsync(EmailTemplateKeys.ContactAdmin, language, ct);
                var (adminSubject, adminBody) = RenderTemplate(
                    adminTemplate,
                    tokens,
                    $"New contact request from {tokens["FullName"]}",
                    $"A new contact request has been submitted.\n\nName: {tokens["FullName"]}\nEmail: {tokens["Email"]}\nLanguage: {tokens["Language"]}\nReceived: {tokens["CreatedAt"]}\n\nMessage:\n{tokens["Message"]}");

                await _emailSender.SendEmailAsync(
                    adminRecipients,
                    adminSubject,
                    adminBody,
                    isBodyHtml: true,
                    ct: ct);
            }
        }

        private async Task SendReservationNotificationsAsync(TourDto tour, ReservationDetailsDto reservation, string language, CancellationToken ct)
        {
            var (tokens, smtp, _) = await BuildReservationContextAsync(tour, reservation, language, ct);

            var userTemplate = await ResolveTemplateAsync(EmailTemplateKeys.ReservationUser, language, ct);
            var (userSubject, userBody) = RenderTemplate(
                userTemplate,
                tokens,
                $"Reservation received – {tour.TourName}",
                $"Hello {tokens["FullName"]},\n\nThank you for booking {tour.TourName} with Tour Antalya.\n\nReservation ID: {tokens["ReservationId"]}\nPreferred date: {tokens["PreferredDate"]}\nGuests: Adults {tokens["Adults"]}, Children {tokens["Children"]}, Infants {tokens["Infants"]}\nPayment method: {tokens["PaymentMethod"]}\n\nWe will contact you shortly with confirmation details.\n\nBest regards,\nTour Antalya");

            await _emailSender.SendEmailAsync(
                reservation.CustomerEmail,
                userSubject,
                userBody,
                toName: tokens["FullName"],
                isBodyHtml: true,
                ct: ct);

            var adminRecipients = ParseRecipients(smtp.NotificationEmail);
            if (adminRecipients.Count > 0)
            {
                var adminTemplate = await ResolveTemplateAsync(EmailTemplateKeys.ReservationAdmin, language, ct);
                var (adminSubject, adminBody) = RenderTemplate(
                    adminTemplate,
                    tokens,
                    $"New reservation – {tour.TourName}",
                    $"A new reservation has been placed.\n\nTour: {tokens["TourName"]}\nReservation ID: {tokens["ReservationId"]}\nGuest: {tokens["FullName"]}\nEmail: {tokens["Email"]}\nPhone: {tokens["Phone"]}\nPreferred date: {tokens["PreferredDate"]}\nAdults: {tokens["Adults"]} · Children: {tokens["Children"]} · Infants: {tokens["Infants"]}\nPickup/location: {tokens["PickupLocation"]}\nHotel: {tokens["HotelName"]} {tokens["RoomNumber"]}\nNotes: {tokens["Notes"]}\nPayment method: {tokens["PaymentMethod"]}\nPayment reference: {tokens["PaymentReference"]}\nCreated: {tokens["CreatedAt"]}");

                await _emailSender.SendEmailAsync(
                    adminRecipients,
                    adminSubject,
                    adminBody,
                    isBodyHtml: true,
                    ct: ct);
            }
        }

        private async Task SendPaymentConfirmationEmailsAsync(TourDto tour, ReservationDetailsDto reservation, decimal amount, string currency, string language, CancellationToken ct)
        {
            var (tokens, smtp, invoiceSettings) = await BuildReservationContextAsync(tour, reservation, language, ct);
            var currencyCode = string.IsNullOrWhiteSpace(currency) ? "EUR" : currency.ToUpperInvariant();
            var effectiveAmount = amount > 0 ? amount : Convert.ToDecimal(reservation.TotalPrice);
            tokens["Currency"] = currencyCode;
            tokens["Amount"] = effectiveAmount.ToString("N2", CultureInfo.InvariantCulture);
            tokens["AmountWithCurrency"] = $"{currencyCode} {tokens["Amount"]}";

            var attachment = await _invoiceDocumentService.CreateInvoiceAsync(tour, reservation, effectiveAmount, currencyCode, ct);
            var attachments = attachment is null ? null : new[] { attachment };
            var guestName = tokens["FullName"];
            var paymentReferenceDisplay = string.IsNullOrWhiteSpace(tokens["PaymentReference"]) ? "—" : tokens["PaymentReference"];
            var phoneLine = string.IsNullOrWhiteSpace(tokens["CompanyPhone"]) ? string.Empty : $"<br/>{tokens["CompanyPhone"]}";
            var emailLine = string.IsNullOrWhiteSpace(tokens["CompanyEmail"]) ? string.Empty : $"<br/><a href=\"mailto:{tokens["CompanyEmail"]}\">{tokens["CompanyEmail"]}</a>";
            var contactSignature = $"{invoiceSettings.CompanyName}{phoneLine}{emailLine}";

            var userSubject = $"Payment confirmed – {tokens["TourName"]}";
            var userBody = $@"
<p>Hi {guestName},</p>
<p>We have received your payment of <strong>{tokens["AmountWithCurrency"]}</strong> for <strong>{tokens["TourName"]}</strong>.</p>
<p>
    Reservation ID: <strong>#{tokens["ReservationId"]}</strong><br/>
    Payment reference: <strong>{paymentReferenceDisplay}</strong><br/>
    Tour date: {tokens["PreferredDate"]}
</p>
<p>Your PDF receipt is attached. We will share pickup and transfer details 24 hours before departure.</p>
<p>Warm regards,<br/>{contactSignature}</p>";

            await _emailSender.SendEmailAsync(
                reservation.CustomerEmail,
                userSubject,
                userBody,
                toName: guestName,
                isBodyHtml: true,
                attachments: attachments,
                ct: ct);

            var adminRecipients = ParseRecipients(smtp.NotificationEmail);
            if (adminRecipients.Count > 0)
            {
                var adminSubject = $"Payment received – {tokens["TourName"]}";
                var adminBody = $@"
<p>A payment has been confirmed.</p>
<table style=""width:100%;border-collapse:collapse;"">
    <tr><th align=""left"" style=""padding:6px 8px;border-bottom:1px solid #dbe3f0;"">Guest</th><td style=""padding:6px 8px;border-bottom:1px solid #dbe3f0;"">{guestName} ({tokens["Email"]})</td></tr>
    <tr><th align=""left"" style=""padding:6px 8px;border-bottom:1px solid #dbe3f0;"">Amount</th><td style=""padding:6px 8px;border-bottom:1px solid #dbe3f0;"">{tokens["AmountWithCurrency"]}</td></tr>
    <tr><th align=""left"" style=""padding:6px 8px;border-bottom:1px solid #dbe3f0;"">Reference</th><td style=""padding:6px 8px;border-bottom:1px solid #dbe3f0;"">{paymentReferenceDisplay}</td></tr>
    <tr><th align=""left"" style=""padding:6px 8px;border-bottom:1px solid #dbe3f0;"">Reservation</th><td style=""padding:6px 8px;border-bottom:1px solid #dbe3f0;"">#{tokens["ReservationId"]} – {tokens["TourName"]}</td></tr>
    <tr><th align=""left"" style=""padding:6px 8px;"">Created</th><td style=""padding:6px 8px;"">{tokens["CreatedAt"]}</td></tr>
</table>
<p>Please update the itinerary and send the pickup confirmation to the guest.</p>";

                await _emailSender.SendEmailAsync(
                    adminRecipients,
                    adminSubject,
                    adminBody,
                    isBodyHtml: true,
                    attachments: attachments,
                    ct: ct);
            }
        }

        private async Task SendPaymentFailedEmailsAsync(
            TourDto tour,
            ReservationDetailsDto reservation,
            decimal amount,
            string currency,
            string language,
            CancellationToken ct)
        {
            var (tokens, smtp, _) = await BuildReservationContextAsync(tour, reservation, language, ct);
            var currencyCode = string.IsNullOrWhiteSpace(currency) ? "EUR" : currency.ToUpperInvariant();
            var effectiveAmount = amount > 0 ? amount : Convert.ToDecimal(reservation.TotalPrice);
            tokens["Currency"] = currencyCode;
            tokens["Amount"] = effectiveAmount.ToString("N2", CultureInfo.InvariantCulture);
            tokens["AmountWithCurrency"] = $"{currencyCode} {tokens["Amount"]}";

            var guestName = tokens["FullName"];
            var paymentReferenceDisplay = string.IsNullOrWhiteSpace(tokens["PaymentReference"]) ? "—" : tokens["PaymentReference"];
            var userSubject = $"Payment not completed – {tokens["TourName"]}";
            var userBody = $@"
<p>Hi {guestName},</p>
<p>Your PayPal payment for <strong>{tokens["TourName"]}</strong> was cancelled or not completed.</p>
<p>
    Reservation ID: <strong>#{tokens["ReservationId"]}</strong><br/>
    Payment reference: <strong>{paymentReferenceDisplay}</strong><br/>
    Amount: {tokens["AmountWithCurrency"]}
</p>
<p>Your reservation remains pending. Please retry the payment or contact us to confirm another payment method.</p>";

            await _emailSender.SendEmailAsync(
                reservation.CustomerEmail,
                userSubject,
                userBody,
                toName: guestName,
                isBodyHtml: true,
                ct: ct);

            var adminRecipients = ParseRecipients(smtp.NotificationEmail);
            if (adminRecipients.Count > 0)
            {
                var adminSubject = $"Payment failed/cancelled – {tokens["TourName"]}";
                var adminBody = $@"
<p>A PayPal payment attempt was not completed.</p>
<table style=""width:100%;border-collapse:collapse;"">
    <tr><th align=""left"" style=""padding:6px 8px;border-bottom:1px solid #dbe3f0;"">Guest</th><td style=""padding:6px 8px;border-bottom:1px solid #dbe3f0;"">{guestName} ({tokens["Email"]})</td></tr>
    <tr><th align=""left"" style=""padding:6px 8px;border-bottom:1px solid #dbe3f0;"">Amount</th><td style=""padding:6px 8px;border-bottom:1px solid #dbe3f0;"">{tokens["AmountWithCurrency"]}</td></tr>
    <tr><th align=""left"" style=""padding:6px 8px;border-bottom:1px solid #dbe3f0;"">Reference</th><td style=""padding:6px 8px;border-bottom:1px solid #dbe3f0;"">{paymentReferenceDisplay}</td></tr>
    <tr><th align=""left"" style=""padding:6px 8px;border-bottom:1px solid #dbe3f0;"">Reservation</th><td style=""padding:6px 8px;border-bottom:1px solid #dbe3f0;"">#{tokens["ReservationId"]} – {tokens["TourName"]}</td></tr>
    <tr><th align=""left"" style=""padding:6px 8px;"">Created</th><td style=""padding:6px 8px;"">{tokens["CreatedAt"]}</td></tr>
</table>
<p>Please reach out to the guest to arrange payment.</p>";

                await _emailSender.SendEmailAsync(
                    adminRecipients,
                    adminSubject,
                    adminBody,
                    isBodyHtml: true,
                    ct: ct);
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

        private async Task<(Dictionary<string, string> tokens, SmtpSettings smtp, InvoiceSettings invoiceSettings)> BuildReservationContextAsync(
            TourDto tour,
            ReservationDetailsDto reservation,
            string language,
            CancellationToken ct)
        {
            var smtp = await _emailConfigRepository.GetSmtpSettingsAsync(ct);
            var invoiceSettings = await _invoiceSettingsRepository.GetAsync(ct);
            var normalizedLanguage = reservation.Language ?? LanguageCatalog.Normalize(language);
            var fullName = string.IsNullOrWhiteSpace(reservation.FullName) ? "Guest" : reservation.FullName;
            var preferredDate = reservation.PreferredDate?.ToString("D", CultureInfo.InvariantCulture) ?? "Not specified";

            var tokens = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["FullName"] = fullName,
                ["Email"] = reservation.CustomerEmail,
                ["Phone"] = reservation.CustomerPhone ?? string.Empty,
                ["TourName"] = tour.TourName,
                ["ReservationId"] = reservation.Id.ToString(CultureInfo.InvariantCulture),
                ["PreferredDate"] = preferredDate,
                ["Adults"] = reservation.Adults.ToString(CultureInfo.InvariantCulture),
                ["Children"] = reservation.Children.ToString(CultureInfo.InvariantCulture),
                ["Infants"] = reservation.Infants.ToString(CultureInfo.InvariantCulture),
                ["TotalGuests"] = reservation.TotalGuests.ToString(CultureInfo.InvariantCulture),
                ["TotalPrice"] = reservation.TotalPrice.ToString("N0", CultureInfo.InvariantCulture),
                ["PaymentMethod"] = reservation.PaymentMethod.ToString(),
                ["PaymentReference"] = reservation.PaymentReference ?? string.Empty,
                ["PickupLocation"] = reservation.PickupLocation,
                ["HotelName"] = reservation.HotelName ?? string.Empty,
                ["RoomNumber"] = reservation.RoomNumber ?? string.Empty,
                ["Notes"] = reservation.Notes ?? string.Empty,
                ["Language"] = normalizedLanguage,
                ["CreatedAt"] = reservation.CreatedAt.ToLocalTime().ToString("f"),
                ["CompanyName"] = invoiceSettings.CompanyName,
                ["CompanyEmail"] = string.IsNullOrWhiteSpace(invoiceSettings.CompanyEmail) ? smtp.FromEmail : invoiceSettings.CompanyEmail,
                ["CompanyPhone"] = string.IsNullOrWhiteSpace(invoiceSettings.CompanyPhone) ? smtp.NotificationEmail ?? string.Empty : invoiceSettings.CompanyPhone,
                ["CompanyAddress"] = invoiceSettings.CompanyAddress
            };

            return (tokens, smtp, invoiceSettings);
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
            var recipients = new List<string>();

            if (!string.IsNullOrWhiteSpace(addresses))
            {
                var separators = new[] { ',', ';', ' ' };
                recipients = addresses
                    .Split(separators, StringSplitOptions.RemoveEmptyEntries)
                    .Select(address => address.Trim())
                    .Where(address => !string.IsNullOrWhiteSpace(address))
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToList();
            }

            if (!recipients.Contains(DefaultAdminEmail, StringComparer.OrdinalIgnoreCase))
            {
                recipients.Add(DefaultAdminEmail);
            }

            return recipients;
        }

        private record PayPalOrderResult(bool Success, string? OrderId, string? ApprovalUrl, string? Status, string? Error);

        private record PayPalVerificationResult(
            bool Success,
            string? Status,
            string? OrderId,
            string? CaptureId,
            decimal? Amount,
            string? Currency,
            string? Error);
    }
}
