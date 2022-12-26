using System;
using AutoMapper;
using Market.Data.Couriers;
using Market.Data.Stores;
using Market.Data.Users;
using Market.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace MarketAPI.Controllers
{
    [Route("[controller]")]
    public class CourierController : Controller
    {
        private ICourierService _courierService;
        private IMapper _mapper;

        
        public CourierController(ICourierService courierService, IMapper mapper)
		{
            _mapper = mapper;

            _courierService = courierService;
        }

        [HttpPost("Create")]
        public async Task<ActionResult> CreateCourier(CourierRequest request)
        {
            try
            {
                _courierService.create(request);
                return Ok("Courier Created Successfully");

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
        [HttpPut("Update")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "COURIER, ADMIN")]
        public IActionResult Update([Required] int id, CourierUpdateRequest model)
        {
            try
            {
                return Ok(_courierService.update(id, model));

            }
            catch (HttpRequestException e)
            {
                return Conflict(e.Message);

            }
            catch (NullReferenceException n)
            {
                return NotFound("Courier Does not Exist");
            }
        }
        [HttpGet("GetAll")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "ADMIN")]
        public IActionResult GetAll()
        {
            var users = _courierService.getAll();
            return Ok(users);
        }

        [HttpGet("GetById")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "COURIER, ADMIN")]
        public IActionResult GetById(int id)
        {
            var user = _courierService.getById(id);
            if (user is null)
                return NotFound("Courier Does not Exist");
            return Ok(user);
        }

        [HttpDelete("Delete")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "ADMIN")]
        public IActionResult Delete(int id)
        {
            try
            {
                _courierService.delete(id);
                return Ok(new { message = "Courier deleted successfully" });

            }
            catch (Exception e)
            {
                return NotFound("Courier does not Exist");
            }
        }
    }
}

