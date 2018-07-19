using System;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Blue.Data.IdentityService;
using Blue.Data.Models.IdentityModel;
using Blue.IdentityServer.Attributes;
using Blue.IdentityServer.Infrastructure.Services;
using Blue.IdentityServer.Models.AccountViewModel;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace Blue.IdentityServer.Controllers
{
    [SecurityHeaders]
    public class AccountController : Controller
    {
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IClientStore _clientStore;
        private readonly ILoginService<User> _loginService;

        public AccountController(IIdentityServerInteractionService interaction, IClientStore clientStore,
            ApplicationUserManager userManager, ILoginService<User> loginService)
        {
            _interaction = interaction;
            _clientStore = clientStore;
            _loginService = loginService;
        }

        /// <summary>
        /// Show login page
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Login(string returnUrl)
        {
            var context = await _interaction.GetAuthorizationContextAsync(returnUrl);
            if (context?.IdP != null)
            {
                // if IdP is passed, then bypass showing the login screen
                return ExternalLogin(context.IdP, returnUrl);
            }

            var vm = await BuildLoginViewModelAsync(returnUrl, context);

            ViewData["ReturnUrl"] = returnUrl;

            return View(vm);
        }

        /// <summary>
        /// Handle postback from username/password login
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _loginService.FindByUsername(model.UserName);
                if (await _loginService.ValidateCredentials(user, model.Password))
                {
                    AuthenticationProperties props = null;
                    if (model.RememberMe)
                    {
                        props = new AuthenticationProperties
                        {
                            IsPersistent = true,
                            ExpiresUtc = DateTimeOffset.UtcNow.AddYears(10)
                        };
                    };

                    await _loginService.SignIn(user);

                    // make sure the returnUrl is still valid, and if yes - redirect back to authorize endpoint
                    if (_interaction.IsValidReturnUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }

                    return Redirect("~/");
                }

                ModelState.AddModelError("", "Invalid username or password.");
            }

            // something went wrong, show form with error
            var vm = await BuildLoginViewModelAsync(model);

            ViewData["ReturnUrl"] = model.ReturnUrl;

            return View(vm);
        }

        private async Task<LoginViewModel> BuildLoginViewModelAsync(string returnUrl, AuthorizationRequest context)
        {
            var allowLocal = true;
            if (context?.ClientId != null)
            {
                var client = await _clientStore.FindEnabledClientByIdAsync(context.ClientId);
                if (client != null)
                {
                    allowLocal = client.EnableLocalLogin;
                }
            }

            return new LoginViewModel
            {
                ReturnUrl = returnUrl,
                UserName = context?.LoginHint,
            };
        }

        async Task<LoginViewModel> BuildLoginViewModelAsync(LoginViewModel model)
        {
            var context = await _interaction.GetAuthorizationContextAsync(model.ReturnUrl);
            var vm = await BuildLoginViewModelAsync(model.ReturnUrl, context);
            vm.UserName = model.UserName;
            vm.RememberMe = model.RememberMe;
            return vm;
        }

        /// <summary>
        /// initiate roundtrip to external authentication provider
        /// </summary>
        [HttpGet]
        public IActionResult ExternalLogin(string provider, string returnUrl)
        {
            if (returnUrl != null)
            {
                returnUrl = UrlEncoder.Default.Encode(returnUrl);
            }
            returnUrl = "/account/externallogincallback?returnUrl=" + returnUrl;

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