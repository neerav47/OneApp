using System.Runtime.Serialization;

namespace OneApp.Contracts.v1.Request;

[DataContract]
public class EditInvoiceItemRequest
{
    [DataMember(Name = "unitPrice")]
    public decimal UnitPrice { get; set; }

    [DataMember(Name = "quantity")]
    public decimal Quantity { get; set; }
}
