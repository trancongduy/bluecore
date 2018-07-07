using System.Linq;
using Blue.Api.Attributes;
using Framework.Constract.Constant;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Blue.Api.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = Policies.SuperAdmin)]
    public class IdentityController : ControllerBase
    {
        [HttpGet]
        [TypeFilter(typeof(AuditAttribute), Arguments = new object[] { 2 })]
        public IActionResult Get()
        {
            return Ok(User.Claims.Select(c => new { c.Type, c.Value }));
        }
    }
}
