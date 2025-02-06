using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Customer
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(255)]
    public string FullName { get; set; }

    [Required]
    [MaxLength(20)]
    public string Phone { get; set; }

    [MaxLength(255)]
    public string Email { get; set; }

    public string Address { get; set; }

    [MaxLength(50)]
    [Column("passport_number")]
    public string PassportNumber { get; set; }
}
