using System.ComponentModel.DataAnnotations;

public class Tariff
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(255)]
    public string TariffName { get; set; }

    [Required]
    public decimal Multiplier { get; set; }

    [Required]
    public decimal DiscountPercentage { get; set; } = 0;
}
