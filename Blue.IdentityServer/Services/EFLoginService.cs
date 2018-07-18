using System.Threading.Tasks;
using Blue.Data.Models.IdentityModel;
using Microsoft.AspNetCore.Identity;

namespace Blue.IdentityServer.Services
{
    public class EfLoginService : ILoginService<User>
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public EfLoginService(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<User> FindByUsername(string user)
        {
            return await _userManager.FindByNameAsync(user);
        }

        public async Task<bool> ValidateCredentials(User user, string password)
        {
            return await _userManager.CheckPasswordAsync(user, password);
        }

        public Task SignIn(User user)
        {
            return _signInManager.SignInAsync(user, true);
        }
    }
}
