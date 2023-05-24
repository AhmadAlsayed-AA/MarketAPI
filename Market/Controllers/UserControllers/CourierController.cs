using System;
using AutoMapper;
using Market.Data.Couriers;
using Market.Data.Stores;
using Market.Data.Users;
using Market.Services.UserServices;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;
using Market.Services.Helpers.Validation;

using System.Threading.Tasks;
using ValidationException = Market.Services.Helpers.Validation.ValidationException;

namespace MarketAPI.Controllers.UserControllers
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
        public async Task<ActionResult> CreateCourier([FromBody] CourierRequest request)
        {
            try
            {
                await _courierService.Create(request);
                return Ok("Courier Created Successfully");
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { errors = ex.Errors });
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
        public async Task<IActionResult> Update([Required] int id, CourierUpdateRequest model)
        {
            try
            {
                var updatedCourier = await _courierService.Update(id, model);
                return Ok(updatedCourier);
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
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "ADMIN, OWNER")]
        public async Task<ActionResult> GetAll()
        {
            var couriers = await _courierService.GetAll();
            return Ok(couriers);
        }

        [HttpGet("GetById")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "COURIER, ADMIN, OWNER")]
        public IActionResult GetById(int id)
        {
            var courier = _courierService.GetById(id);
            if (courier is null)
                return NotFound("Courier Does not Exist");
            return Ok(courier);
        }

        [HttpDelete("Delete")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "ADMIN, OWNER")]
        public IActionResult Delete(int id)
        {
            try
            {
                _courierService.Delete(id);
                return Ok(new { message = "Courier deleted successfully" });
            }
            catch (Exception e)
            {
                return NotFound("Courier does not Exist");
            }
        }
    }
}
