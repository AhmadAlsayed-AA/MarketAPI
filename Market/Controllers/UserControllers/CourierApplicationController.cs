using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Market.Data.Couriers;
using Market.Data.Stores;
using Market.Services;
using Microsoft.AspNetCore.Mvc;
using static Market.Services.Helpers.LocalEnums.Enums;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace MarketAPI.Controllers.UserControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourierApplicationController : ControllerBase
    {
        private readonly ICourierApplicationService _courierApplicationService;

        public CourierApplicationController(ICourierApplicationService courierApplicationService)
        {
            _courierApplicationService = courierApplicationService;
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "ADMIN")]
        public async Task<IActionResult> GetAll()
        {
            var courierApplications = await _courierApplicationService.GetAll();
            return Ok(courierApplications);
        }

        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "ADMIN")]
        public async Task<IActionResult> GetById(int id)
        {
            var courierApplication = await _courierApplicationService.GetById(id);
            if (courierApplication == null)
            {
                return NotFound();
            }

            return Ok(courierApplication);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "ADMIN")]
        public async Task<IActionResult> Create([FromBody] CourierApplication request)
        {
            await _courierApplicationService.Create(request);
            return Ok("Courier application created successfully");
        }

        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "ADMIN")]
        public async Task<IActionResult> Update(int id, [FromBody] StoreUpdateRequest request)
        {
            try
            {
                await _courierApplicationService.Update(id, request);
                return Ok("Courier application updated successfully");
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "ADMIN")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _courierApplicationService.Delete(id);
                return Ok("Courier application deleted successfully");
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpPatch("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "ADMIN")]
        public async Task<IActionResult> UpdateStatus(int id, CourierApprovalStatus status)
        {
            try
            {
                await _courierApplicationService.UpdateStatus(id, status);
                return Ok("Courier application status updated successfully");
            }
            catch (Exception)
            {
                return NotFound();
            }
        }
    }
}
