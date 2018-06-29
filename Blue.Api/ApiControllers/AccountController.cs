using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blue.Constract.ViewModels.Account;
using Blue.Data.IdentityModel;
using Blue.Data.IdentityService;
using Framework.Infrastructure.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Blue.Api.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : AuthorizedApiController
    {
        private readonly ApplicationUserManager _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AccountController(ApplicationUserManager userManager, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        //
        // POST: api/Account/Register
        [HttpPost]
        [AllowAnonymous]
        [Route("Register")]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest();
            
            var user = new User
            {
                UserName = model.UserName,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                return Accepted();
            }

            return BadRequest();
        }

        [HttpPost]
        [Route("AssignCurrentUserToRoles")]
        public async Task<ActionResult> AssignCurrentUserToRoles(IEnumerable<string> roles)
        {
            var roleEnumerable = roles as IList<string> ?? roles.ToList();
            if (!ModelState.IsValid || !roleEnumerable.Any())
            {
                return BadRequest();
            }

            var currentUser = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);

            if (currentUser == null) return BadRequest();

            var result = await _userManager.AddToRolesAsync(currentUser, roleEnumerable);

            if (result.Succeeded)
            {
                return Accepted();
            }

            return BadRequest();
        }

        [HttpPost]
        public async Task<ActionResult> AssignUserToRoles(int userId, IEnumerable<string> roles)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user == null) return BadRequest();

            var result = await _userManager.AddToRolesAsync(user, roles);

            if (result.Succeeded)
            {
                return Accepted();
            }

            return BadRequest();
        }
    }
}
