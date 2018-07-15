using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blue.Constract.Dtos;
using Blue.Constract.Dtos.Role;
using Blue.Data.IdentityService;
using Blue.Data.Models.IdentityModel;
using Framework.Constract.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blue.Api.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
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
        public async Task<ActionResult> GetRoles()
        {
            var result = await _roleManager.Roles.Where(r => !r.IsDeleted).ToListAsync();
            if (result == null)
            {
                return NotFound();
            }

            var role = _mapper.MapTo<IEnumerable<RoleDto>>(result);

            return Ok(role);
        }

        [HttpGet("{id}", Name = "GetRole")]
        public async Task<ActionResult> GetRole(string id)
        {
            var result = await _roleManager.FindByIdAsync(id);
            if (result == null)
            {
                return NotFound();
            }

            var role = _mapper.MapTo<RoleDto>(result);

            return Ok(role);
        }

        // POST: api/Roles
        [HttpPost]
        public async Task<ActionResult> CreateRole([FromBody]RoleForCreationDto role)
        {
            if (!ModelState.IsValid) 
            {
                return BadRequest();
            }

            var roleEntity = _mapper.MapTo<Role>(role);
            var result = await _roleManager.CreateAsync(roleEntity);

            if (!result.Succeeded)
            {
               throw new Exception("Creating a role failed on save.");
            }

            var roleToReturn = _mapper.MapTo<RoleDto>(roleEntity);
            return CreatedAtRoute("GetRole", new { id = roleToReturn.Id }, roleToReturn);
        }

        // PUT: api/Roles/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateRole(string id, [FromBody]RoleForUpdateDto role)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var roleExists = await _roleManager.FindByIdAsync(id);
            if (roleExists == null)
            {
                return NotFound();
            }

            var roleEntity = _mapper.MapToInstance(role, roleExists);
            var result = await _roleManager.UpdateAsync(roleEntity);

            if (!result.Succeeded)
            {
                throw new Exception($"Update role {id} failed on save.");
            }

            return NoContent();
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

            if (!result.Succeeded)
            {
                throw new Exception($"Deleting role {id} failed on save.");
            }

            return NoContent();
        }
    }
}