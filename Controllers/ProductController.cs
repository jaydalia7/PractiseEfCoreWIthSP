using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PractiseEfCoreWIthSP.Models.ViewModels;
using PractiseEfCoreWIthSP.Services;
using PractiseEfCoreWIthSP.Services.IService;

namespace PractiseEfCoreWIthSP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] AddProductModel addProductModel, CancellationToken cancellationToken = default)
        {
            var result = await _productService.CreateProduct(addProductModel, cancellationToken);
            if (result.StatusCode == StatusCodes.Status200OK)
                return Ok(result);
            else
                return BadRequest(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllProduct(CancellationToken cancellationToken)
        {
            var result = await _productService.GetAllProduct(cancellationToken);
            if (result.StatusCode == StatusCodes.Status404NotFound)
            {
                return NotFound(result);
            }
            else if (result.StatusCode == StatusCodes.Status200OK)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpPut("{productId}")]
        public async Task<IActionResult> UpdateProduct([FromRoute] int productId, [FromBody] AddProductModel updateProductModel, CancellationToken cancellationToken = default)
        {
            var result = await _productService.UpdateProduct(productId, updateProductModel, cancellationToken);
            if (result.StatusCode == StatusCodes.Status404NotFound)
            {
                return NotFound(result);
            }
            else if (result.StatusCode == StatusCodes.Status200OK)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [Authorize]
        [HttpDelete("{productId}")]
        public async Task<IActionResult> RemoveProduct([FromRoute] int productId, CancellationToken cancellationToken = default)
        { 
            var result = await _productService.RemoveProduct(productId, cancellationToken);
            if (result.StatusCode == StatusCodes.Status404NotFound)
            {
                return NotFound(result);
            }
            else if (result.StatusCode == StatusCodes.Status200OK)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }
    }
}
