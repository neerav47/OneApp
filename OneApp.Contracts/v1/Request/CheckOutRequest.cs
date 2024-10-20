﻿using OneApp.Contracts.v1.Response;
using System.Runtime.Serialization;

namespace OneApp.Contracts.v1.Request;

[DataContract]
public class CheckOutRequest
{
    [DataMember(Name = "receiptId")]
    public required Guid ReceiptId { get; set; }

    [DataMember(Name = "tenantId")]
    public required Guid TenantId { get; set; }

    [DataMember(Name = "customerId")]
    public required Guid CustomerId { get; set; }

    [DataMember(Name = "invoiceItems")]
    public required IEnumerable<InvoiceItem> InvoiceItems { get; set; }

    public List<Guid> InvoiceItemIds => InvoiceItems?.Select(i => i.Id).ToList() ?? [];

    public List<Guid> ProductIds => InvoiceItems?.Select(i => i.ProductId).ToList() ?? [];
}
