using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Resonance.App.Models;

namespace Resonance.App.Areas.Identity.Pages.Account;

public class ExternalLoginModel : PageModel
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;

    public ExternalLoginModel(
        SignInManager<ApplicationUser> signInManager,
        UserManager<ApplicationUser> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }

    [TempData]
    public string? ErrorMessage { get; set; }

    public string? ReturnUrl { get; set; }

    public async Task<IActionResult> OnGetAsync(
        string? returnUrl = null,
        string? remoteError = null)
    {
        ReturnUrl = returnUrl ?? Url.Content("~/");

        if (!string.IsNullOrEmpty(remoteError))
        {
            ErrorMessage = $"Error from external provider: {remoteError}";
            return RedirectToPage("./Login", new { ReturnUrl });
        }

        var info = await _signInManager.GetExternalLoginInfoAsync();
        if (info == null)
        {
            ErrorMessage = "Error loading external login information.";
            return RedirectToPage("./Login", new { ReturnUrl });
        }

        var signInResult = await _signInManager.ExternalLoginSignInAsync(
            info.LoginProvider,
            info.ProviderKey,
            isPersistent: false,
            bypassTwoFactor: true);

        if (signInResult.Succeeded)
        {
            return LocalRedirect(ReturnUrl);
        }

        var email = info.Principal.FindFirstValue(ClaimTypes.Email);
        if (string.IsNullOrWhiteSpace(email))
        {
            ErrorMessage = "Google account did not return an email address.";
            return RedirectToPage("./Login", new { ReturnUrl });
        }

        var user = new ApplicationUser
        {
            UserName = email,
            Email = email,
            DisplayName = info.Principal.FindFirstValue(ClaimTypes.Name) ?? email,
            CreatedAt = DateTime.UtcNow
        };

        var createResult = await _userManager.CreateAsync(user);
        if (!createResult.Succeeded)
        {
            ErrorMessage = string.Join(
                "; ",
                createResult.Errors.Select(e => e.Description));
            return RedirectToPage("./Login", new { ReturnUrl });
        }

        var addLoginResult = await _userManager.AddLoginAsync(user, info);
        if (!addLoginResult.Succeeded)
        {
            ErrorMessage = string.Join(
                "; ",
                addLoginResult.Errors.Select(e => e.Description));
            return RedirectToPage("./Login", new { ReturnUrl });
        }

        await _signInManager.SignInAsync(user, isPersistent: false);

        return LocalRedirect(ReturnUrl);
    }
}