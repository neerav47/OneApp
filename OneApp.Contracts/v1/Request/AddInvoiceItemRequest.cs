using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace OneApp.Contracts.v1.Request;

[DataContract]
public class AddInvoiceItemRequest
{
    [Required]
    [DataMember(Name = "receiptId")]
    public Guid ReceiptId { get; set; }

    [Required]
    [DataMember(Name = "productId")]
    public Guid ProductId { get; set; }

    [Required]
    [DataMember(Name = "quantity")]
    public decimal Quantity { get; set; }

    [Required]
    [DataMember(Name = "unitPrice")]
    public decimal UnitPrice { get; set; }
}
