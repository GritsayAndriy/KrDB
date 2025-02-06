using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Equipment
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(255)]
    public string Name { get; set; }

    [MaxLength(255)]
    public string Brand { get; set; }

    [MaxLength(255)]
    public string Model { get; set; }

    [ForeignKey("Category")]
    public int? CategoryId { get; set; }
    public Category Category { get; set; }

    [Required]
    public decimal BasePricePerDay { get; set; }
}
