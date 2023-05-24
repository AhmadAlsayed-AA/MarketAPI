using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Market.Data.Stores;
using Market.Services;
using Market.Services.Helpers.Validation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace Market.Controllers.UserControllers
{
    [ApiController]
    [Route("api/stores")]
    public class StoreController : ControllerBase
    {
        private readonly IStoreService _storeService;

        public StoreController(IStoreService storeService)
        {
            _storeService = storeService;
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "ADMIN, OWNER")]
        public async Task<IActionResult> GetAllStores()
        {
            var stores = await _storeService.GetAll();
            return Ok(stores);
        }

        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "ADMIN, OWNER")]
        public async Task<IActionResult> GetStoreById(int id)
        {
            var store = await _storeService.GetById(id);
            if (store == null)
            {
                return NotFound();
            }
            return Ok(store);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "ADMIN, OWNER")]
        public async Task<ActionResult> CreateStore([FromBody] StoreRequest storeRequest)
        {
            try
            {
                var store = await _storeService.Create(storeRequest);
                return Ok(store);
            }
            catch (ValidationException ex)
            {
                // Re-throw the exception with the original error messages
                return BadRequest(new { errors = ex.Errors });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
            }
        }

        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "ADMIN, OWNER")]
        public async Task<IActionResult> UpdateStore(int id, [FromBody] StoreUpdateRequest request)
        {
            try
            {
                var updatedStore = await _storeService.Update(id, request);
                return Ok(updatedStore);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
            }
        }

        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "ADMIN, OWNER")]
        public async Task<IActionResult> DeleteStore(int id)
        {
            try
            {
                await _storeService.Delete(id);
                return Ok("Store Deleted");
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
