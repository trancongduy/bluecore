﻿using System.Collections.Generic;
using Blue.Constract.Dtos;
using Blue.DomainService;
using Framework.Constract.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Blue.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ContactController : ControllerBase
    {
        private readonly IContactService _contactService;
        private readonly IMapper _mapper;

        public ContactController(IContactService contactService, IMapper mapper)
        {
            _contactService = contactService;
            _mapper = mapper;
        }

        // GET: api/values
        [HttpGet]
        //public IEnumerable<string> Get()
        public IActionResult Get()
        {
            var contact = _mapper.MapTo<IEnumerable<ContactDto>>(_contactService.GetAll());
            return Ok(contact);
        }

    }
}
