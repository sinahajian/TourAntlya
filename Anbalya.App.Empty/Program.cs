using System;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Models.DbContexts;
using Models.Helper;
using Models.Interface;
using Models.Repository;
using Models.Services;


var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("SinaRailway");

builder.Services.AddDbContext<TourDbContext>(options =>
    options.UseNpgsql(connectionString));
Console.WriteLine(">>> CONNECTION: " + connectionString);
builder.Services.AddScoped<ITourRepository, TourRepository>();
builder.Services.AddScoped<IManagerRepository, ManagerRepository>();
builder.Services.AddScoped<IManagerTourRepository, ManagerTourRepository>();
builder.Services.AddScoped<ILandingContentRepository, LandingContentRepository>();
builder.Services.AddScoped<IReservationRepository, ReservationRepository>();
builder.Services.AddScoped<IPaymentOptionRepository, PaymentOptionRepository>();
builder.Services.AddScoped<IPayPalSettingsRepository, PayPalSettingsRepository>();
builder.Services.AddScoped<ILanguageResolver, LanguageResolver>();
builder.Services.AddScoped<IRoyalFacilityRepository, RoyalFacilityRepository>();
builder.Services.AddScoped<IAboutContentRepository, AboutContentRepository>();
builder.Services.AddScoped<IContactMessageRepository, ContactMessageRepository>();
builder.Services.AddScoped<IEmailConfigurationRepository, EmailConfigurationRepository>();
builder.Services.AddScoped<IInvoiceSettingsRepository, InvoiceSettingsRepository>();
builder.Services.AddScoped<IEmailSender, SmtpEmailSender>();
builder.Services.AddSingleton<IPayPalHelper, PayPalHelper>();
builder.Services.AddTransient<AppDataSeeder>();
builder.Services.AddTransient<IInvoiceDocumentService, InvoiceDocumentService>();
builder.Services.AddMemoryCache();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.Cookie.Name = ".TourAntalya.ManagerSession";
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.IdleTimeout = TimeSpan.FromHours(8);
});
builder.Services.AddControllersWithViews();
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
var app = builder.Build();

var canonicalBaseUrl = builder.Configuration["Site:BaseUrl"];
if (!app.Environment.IsDevelopment() && !string.IsNullOrWhiteSpace(canonicalBaseUrl))
{
    if (Uri.TryCreate(canonicalBaseUrl, UriKind.Absolute, out var canonicalUri))
    {
        app.Use(async (context, next) =>
        {
            if (!string.Equals(context.Request.Host.Host, canonicalUri.Host, StringComparison.OrdinalIgnoreCase))
            {
                var targetUrl = $"{canonicalUri.Scheme}://{canonicalUri.Host}{context.Request.PathBase}{context.Request.Path}{context.Request.QueryString}";
                context.Response.StatusCode = StatusCodes.Status301MovedPermanently;
                context.Response.Headers.Location = targetUrl;
                return;
            }

            await next();
        });
    }
}
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<TourDbContext>();
    db.Database.Migrate("UpdatePayPalLive");        // جداول را می‌سازد/آپدیت می‌کند

    var seeder = scope.ServiceProvider.GetRequiredService<AppDataSeeder>();
    await seeder.SeedAsync();
}

app.UseDeveloperExceptionPage();
app.UseStaticFiles();
app.UseStatusCodePages();
app.UseHttpsRedirection();

app.UseRouting();         // ترتیب مهم است
app.UseSession();
//app.UseAuthorization();   // اگر احراز هویت/مجوز دارید، اینجا می‌آید

// دیگر UseEndpoints لازم نیست:
app.MapControllerRoute(
    name: "robots",
    pattern: "robots.txt",
    defaults: new { controller = "Seo", action = "Robots" });

app.MapControllerRoute(
    name: "sitemap",
    pattern: "sitemap.xml",
    defaults: new { controller = "Seo", action = "Sitemap" });

app.MapDefaultControllerRoute();


app.Run();
