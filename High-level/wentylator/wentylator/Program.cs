using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using wentylator.Data;
using CoolFan.Models;
using CoolFan.HelpClasses;
using CoolFan.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Konfiguracja kontekstu bazy danych
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Usuwamy Identity i konfigurujemy us³ugi Razor Pages
// builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
//     .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddRazorPages();
builder.Services.AddTransient<ISensorDataFetcher, SensorDataFetcher>();
builder.Services.AddTransient<IFanControlService, FanControlService>();

// Konfiguracja uwierzytelniania z u¿yciem ciasteczek
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login";
    });

var app = builder.Build();

// Konfiguracja aplikacji dla trybu produkcyjnego
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();

app.Run();
