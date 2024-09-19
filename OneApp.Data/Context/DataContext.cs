﻿using Microsoft.AspNetCore.Identity;
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
    public DbSet<InventoryHistory> InventoryHistory { get; set; }

    public DataContext(DbContextOptions<DataContext> options): base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<IdentityRole>().HasData(new List<IdentityRole>()
        {
            new()
            {
                Id = "18bcb22b-b211-4031-bbe7-0b5a0b6170b3",
                Name = "Basic user",
                NormalizedName = "BASIC USER"
            },
            new()
            {
                Id = "58c49c6a-e192-4fa6-a040-b4db3a87f22b",
                Name = "System admin",
                NormalizedName = "SYSTEM ADMIN"
            },
            new()
            {
                Id = "1706a615-1a92-4218-a5fa-1342a671fe8b",
                Name = "Global admin",
                NormalizedName = "GLOBAL ADMIN"
            }
        });
    }
}
