using System;
using System.ComponentModel.DataAnnotations;

namespace OneApp.Business.DTOs;

public class InventoryHistoryDto
{
    public Guid InvetoryId { get; set; }

    public Guid ProductId { get; set; }

    public int Quantity { get; set; }

    public long TransactionId { get; set; }

    public DateTime CreatedDate { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime LastUpdatedDate { get; set; }

    public Guid LastUpdatedBy { get; set; }
}

