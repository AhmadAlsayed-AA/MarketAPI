
using Market.Data.Addresses;
using Market.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;


// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MarketAPI.Controllers
{
    [Route("[controller]")]
    public class AddressController : Controller
    {
        // GET: /<controller>/
        private readonly IAddressService _addressService;
        

        public AddressController(IAddressService addressService)

        {

            _addressService = addressService;

        }


       
        [HttpPost("CreateAddress")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "CUSTOMER, ADMIN")]
        public async Task<ActionResult> Create( AddressRequest model)
        {
            try
            {
                _addressService.create(model);
                return Ok("Address Created");

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
            var addresses = _addressService.getAll();
            return Ok(addresses);
        }

        [HttpGet("GetById/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "CUSTOMER, ADMIN")]
        public IActionResult GetById(int id)
        {
            var address = _addressService.getById(id);
            if (address is null)
                return NotFound("Address Does not Exist");

            return Ok(address);
        }
        [HttpGet("GetByUserId/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "CUSTOMER, ADMIN")]
        public IActionResult GetByUserId(int id)
        {
            var addresses = _addressService.getByUserId(id);
            if (addresses is null)
                return NotFound("Address Does not Have An Address");

            return Ok(addresses);
        }

        [HttpPut("Update/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "CUSTOMER,ADMIN")]
        public IActionResult Update(int id, AddressUpdateRequest model)
        {
            try
            {

                _addressService.update(id, model);
                return Ok(new { message = "Address updated successfully" });

            }
            catch (HttpRequestException e)
            {
                return Conflict(e.Message);

            }
            catch (NullReferenceException n)
            {
                return NotFound("Address Does not Exist");
            }
        }

        [HttpDelete("Delete/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Customer,ADMIN")]
        public IActionResult Delete(int id)
        {
            try
            {
                _addressService.delete(id);
                return Ok(new { message = "Address Deleted Successfully" });

            }
            catch (Exception e)
            {
                return NotFound("Address does not Exist");
            }
        }
    }
}

