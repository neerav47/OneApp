using OneApp.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace OneApp.Business.DTOs;

public sealed class InventoryDto
{
    public Product Product { get; set; } = default!;

    public int Quantity { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime CreatedBy { get; set; }

    public DateTime LastUpdatedDate { get; set; }

    public DateTime LastUpdatedBy { get; set; }
}


