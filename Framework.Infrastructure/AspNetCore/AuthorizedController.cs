using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Framework.Infrastructure.AspNetCore
{
    [Authorize]
    public class AuthorizedApiController : ControllerBase
    {
    }
}
