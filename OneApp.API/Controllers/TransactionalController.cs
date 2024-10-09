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
    public async Task<IActionResult> CreateInvoice([FromBody] CreateInvoiceRequest request)
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

    [HttpDelete("v1/invoice/{id}")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> DeleteInvoiceById(string id)
    {
        var result = await _transactionalService.DeleteInvoice(id);
        return Ok(result);
    }

    [HttpPost("v1/invoice/{id}/invoiceItem")]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> AddInvoiceItem([FromBody] AddInvoiceItemRequest request)
    {
        var invoiceItemId = await _transactionalService.AddInvoiceItem(request);
        return Ok(invoiceItemId);
    }

    [HttpGet("v1/invoice/{id}/invoiceItems")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> GetInvoiceItemsByInvoiceId(string id)
    {
        var invoiceItems = await _transactionalService.GetInvoiceItemsByInvoiceId(id);
        return invoiceItems?.Count() > 0 ? Ok(invoiceItems) : NotFound();
    }

    [HttpPut("v1/invoice/{invoiceId}/invoiceItem/{itemId}")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> EditInvoiceItem(string invoiceId, string itemId, [FromBody] EditInvoiceItemRequest request)
    {
        var result = await _transactionalService.EditInvoiceItem(Guid.Parse(invoiceId), Guid.Parse(itemId), request);
        return Ok(result);
    }

    [HttpDelete("v1/invoice/{invoiceId}/invoiceItem/{itemId}")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> DeleteInvoiceItem(Guid invoiceId, Guid itemId)
    {
        var result = await _transactionalService.DeleteInvoiceItem(invoiceId, itemId);
        return Ok(result);
    }
}
