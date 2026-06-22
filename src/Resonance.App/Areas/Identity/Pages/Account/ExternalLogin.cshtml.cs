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

    public async Task<IActionResult> OnGetAsync(
        string? returnUrl = null,
        string? remoteError = null)
    {
        var safeReturnUrl = Url.IsLocalUrl(returnUrl)
            ? returnUrl
            : Url.Content("~/");

        if (!string.IsNullOrWhiteSpace(remoteError))
        {
            ErrorMessage = $"Error from external provider: {remoteError}";
            return RedirectToPage("./Login", new { returnUrl = safeReturnUrl });
        }

        var info = await _signInManager.GetExternalLoginInfoAsync();
        if (info == null)
        {
            ErrorMessage = "Error loading external login information.";
            return RedirectToPage("./Login", new { returnUrl = safeReturnUrl });
        }

        await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

        var signInResult = await _signInManager.ExternalLoginSignInAsync(
            info.LoginProvider,
            info.ProviderKey,
            isPersistent: false,
            bypassTwoFactor: true);

        if (signInResult.Succeeded)
        {
            return LocalRedirect(safeReturnUrl!);
        }

        if (signInResult.IsLockedOut)
        {
            return RedirectToPage("./Lockout");
        }

        if (signInResult.IsNotAllowed)
        {
            ErrorMessage = "This account is not allowed to sign in.";
            return RedirectToPage("./Login", new { returnUrl = safeReturnUrl });
        }

        var email = info.Principal.FindFirstValue(ClaimTypes.Email);
        if (string.IsNullOrWhiteSpace(email))
        {
            ErrorMessage = "Google account did not return an email address.";
            return RedirectToPage("./Login", new { returnUrl = safeReturnUrl });
        }

        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            user = new ApplicationUser
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
                return RedirectToPage("./Login", new { returnUrl = safeReturnUrl });
            }
        }

        var addLoginResult = await _userManager.AddLoginAsync(user, info);
        if (!addLoginResult.Succeeded)
        {
            ErrorMessage = string.Join(
                "; ",
                addLoginResult.Errors.Select(e => e.Description));
            return RedirectToPage("./Login", new { returnUrl = safeReturnUrl });
        }

        await _signInManager.SignInAsync(user, isPersistent: false);
        return LocalRedirect(safeReturnUrl!);
    }
}