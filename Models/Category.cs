using System.ComponentModel.DataAnnotations;

public class Category
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(255)]
    public string CategoryName { get; set; }
}
