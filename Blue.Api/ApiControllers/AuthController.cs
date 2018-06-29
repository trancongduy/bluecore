using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Blue.Constract.ViewModels.Account;
using Blue.Data.IdentityModel;
using Blue.Data.IdentityService;
using Framework.Common.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Blue.Api.ApiControllers
{
    [Route("auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationUserManager _userManager;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly TokenProviderOptions _options;
        private readonly IConfigurationRoot _configurationRoot;

        public AuthController(ApplicationUserManager userManager, 
            IPasswordHasher<User> passwordHasher, 
            IConfigurationRoot configurationRoot, 
            IOptions<TokenProviderOptions> options)
        {
            _userManager = userManager;
            _passwordHasher = passwordHasher;
            _configurationRoot = configurationRoot;
            _options = options.Value;
        }

        [HttpPost("CreateToken")]
        [Route("token")]
        public async Task<IActionResult> CreateToken(LoginInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var user = await _userManager.FindByNameAsync(model.Username);
            if (user == null || _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, model.Password) != PasswordVerificationResult.Success)
            {
                return Unauthorized();
            }

            var userClaims = await _userManager.GetClaimsAsync(user);

            var jwtSecurityToken = await GetJwtSecurityToken(user);
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

            return Ok(new
            {
                access_token = encodedJwt,
                expires_in = _options.Expiration.TotalSeconds,
                token_type = "Bearer"
            });
        }

        private async Task<JwtSecurityToken> GetJwtSecurityToken(User user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var decodedBytes = Encoding.UTF8.GetBytes(_configurationRoot["JWTSettings:SecretKey"]);
            var base64secret = Convert.ToBase64String(decodedBytes);
            var encodedBytes = Convert.FromBase64String(base64secret);

            var now = DateTime.UtcNow;
            var symmetricSecurityKey = new SymmetricSecurityKey(encodedBytes);
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            return new JwtSecurityToken(
                issuer: _configurationRoot["JWTSettings:ClientId"],
                audience: _configurationRoot["JWTSettings:Audience"],
                claims: GetTokenClaims(user).Union(userClaims),
                notBefore: now,
                expires: now.Add(_options.Expiration),
                signingCredentials: signingCredentials
            );
        }

        private static IEnumerable<Claim> GetTokenClaims(User user)
        {
            return new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
                new Claim(JwtRegisteredClaimNames.Email, user.Email)
            };
        }
    }
}
