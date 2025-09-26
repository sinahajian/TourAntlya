using Microsoft.EntityFrameworkCore;
using Models.DbContexts;
using Models.Helper;
using Models.Interface;
using Models.Repository;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<TourDbContext>(options =>
{
    var cs = builder.Configuration.GetConnectionString("Default");
    options.UseNpgsql(cs);
}

);
builder.Services.AddScoped<ITourRepository, TourRepository>();
builder.Services.AddScoped<ILanguageResolver, LanguageResolver>();
builder.Services.AddControllersWithViews();
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
var app = builder.Build();
app.UseDeveloperExceptionPage();
app.UseStaticFiles();
app.UseStatusCodePages();
app.UseHttpsRedirection();

app.UseRouting();         // ترتیب مهم است
//app.UseAuthorization();   // اگر احراز هویت/مجوز دارید، اینجا می‌آید

// دیگر UseEndpoints لازم نیست:
app.MapDefaultControllerRoute();


app.Run();
