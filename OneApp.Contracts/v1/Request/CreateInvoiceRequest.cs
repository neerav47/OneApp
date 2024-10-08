using System.Runtime.Serialization;

namespace OneApp.Contracts.v1.Request;

[DataContract]
public class CreateInvoiceRequest
{
    [DataMember(Name = "customerId")]
    public Guid CustomerId { get; set; } = default!;
}
