using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Defect
{
    [Key]
    public int Id { get; set; }

    [ForeignKey("Inventory")]
    public int InventoryId { get; set; }
    public Inventory Inventory { get; set; }

    [Required]
    public DateTime DefectDate { get; set; }

    [Required]
    public string Description { get; set; }

    [Required]
    [MaxLength(50)]
    public string Status { get; set; } = "active";

    public decimal? RepairCost { get; set; }
}
