using OneApp.Business.DTOs;
using OneApp.Contracts.v1.Request;

namespace OneApp.Business.Interfaces;

public interface ICustomerService
{
    Task<Guid> AddCustomer(CreateCustomerRequest request);

    Task<CustomerDto?> GetCustomerbyId(string id);

    Task<IEnumerable<CustomerDto>> GetCustomers();
}
