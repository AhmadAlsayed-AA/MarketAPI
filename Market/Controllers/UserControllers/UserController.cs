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
using Market.Services.UserServices;
using Market.Services.Helpers.Validation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MarketAPI.Controllers.UserControllers
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

        public async Task<ActionResult<UserResponse>> signIn([FromBody] AuthRequest request)
        {
            try
            {
                UserResponse response;


                return Ok(await _userService.signIn(request));
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { errors = ex.Errors });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
            }
        }

        [HttpPost("Register")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "OWNER")]
        public async Task<ActionResult<UserResponse>> Register([FromBody]RegisterRequest request)
        {
            try
            {
                var response = await _userService.register(request);
                return Ok(response);
            }
            catch (ValidationException ex) 
            {
                return BadRequest(new { errors = ex.Errors });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
            }
        }


        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = " OWNER, ADMIN")]
        public async Task<ActionResult<User[]>> GetAll()
        {
            try
            {
                var users = await _userService.getAll();
                return Ok(users);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
            }
        }

        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "OWNER, ADMIN")]
        public async Task<ActionResult> GetById(int id)
        {
            try
            {
                var user = await _userService.getById(id);
                if (user is null)
                    return NotFound("User Does not Exist");

                return Ok(user);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
            }
        }

        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = " OWNER")]
        public async Task<ActionResult<UserResponse>> Update(int id, [FromBody] UpdateRequest request)
        {
            try
            {
                var response = await _userService.update(id, request);
                return Ok(response);
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { errors = ex.Errors });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
            }
        }

        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "OWNER")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _userService.delete(id);
                return Ok("User Deleted");
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { errors = ex.Errors });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
            }
        }

        [HttpPatch("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "OWNER, ADMIN")]
        public async Task<ActionResult> ChangeIsActive(int id, bool isActive)
        {
            try
            {
                await _userService.changeIsActive(id, isActive);
                return Ok(new { message = "IsActive updated successfully" });
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { errors = ex.Errors });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
            }
        }
    }
}

