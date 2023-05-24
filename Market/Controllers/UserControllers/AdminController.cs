using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Market.Data.Shared;
using Market.Data.Users;
using Market.Services.UserServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MarketAPI.Controllers.UserControllers
{
    [Route("[controller]")]
    public class AdminController : Controller
    {
        public IAdminService _adminService { get; set; }
        private IMapper _mapper;

        public AdminController(IAdminService adminService, IMapper mapper)
        {
            _adminService = adminService;
            _mapper = mapper;

        }
        // GET: /<controller>/
        [HttpGet("GetAll")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "OWNER")]
        public IActionResult GetAll()
        {
            var admins = _adminService.getAll();
            return Ok(admins);
        }

        [HttpGet("GetById")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "OWNER, ADMIN")]
        public IActionResult GetById(int id)
        {
            var admin = _adminService.getById(id);
            if (admin is null)
                return NotFound("admin Does not Exist");
            return Ok(admin);
        }

        

        [HttpPost("Create")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "OWNER")]

        public async Task<ActionResult<User>> Register(RegistrationDTO model)
        {
            try
            {
                return Ok(_adminService.create(model));

            }
            catch (NullReferenceException e)
            {
                return BadRequest("Values Cannot Be null");
            }
            catch (HttpRequestException h)
            {
                return Conflict(h.Message);
            }

        }

    }
}

