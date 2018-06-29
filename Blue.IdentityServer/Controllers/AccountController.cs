using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Framework.Common.ActionFilters;
using Blue.DomainService;
using Blue.Constract.ViewModels.Account;
using Blue.Data.IdentityService;
using IdentityServer4;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using Microsoft.AspNetCore.Authentication;
using Blue.IdentityServer.Attributes;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Blue.IdentityServer.Controllers
{
    [SecurityHeaders]
    public class AccountController : Controller
    {
        private readonly ApplicationUserManager _userManager;
        private readonly ApplicationSignInManager _signInManager;
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IClientStore _clientStore;
        private readonly ILogger _logger;
        private readonly AccountService _accountService;

        public AccountController(
            ApplicationUserManager userManager,
            ApplicationSignInManager signInManager,
            ILoggerFactory loggerFactory,
            IIdentityServerInteractionService interaction,
            IHttpContextAccessor httpContext,
            IClientStore clientStore)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = loggerFactory.CreateLogger<AccountController>();
            _interaction = interaction;
            _clientStore = clientStore;

            _accountService = new AccountService(interaction, httpContext, clientStore);
        }

        /// <summary>
        /// Show login page
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Login(string returnUrl)
        {
            var vm = await _accountService.BuildLoginViewModelAsync(returnUrl);

            if (vm.IsExternalLoginOnly)
            {
                // only one option for logging in
                return await ExternalLogin(vm.ExternalLoginScheme, returnUrl);
            }

            return View(vm);
        }

        /// <summary>
        /// Handle postback from username/password login
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginInputModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberLogin, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    AuthenticationProperties props = null;
                    // only set explicit expiration here if persistent. 
                    // otherwise we reply upon expiration configured in cookie middleware.
                    if (AccountOptions.AllowRememberLogin && model.RememberLogin)
                    {
                        props = new AuthenticationProperties
                        {
                            IsPersistent = true,
                            ExpiresUtc = DateTimeOffset.UtcNow.Add(AccountOptions.RememberMeLoginDuration)
                        };
                    };

                    // issue authentication cookie with subject ID and username
                    var user = await _userManager.FindByNameAsync(model.Username);
                    await HttpContext.SignInAsync(user.Id.ToString(), user.UserName, props);

                    // make sure the returnUrl is still valid, and if yes - redirect back to authorize endpoint
                    if (_interaction.IsValidReturnUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }

                    return Redirect("~/");
                }

                ModelState.AddModelError("", AccountOptions.InvalidCredentialsErrorMessage);
            }

            // something went wrong, show form with error
            var vm = await _accountService.BuildLoginViewModelAsync(model);
            return View(vm);
        }

        //
        // POST: api/Account/Login
        [HttpPost]
        [AllowAnonymous]
        [Route("Login")]
        [ValidationFilter]
        public async Task<IActionResult> ApiLogin(LoginInputModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Username or password is unvalid!");
                return NotFound("Username or password is unvalid!");
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, set lockoutOnFailure: true
            var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberLogin, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                _logger.LogInformation(1, "User logged in.");
                return Ok("LoggedIn");
            }

            if (result.RequiresTwoFactor)
            {
                return RedirectToAction("SendCode", new { ReturnUrl = model.ReturnUrl, RememberMe = model.RememberLogin });
            }

            if (result.IsLockedOut)
            {
                _logger.LogWarning(2, "User account locked out.");
                return Ok("Lockout");
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return Unauthorized();
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("Logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(IdentityServerConstants.DefaultCookieAuthenticationScheme);

            // delete authentication cookie
            await _signInManager.SignOutAsync();

            return Ok("LoggedOut");
        }

        //
        // POST: api/Account/Logout
        [HttpPost]
        [AllowAnonymous]
        [Route("Logout")]
        public async Task<IActionResult> Logout(LogoutViewModel model)
        {
            var vm = await _accountService.BuildLoggedOutViewModelAsync(model.LogoutId);
            if (vm.TriggerExternalSignout)
            {
                string url = Url.Action("Logout", new { logoutId = vm.LogoutId });
                try
                {
                    // hack: try/catch to handle social providers that throw
                    await HttpContext.SignOutAsync(vm.ExternalAuthenticationScheme,
                        new AuthenticationProperties { RedirectUri = url });
                }
                catch (NotSupportedException) // this is for the external providers that don't have signout
                {
                }
                catch (InvalidOperationException) // this is for Windows/Negotiate
                {
                }
            }

            // delete authentication cookie
            await _signInManager.SignOutAsync();

            return Ok("LoggedOut");
        }

        /// <summary>
        /// initiate roundtrip to external authentication provider
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> ExternalLogin(string provider, string returnUrl)
        {
            returnUrl = Url.Action("ExternalLoginCallback", new { returnUrl = returnUrl });

            // windows authentication is modeled as external in the asp.net core authentication manager, so we need special handling
            if (AccountOptions.WindowsAuthenticationSchemes.Contains(provider))
            {
                // but they don't support the redirect uri, so this URL is re-triggered when we call challenge
                if (HttpContext.User is WindowsPrincipal)
                {
                    var props = new AuthenticationProperties();
                    props.Items.Add("scheme", AccountOptions.WindowsAuthenticationProviderName);

                    var id = new ClaimsIdentity(provider);
                    id.AddClaim(new Claim(ClaimTypes.NameIdentifier, HttpContext.User.Identity.Name));
                    id.AddClaim(new Claim(ClaimTypes.Name, HttpContext.User.Identity.Name));

                    await HttpContext.SignInAsync(IdentityServerConstants.ExternalCookieAuthenticationScheme, new ClaimsPrincipal(id), props);
                    return Redirect(returnUrl);
                }
                else
                {
                    // this triggers all of the windows auth schemes we're supporting so the browser can use what it supports
                    return new ChallengeResult(AccountOptions.WindowsAuthenticationSchemes);
                }
            }
            else
            {
                // start challenge and roundtrip the return URL
                var props = new AuthenticationProperties
                {
                    RedirectUri = returnUrl,
                    Items = { { "scheme", provider } }
                };
                return new ChallengeResult(provider, props);
            }
        }
    }
}
