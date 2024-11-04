using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OneApp.Business.Constants;
using OneApp.Business.Interfaces;
using OneApp.Contracts.v1.Request;
using System.Net;

namespace OneApp.API.Controllers;

[Route("api")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class ProductController(
    IProductService _productService,
    IMapper _mapper) : ControllerBase
{
    [HttpGet("v1/getProducts")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> GetAllProducts()
    {
        var productDtos = await _productService.GetAllProducts();
        var products = _mapper.Map<List<Contracts.v1.Response.Product>>(productDtos);
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
    [Authorize(Roles = Role.SystemAdmin)]
    public async Task<IActionResult> DeleteProductById(string id)
    {
        var result = await _productService.DeleteProductById(id);
        return Ok(result);
    }

    [HttpPost("v1/product")]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [Authorize(Roles = Role.SystemAdmin)]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductRequest request)
    {
        var product = await _productService.CreateProduct(request);
        return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, null);
    }

    [HttpGet("v1/getProductTypes")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> GetAllProductTypes()
    {
        var productTypes = await _productService.GetAllProductTypes();
        return Ok(productTypes);
    }

    [HttpGet("v1/getProductTypeId/{id}")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> GetProductTypeById(string id)
    {
        var productType = await _productService.GetProductTypeById(id);
        return Ok(productType);
    }


    [HttpPost("v1/productType")]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [Authorize (Roles = Role.SystemAdmin)]
    public async Task<IActionResult> CreateProductType([FromBody] CreateProductTypeRequest request)
    {
        var productType = await _productService.CreateProductType(request);
        return CreatedAtAction(nameof(GetProductTypeById), new { id = productType.Id }, null);
    }
}

