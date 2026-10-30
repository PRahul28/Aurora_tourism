
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FormApp.Controllers;

public class AccountController : Controller
{
    [AllowAnonymous]
    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        ViewData["ReturnUrl"] =
            Url.IsLocalUrl(returnUrl) ? returnUrl : "/Rooms";

        return View();
    }

    [AllowAnonymous]
    [HttpGet]
    public IActionResult GoogleLogin(string? returnUrl = null)
    {
        var destination =
            Url.IsLocalUrl(returnUrl) ? returnUrl! : "/Rooms";

        var properties = new AuthenticationProperties
        {
            RedirectUri = destination
        };

        return Challenge(
            properties,
            GoogleDefaults.AuthenticationScheme
        );
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(
            CookieAuthenticationDefaults.AuthenticationScheme
        );

        return RedirectToAction(nameof(Login));
    }
}
