using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Payment
{
    [Key]
    public int Id { get; set; }

    [ForeignKey("Rental")]
    public int RentalId { get; set; }
    public Rental Rental { get; set; }

    [Required]
    public DateTime PaymentDate { get; set; }

    [Required]
    [MaxLength(50)]
    public string PaymentMethod { get; set; }

    [Required]
    public decimal Amount { get; set; }
}
