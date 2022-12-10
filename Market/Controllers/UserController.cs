using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Market.Data;
using Market.Data.Users;
using Market.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MarketAPI.Controllers
{
    [Route("[controller]")]
    public class UserController : Controller
    {
        private IUserService _userService;
        private IMapper _mapper;

        public UserController(
            IUserService userService, IMapper mapper)
      
        {
            _mapper = mapper;

            _userService = userService;
            
        }

       
        [HttpPost("SignIn")]
        
        public async Task<ActionResult<User>> signIn(AuthRequest model)
        {
            var response = _userService.signIn(model);

            if (response is null)
                return NotFound("Email or Password are Incorrect");
            
            return Ok(response);
        }

        [HttpPost("Register")]
        public async Task<ActionResult<User>> Register([FromBody] RegisterRequest model)
        {
            return Ok(_userService.register(model));
        }

        [HttpGet("GetAll")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "ADMIN")]
        public IActionResult GetAll()
        {
            var users = _userService.getAll();
            return Ok(users);
        }

        [HttpGet("GetById/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "CUSTOMER, ADMIN")]
        public IActionResult GetById(int id)
        {
            var user = _userService.getById(id);
            if (user is null)
                return NotFound("User Does not Exist");

            return Ok(user);
        }

        [HttpPut("Update/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "CUSTOMER,ADMIN")]
        public IActionResult Update(int id, UpdateRequest model)
        {
            try
            {
                
                _userService.update(id, model);
                return Ok(new { message = "User updated successfully" });

            }
            catch(HttpRequestException e)
            {
                return Conflict(e.Message);

            }  catch (NullReferenceException n)
            {
                return NotFound("User Does not Exist");
            }
        }

        [HttpDelete("Delete/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "ADMIN")]
        public IActionResult Delete(int id)
        {
            try
            {
                _userService.delete(id);
                return Ok(new { message = "User deleted successfully" });

            }
            catch (Exception e)
            {
                return NotFound("User does not Exist");
            }
        }
    }
}

