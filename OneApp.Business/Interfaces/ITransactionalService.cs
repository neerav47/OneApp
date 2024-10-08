using OneApp.Business.DTOs;
using OneApp.Contracts.v1.Request;

namespace OneApp.Business.Interfaces;

public interface ITransactionalService
{
    Task<Guid> CreateInvoice(CreateInvoiceRequest request);

    Task<InvoiceDto?> GetInvoiceById(string id);

    Task DeleteInvoice(string id);

    Task AddInvoiceItem();

    Task EditInvoiceItem();

    Task DeleteInvoiceItem();

    Task CheckOut();
}
