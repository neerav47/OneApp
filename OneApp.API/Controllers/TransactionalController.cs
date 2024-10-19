using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OneApp.Business.Interfaces;
using OneApp.Contracts.v1.Enums;
using OneApp.Contracts.v1.Request;
using OneApp.Contracts.v1.Response;
using System.Net;

namespace OneApp.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class TransactionalController(ITransactionalService _transactionalService, IMapper _mapper) : ControllerBase
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
    [ProducesResponseType(typeof(Invoice), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> GetInvoiceById(string id)
    {
        var invoiceDto = await _transactionalService.GetInvoiceById(id);
        var invoice = _mapper.Map<Invoice>(invoiceDto);
        return invoice != null ? Ok(invoice) : NotFound();
    }

    [HttpDelete("v1/invoice/{id}")]
    [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
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
    [ProducesResponseType(typeof(IEnumerable<InvoiceItem>), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> GetInvoiceItemsByInvoiceId(string id)
    {
        var invoiceItemDtos = await _transactionalService.GetInvoiceItemsByInvoiceId(id);
        var invoiceItems = _mapper.Map<List<InvoiceItem>?>(invoiceItemDtos);
        return invoiceItems?.Count > 0 ? Ok(invoiceItems) : NotFound();
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

    [HttpPost("v1/invoice/checkout")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> Checkout([FromBody] CheckOutRequest request)
    {
        var result = await _transactionalService.CheckOut(request);
        return Ok(result);
    }

    [HttpGet("v1/invoices/{id}")]
    [ProducesResponseType(typeof(IEnumerable<Invoice>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetInvoices(Guid id, [FromQuery] Status? status, [FromQuery] Guid? userId)
    {
        var invoiceDtos = await _transactionalService.GetInvoices(id, status, userId);
        return Ok(_mapper.Map<List<Invoice>?>(invoiceDtos));
    }
}
