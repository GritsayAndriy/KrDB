using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Inventory
{
    [Key]
    public int Id { get; set; }

    [ForeignKey("Equipment")]
    public int EquipmentId { get; set; }
    public Equipment Equipment { get; set; }

    [Required]
    [MaxLength(255)]
    public string SerialNumber { get; set; }

    [Required]
    public DateTime PurchaseDate { get; set; }

    [Required]
    [MaxLength(50)]
    public string Condition { get; set; } = "new";

    [Required]
    public bool Availability { get; set; } = true;
}
