using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Market.Data.Addresses;
using Market.Data.HelperModels;
using Market.Data.Stores;
using Market.Data.Users;
using Market.Services;
using Market.Services.Helpers.FileUpload;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MarketAPI.Controllers
{
    [Route("[controller]")]
    public class StoreController : Controller
    {
        // GET: /<controller>/
        private IStoreService _storeService;
        private IMapper _mapper;

        public StoreController(
            IStoreService storeService, IMapper mapper)

        {
            _mapper = mapper;

            _storeService = storeService;

        }

        [HttpPost("Create")]
        public async Task<ActionResult> CreateStore( StoreRequest storeRequest)
        {
            try
            {
                _storeService.create(storeRequest);
                return Ok("Store Created Successfully");

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
        [HttpGet("GetAll")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "ADMIN")]
        public IActionResult GetAll()
        {
            var users = _storeService.getAll();
            return Ok(users);
        }

        [HttpGet("GetById")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "STORE, ADMIN")]
        public IActionResult GetById(int id)
        {
            var user = _storeService.getById(id);
            if (user is null)
                return NotFound("Store Does not Exist");
            return Ok(user);
        }

        [HttpPut("Update")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "STORE, ADMIN")]
        public IActionResult Update(int id, StoreUpdateRequest model)
        {
            try
            {

                
                return Ok(_storeService.update(id, model));

            }
            catch (HttpRequestException e)
            {
                return Conflict(e.Message);

            }
            catch (NullReferenceException n)
            {
                return NotFound("Store Does not Exist");
            }
        }

        [HttpPut("ChangeIsActive")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "STORE, ADMIN")]
        public IActionResult ChangeIsActive(int id, bool isActive)
        {
            try
            {

                _storeService.changeIsActive(id, isActive);
                return Ok(new { message = "Store updated successfully" });

            }
            catch (HttpRequestException e)
            {
                return Conflict(e.Message);

            }
            catch (NullReferenceException n)
            {
                return NotFound("Store Does not Exist");
            }
        }

        [HttpDelete("Delete")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "ADMIN")]
        public IActionResult  Delete(int id)
        {
            try
            {
                _storeService.delete(id);
                return Ok(new { message = "Store deleted successfully" });

            }
            catch (Exception e)
            {
                return NotFound("Store does not Exist");
            }
        }

    }
}

