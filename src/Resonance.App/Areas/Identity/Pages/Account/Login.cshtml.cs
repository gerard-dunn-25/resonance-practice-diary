using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Resonance.App.Models;

namespace Resonance.App.Areas.Identity.Pages.Account;

public class LoginModel : PageModel
{
    private readonly SignInManager<ApplicationUser> _signInManager;

    public LoginModel(SignInManager<ApplicationUser> signInManager)
    {
        _signInManager = signInManager;
    }

    [BindProperty(SupportsGet = true)]
    public string? ReturnUrl { get; set; }

    [BindProperty]
    public InputModel Input { get; set; } = new();

    public class InputModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        public bool RememberMe { get; set; }
    }

    public async Task OnGetAsync()
    {
        await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var result = await _signInManager.PasswordSignInAsync(
            Input.Email,
            Input.Password,
            Input.RememberMe,
            lockoutOnFailure: false);

        if (result.Succeeded)
        {
            return LocalRedirect(ReturnUrl ?? "/");
        }

        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
        return Page();
    }

    public IActionResult OnPostGoogleLogin()
    {
        var returnUrl = Url.IsLocalUrl(ReturnUrl) ? ReturnUrl : Url.Content("~/");

        var redirectUrl = Url.Page(
            "/Account/ExternalLogin",
            values: new { area = "Identity", returnUrl });

        var properties = _signInManager.ConfigureExternalAuthenticationProperties(
            "Google",
            redirectUrl);

        return new ChallengeResult("Google", properties);
    }
}