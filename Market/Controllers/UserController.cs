using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Market.Data;
using Market.Data.HelperModels;
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
        private ITokenService _tokenService;
        private IMapper _mapper;

        public UserController(
            IUserService userService, IMapper mapper, ITokenService tokenService)
      
        {
            _mapper = mapper;

            _userService = userService;
            _tokenService = tokenService;


        }

       
        [HttpPost("SignIn")]
        
        public IActionResult signIn([FromBody]AuthRequest model)
        {
            var user = _userService.signIn(model);

            if (user == null)
            {
                return Unauthorized(new ErrorResponse
                {
                    ErrorCode = "Unauthorized",
                    Message = "Invalid username or password."
                });
            }

            var token = _tokenService.GenerateToken(user);

            return Ok(new AuthResponse
            {
                User = user,
                Token = token
            });
        }

        [HttpPost("Register")]
        public async Task<ActionResult<User>> Register(RegisterRequest model)
        {
            try {
                return Ok(_userService.register(model));

            } catch (NullReferenceException e)
            {
                return BadRequest("Values Cannot Be null");
            }
            catch (HttpRequestException h)
            {
                return Conflict(h.Message);
            }
            
        }

        [HttpGet("GetAll")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "ADMIN")]
        public IActionResult GetAll()
        {
            var users = _userService.getAll();
            return Ok(users);
        }

        [HttpGet("GetById")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "CUSTOMER, ADMIN")]
        public IActionResult GetById(int id)
        {
            var user = _userService.getById(id);
            if (user is null)
                return NotFound("User Does not Exist");

            return Ok(user);
        }

        [HttpPut("Update")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "CUSTOMER,ADMIN")]
        public async Task<ActionResult<User>> Update(int id, UpdateRequest model)
        {
            try
            {
                var user = _userService.getById(id);
                if (user is null)
                    return NotFound("User Does not Exist");

                
                return Ok(_userService.update(id, model));

            }
            catch(HttpRequestException e)
            {
                return Conflict(e.Message);

            }  catch (NullReferenceException n)
            {
                return NotFound("User Does not Exist");
            }
        }

        [HttpDelete("Delete")]
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

