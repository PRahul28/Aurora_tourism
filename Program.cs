using FormApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
var builder = WebApplication.CreateBuilder(args);
builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultScheme =
            CookieAuthenticationDefaults.AuthenticationScheme;

        options.DefaultChallengeScheme =
            GoogleDefaults.AuthenticationScheme;
    })
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy =
            CookieSecurePolicy.Always;
    })
    .AddGoogle(options =>
    {
        options.ClientId =
            builder.Configuration["Authentication:Google:ClientId"]!;

        options.ClientSecret =
            builder.Configuration["Authentication:Google:ClientSecret"]!;
    });

builder.Services.AddAuthorization();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);
builder.Services.AddControllersWithViews();

var app = builder.Build();
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
