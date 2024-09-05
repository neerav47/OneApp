using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OneApp.Business.Interfaces;
using System.Net;

namespace OneApp.API.Controllers;

[Route("api")]
[ApiController]
public class ProductController(IProductService _productService, IMapper _mapper) : ControllerBase
{
    [HttpGet("v1/getProducts")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> GetAllProducts()
    {
        var products = await _productService.GetAllProducts();
        return Ok(products);
    }

    [HttpGet("v1/product/{id}")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> GetProductById(string id)
    {
        var product = await _productService.GetProductById(id);
        return product != null ? Ok(product) : NotFound();
    }

    [HttpDelete("v1/product/{id}")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> DeleteProductById(string id)
    {
        await _productService.DeleteProductById(id);
        return Ok();
    }

    [HttpPost("v1/product")]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> CreateProduct([FromBody] dynamic request)
    {
        await _productService.CreateProduct();
        return CreatedAtRoute(nameof(GetProductById), "");
    }

    [HttpGet("v1/getProductTypes")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> GetAllProductTypes()
    {
        var productTypes = await _productService.GetAllProductTypes();
        return Ok(productTypes);
    }

    [HttpPost("v1/productType")]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> CreateProductType([FromBody] dynamic request)
    {
        await _productService.CreateProductType();
        return Created();
    }
}

