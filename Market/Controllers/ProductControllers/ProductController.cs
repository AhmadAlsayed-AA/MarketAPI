using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Market.Data.Products;
using Market.Services.ProductServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace MarketAPI.Controllers.ProductControllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "ADMIN, OWNER")]

        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _productService.GetAll();
            return Ok(products);
        }

        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "ADMIN, STORE, CUSTOMER, OWNER")]

        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _productService.GetById(id);
            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }
        [HttpGet("store/{storeId}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "ADMIN, STORE, CUSTOMER, OWNER")]

        public async Task<IActionResult> GetProductsByStoreId(int storeId)
        {
            var products = await _productService.GetProductsByStoreId(storeId);
            return Ok(products);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "ADMIN, STORE")]

        public async Task<IActionResult> CreateProduct([FromBody] Product product)
        {
            await _productService.Create(product);
            return Ok("Product created successfully");
        }

        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "ADMIN, STORE")]

        public async Task<IActionResult> UpdateProduct(int id, [FromBody] Product updatedProduct)
        {
            try
            {
                await _productService.Update(id, updatedProduct);
                return Ok("Product updated successfully");
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "ADMIN, STORE")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                await _productService.Delete(id);
                return Ok("Product deleted successfully");
            }
            catch (Exception)
            {
                return NotFound();
            }
        }
    }
}