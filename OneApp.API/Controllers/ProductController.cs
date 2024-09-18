using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OneApp.Business.Constants;
using OneApp.Business.DTOs;
using OneApp.Business.Interfaces;
using OneApp.Contracts.v1.Request;
using OneApp.Data.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;

namespace OneApp.API.Controllers;

[Route("api")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class ProductController(
    IProductService _productService,
    IMapper _mapper,
    IHttpContextAccessor _httpContextAccessor) : ControllerBase
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
    [Authorize(Roles = Role.SystemAdmin)]
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
    [Authorize(Roles = Role.SystemAdmin)]
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
        var context = GetContext();
        var productType = await _productService.CreateProductType(request, context);
        return CreatedAtAction(nameof(GetProductTypeById), new { id = productType.Id }, null);
    }

    #region Private method
    private Context GetContext()
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(JwtRegisteredClaimNames.Sub)!;

        var tenantId = _httpContextAccessor.HttpContext?.User.FindFirstValue("tenant")!;

        return new Context { UserId = userId, TenantId = tenantId };
    }
    #endregion
}

