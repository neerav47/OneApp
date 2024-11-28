using System;
using OneApp.Contracts.v1.Response;

namespace OneApp.Services.Interfaces;

public interface ITransactionService
{
    Task<IEnumerable<Invoice>> GetInvoices(Contracts.v1.Enums.Status? status = null, Guid? userId = null);

    Task<Invoice> GetInvoiceById(Guid id);
}

