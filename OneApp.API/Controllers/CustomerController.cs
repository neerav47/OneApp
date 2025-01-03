﻿using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OneApp.Business.Interfaces;
using OneApp.Contracts.v1.Request;
using OneApp.Contracts.v1.Response;
using System.Net;

namespace OneApp.API.Controllers;

[Route("api")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class CustomerController(
    ICustomerService _customerService,
    IMapper _mapper) : ControllerBase
{
    [HttpPost("v1/customer")]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> AddCustomer([FromBody] CreateCustomerRequest request)
    {
        var customerId = await _customerService.AddCustomer(request);
        return CreatedAtAction(nameof(GetCustomerById), new { id = customerId }, null);
    }

    [HttpGet("v1/customer/{id}")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> GetCustomerById(string id)
    {
        var customer = await _customerService.GetCustomerbyId(id);
        return customer != null ? Ok(customer) : NotFound();
    }

    [HttpGet("v1/customers")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetCustomers()
    {
        var customerDtos = await _customerService.GetCustomers();
        return Ok(_mapper.Map<List<Customer>>(customerDtos));
    }
}
