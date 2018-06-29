using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Blue.Data.IdentityModel
{
    public class UserStore : UserStore<User, Role, BlueDbContext, Guid, UserClaim, UserRole, UserLogin, UserToken, RoleClaim>
    {
        public UserStore(BlueDbContext context, IdentityErrorDescriber describer) : base(context, describer)
        {
        }

  //      public override UserRole CreateUserRole(User user, Role role)
		//{
		//	return new UserRole
		//	{
		//		UserId = user.Id,
		//		RoleId = role.Id
		//	};
		//}

  //      public override UserClaim CreateUserClaim(User user, Claim claim)
		//{
		//	var userClaim = new UserClaim { UserId = user.Id };
		//	userClaim.InitializeFromClaim(claim);
		//	return userClaim;
		//}

  //      public override UserLogin CreateUserLogin(User user, UserLoginInfo login)
		//{
		//	return new UserLogin
		//	{
		//		UserId = user.Id,
		//		ProviderKey = login.ProviderKey,
		//		LoginProvider = login.LoginProvider,
		//		ProviderDisplayName = login.ProviderDisplayName
		//	};
		//}

  //      public override UserToken CreateUserToken(User user, string loginProvider, string name, string value)
		//{
		//	return new UserToken
		//	{
		//		UserId = user.Id,
		//		LoginProvider = loginProvider,
		//		Name = name,
		//		Value = value
		//	};
		//}
    }
}
