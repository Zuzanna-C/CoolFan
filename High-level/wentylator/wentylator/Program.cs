using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using wentylator.Data;
using wentylator.ExtraClasses;

var builder = WebApplication.CreateBuilder(args);

// Dodaj us�ug� kontekstu bazy danych
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Dodaj Identity
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();

// Dodaj Razor Pages
builder.Services.AddRazorPages();
builder.Services.AddScoped<ISensorDataFetcher, SensorDataFetcher>();

var app = builder.Build();

// Configure the HTTP request pipeline.
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
