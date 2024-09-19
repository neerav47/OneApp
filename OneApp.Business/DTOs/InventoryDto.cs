using OneApp.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace OneApp.Business.DTOs;

public sealed class InventoryDto
{
    public int Quantity { get; set; }

    public long TransactionId { get; set; }

    public DateTime CreatedDate { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime LastUpdatedDate { get; set; }

    public Guid LastUpdatedBy { get; set; }
}


