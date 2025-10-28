using Microsoft.EntityFrameworkCore;
using Models.DbContexts;
using Models.Helper;
using Models.Interface;
using Models.Repository;
using Models.Services;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<TourDbContext>(options =>
{
    var connectionKey = builder.Environment.IsDevelopment()
        ? "SinaLocalhost"    // local postgres (sina localhost)
        : "SinaRailway";     // production railway (sina railway)
    var cs = builder.Configuration.GetConnectionString(connectionKey);
    if (string.IsNullOrWhiteSpace(cs))
    {
        throw new InvalidOperationException($"Connection string '{connectionKey}' was not found.");
    }
    options.UseNpgsql(cs);
});
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
builder.Services.AddScoped<IEmailSender, SmtpEmailSender>();
builder.Services.AddControllersWithViews();
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<TourDbContext>();
    db.Database.Migrate("test13");        // جداول را می‌سازد/آپدیت می‌کند
    // (اختیاری) اگر Seed داده نیاز داری، اینجا انجام بده
}

app.UseDeveloperExceptionPage();
app.UseStaticFiles();
app.UseStatusCodePages();
app.UseHttpsRedirection();

app.UseRouting();         // ترتیب مهم است
//app.UseAuthorization();   // اگر احراز هویت/مجوز دارید، اینجا می‌آید

// دیگر UseEndpoints لازم نیست:
app.MapDefaultControllerRoute();


app.Run();
