using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OneApp.Data.Models;

namespace OneApp.Data.Context;

public class DataContext: IdentityDbContext<User>
{
    public DbSet<Tenant> Tenant { get; set; }
    public DataContext(DbContextOptions<DataContext> options): base(options)
    {
        
    }
}
