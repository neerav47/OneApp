using System;
using System.Runtime.Serialization;

namespace OneApp.Contracts.v1.Request;

[DataContract]
public class NewInvoiceItem
{
    [DataMember(Name = "productId")]
    public Guid ProductId { get; set; }

    [DataMember(Name = "unitPrice")]
    public int UnitPrice { get; set; }

    [DataMember(Name = "quantity")]
    public int Quantity { get; set; }

    public int Total => UnitPrice * Quantity;

    public string ProductName { get; set; } = string.Empty;

    public Guid TempId { get; set; }
}
