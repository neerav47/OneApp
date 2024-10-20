using System.Runtime.Serialization;

namespace OneApp.Contracts.v1.Request;

[DataContract]
public class CreateCustomerRequest
{
    [DataMember(Name = "firstName")]
    public string FirstName { get; set; } = default!;

    [DataMember(Name = "lastName")]
    public string LastName { get; set; } = default!;

    [DataMember(Name = "email")]
    public string? Email { get; set; }

    [DataMember(Name = "phone")]
    public string Phone { get; set; } = default!;

    [DataMember(Name = "address")]
    public string Address { get; set; } = default!;
}
