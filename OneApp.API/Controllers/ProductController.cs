using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OneApp.Business.Constants;
using OneApp.Business.Interfaces;
using OneApp.Contracts.v1.Request;
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
        var context = GetContext();
        var products = await _productService.GetAllProducts(context);
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
        var context = GetContext();
        var result = await _productService.DeleteProductById(id, context);
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
        var context = GetContext();
        var product = await _productService.CreateProduct(request, context);
        return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, null);
    }

    [HttpGet("v1/getProductTypes")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> GetAllProductTypes()
    {
        var context = GetContext();
        var productTypes = await _productService.GetAllProductTypes(context);
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

        return new Context { UserId = Guid.Parse(userId), TenantId = Guid.Parse(tenantId) };
    }
    #endregion
}

