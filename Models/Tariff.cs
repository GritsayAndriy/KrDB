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


    public decimal DiscountAmount(decimal BasePricePerDay) 
    {
        int days = 0;
        switch (this.Id)
        {
            case 1:
                days = 1;
                break;
            case 2:
                days = 7;
                break;
            case 3:
                days = 30;
                break;
            default:
                days = 0;
                break;
        }

        return days * BasePricePerDay * this.Multiplier;
    }
}
