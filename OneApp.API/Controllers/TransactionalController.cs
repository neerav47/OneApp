using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OneApp.Business.Interfaces;
using OneApp.Contracts.v1.Request;
using System.Net;

namespace OneApp.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class TransactionalController(ITransactionalService _transactionalService) : ControllerBase
{
    [HttpPost("v1/invoice")]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> CreateInvoice(CreateInvoiceRequest request)
    {
        var receiptId = await _transactionalService.CreateInvoice(request);
        return CreatedAtAction(nameof(GetInvoiceById), new { id = receiptId }, null);
    }

    [HttpGet("v1/invoice/{id}")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> GetInvoiceById(string id)
    {
        var invoice = await _transactionalService.GetInvoiceById(id);
        return invoice != null ? Ok(invoice) : NotFound();
    }
}
