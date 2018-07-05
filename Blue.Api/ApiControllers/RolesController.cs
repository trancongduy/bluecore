using System.Collections.Generic;
using System.Threading.Tasks;
using Blue.Constract.Dtos;
using Blue.Data.IdentityService;
using Framework.Constract.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blue.Api.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RolesController : ControllerBase
    {
        private readonly ApplicationRoleManager _roleManager;
        private readonly IMapper _mapper;

        public RolesController(ApplicationRoleManager roleManager, IMapper mapper)
        {
            _roleManager = roleManager;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            if (roles == null)
            {
                return NotFound();
            }

            var result = _mapper.MapTo<IEnumerable<RoleDto>>(roles);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }

            var result = await _roleManager.DeleteAsync(role);

            if (result.Succeeded)
            {
                return NoContent();
            }

            return BadRequest();
        }
    }
}