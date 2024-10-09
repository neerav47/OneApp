using OneApp.Business.DTOs;
using OneApp.Contracts.v1.Request;

namespace OneApp.Business.Interfaces;

public interface ITransactionalService
{
    Task<Guid> CreateInvoice(CreateInvoiceRequest request);

    Task<InvoiceDto?> GetInvoiceById(string id);

    Task<bool> DeleteInvoice(string id);

    Task<Guid> AddInvoiceItem(AddInvoiceItemRequest request);

    Task<IEnumerable<InvoiceItemDto>> GetInvoiceItemsByInvoiceId(string invoiceId);

    Task<bool> EditInvoiceItem(Guid invoiceId, Guid invoiceItemId, EditInvoiceItemRequest request);

    Task<bool> DeleteInvoiceItem(Guid invoiceId, Guid invoiceItemId);

    Task CheckOut();
}
