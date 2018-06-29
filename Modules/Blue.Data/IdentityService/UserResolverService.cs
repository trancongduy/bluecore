using Framework.Constract.Constant;
using Microsoft.AspNetCore.Http;

namespace Blue.Data.IdentityService
{
    public interface IUserResolverService
    {
        string GetUser();
    }

    public class UserResolverService : IUserResolverService
    {
        readonly IHttpContextAccessor _httpContextAccessor;

        public UserResolverService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetUser()
        {
            var userName = UserType.SystemGenerated;

            var identity = _httpContextAccessor?.HttpContext.User?.Identity;

            if (identity != null && identity.IsAuthenticated)
            {
                userName = identity.Name;
            }

            return userName;
        }
    }
}
