using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blue.Constract.Dtos.Role;
using Blue.Data.IdentityService;
using Blue.Data.Models.IdentityModel;
using Framework.Common.ModelBinders;
using Framework.Constract.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blue.Api.ApiControllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    //[Authorize]
    public class RoleCollectionsController : ControllerBase
    {
        private readonly ApplicationRoleManager _roleManager;
        private readonly IMapper _mapper;

        public RoleCollectionsController(ApplicationRoleManager roleManager, IMapper mapper)
        {
            _roleManager = roleManager;
            _mapper = mapper;
        }

        [HttpGet("{ids}", Name = "GetRoleCollection")]
        public async Task<ActionResult> GetRoleCollection([ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> ids)
        {
            if (ids == null)
            {
                return BadRequest();
            }

            var roleEntities = await _roleManager.Roles.Where(x => ids.Any(i => i == x.Id)).ToListAsync();

            if (ids.Count() != roleEntities.Count)
            {
                return NotFound();
            }

            var rolesToReturn = _mapper.MapTo<IEnumerable<RoleDto>>(roleEntities);

            return Ok(rolesToReturn);
        }

        [HttpPost]
        public async Task<ActionResult> CreateRoleCollection([FromBody] IEnumerable<RoleForCreationDto> roleCollection)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var roleEntities = _mapper.MapTo<IEnumerable<Role>>(roleCollection);
            foreach (var roleEntity in roleEntities)
            {
                await _roleManager.CreateAsync(roleEntity);
            }

            var roleCollectionToReturn = _mapper.MapTo<IEnumerable<RoleDto>>(roleEntities);
            var idsAsString = string.Join(",", roleCollectionToReturn.Select(x => x.Id));

            return AcceptedAtRoute("GetRoleCollection", new {ids = idsAsString}, roleCollectionToReturn);
        }
    }
}
