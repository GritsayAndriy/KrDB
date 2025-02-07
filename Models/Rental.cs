using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Rental
{
    [Key]
    public int Id { get; set; }

    [ForeignKey("Customer")]
    public int CustomerId { get; set; }
    public Customer? Customer { get; set; }

    [ForeignKey("Inventory")]
    public int InventoryId { get; set; }
    public Inventory? Inventory { get; set; }

    [Required]
    public DateTime StartDate { get; set; }

    [Required]
    public DateTime EndDate { get; set; }

    [ForeignKey("Tariff")]
    public int? TariffId { get; set; }
    public Tariff? Tariff { get; set; }

    [Required]
    public decimal TotalPrice { get; set; }

    [Required]
    [MaxLength(50)]
    public string Status { get; set; } = "pending";
}
