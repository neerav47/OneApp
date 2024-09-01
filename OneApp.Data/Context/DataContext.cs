using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OneApp.Data.Models;

namespace OneApp.Data.Context;

public class DataContext: IdentityDbContext<User>
{
    public DbSet<Tenant> Tenant { get; set; }
    public DbSet<Product> Product { get; set; }
    public DbSet<ProductType> ProductType { get; set; }
    public DbSet<Inventory> Inventory { get; set; }
    public DbSet<Transaction> Transaction { get; set; }

    public DataContext(DbContextOptions<DataContext> options): base(options)
    {
    }
}
