using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Market.Data.Customers;
using Market.Data.Stores;
using Market.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MarketAPI.Controllers
{
    [Route("[controller]")]
    public class CustomerController : Controller
    {
        // GET: /<controller>/
        private ICustomerService _customerService;
        private IMapper _mapper;

        public CustomerController(
            ICustomerService customerService, IMapper mapper)

        {
            _mapper = mapper;

            _customerService = customerService;

        }

        [HttpPost("Create")]
        public async Task<ActionResult> CreateCustomer(CustomerRequest request)
        {
            try
            {
                _customerService.create(request);
                return Ok("Customer Created Successfully");

            }
            catch (NullReferenceException e)
            {
                return BadRequest("Customer Cannot Be null");
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
            var customers = _customerService.getAll();
            return Ok(customers);
        }

        [HttpGet("GetById")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "STORE, ADMIN")]
        public IActionResult GetById(int id)
        {
            var customer = _customerService.getById(id);
            if (customer is null)
                return NotFound("Store Does not Exist");
            return Ok(customer);
        }

        [HttpPut("Update")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "STORE, ADMIN")]
        public IActionResult Update(int id, CustomerUpdateRequest model)
        {
            try
            {


                return Ok(_customerService.update(id, model));

            }
            catch (HttpRequestException e)
            {
                return Conflict(e.Message);

            }
            catch (NullReferenceException n)
            {
                return NotFound("Customer Does not Exist");
            }
        }

        

        [HttpDelete("Delete")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "ADMIN")]
        public IActionResult Delete(int id)
        {
            try
            {
                _customerService.delete(id);
                return Ok(new { message = "Customer deleted successfully" });

            }
            catch (Exception e)
            {
                return NotFound("Customer does not Exist");
            }
        }
    }
}

