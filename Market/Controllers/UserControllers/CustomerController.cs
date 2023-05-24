using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Market.Data.Customers;
using Market.Data.Shared;
using Market.Data.Stores;
using Market.Services;
using Market.Services.Helpers.Validation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MarketAPI.Controllers.UserControllers
{
    [Route("[controller]")]
    public class CustomerController : Controller
    {
        private ICustomerService _customerService;
        private IMapper _mapper;

        public CustomerController(ICustomerService customerService, IMapper mapper)
        {
            _customerService = customerService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCustomer([FromBody] RegistrationDTO request)
        {
            try
            {
                var customer = await _customerService.Create(request);
                return Ok(customer);
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { errors = ex.Errors });
            }
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "ADMIN, OWNER")]
        public async Task<IActionResult> GetAll()
        {
            var customers = await _customerService.GetAll();
            return Ok(customers);
        }

        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "STORE, ADMIN, OWNER")]
        public async Task<IActionResult> GetById(int id)
        {
            var customer = await _customerService.GetById(id);
            if (customer is null)
                return NotFound("Store Does not Exist");
            return Ok(customer);
        }

        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = " ADMIN, OWNER")]
        public async Task<IActionResult> Update(int id, [FromBody] CustomerUpdateRequest model)
        {
            try
            {
                return Ok(await _customerService.Update(id, model));
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

        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "ADMIN, OWNER")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _customerService.Delete(id);
                return Ok(new { message = "Customer deleted successfully" });
            }
            catch (Exception e)
            {
                return NotFound("Customer does not Exist");
            }
        }
    }
}
