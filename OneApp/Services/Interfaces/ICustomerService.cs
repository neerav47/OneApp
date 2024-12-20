using System;
using OneApp.Contracts.v1.Response;

namespace OneApp.Services.Interfaces;

public interface ICustomerService
{
    Task<IEnumerable<Customer>> GetCustomers();
}
