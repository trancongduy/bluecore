using System.Threading.Tasks;

namespace Blue.IdentityServer.Services
{
    public interface ILoginService<T>
    {
        Task<bool> ValidateCredentials(T user, string password);
        Task<T> FindByUsername(string user);
        Task SignIn(T user);
    }
}
