using System.ComponentModel.DataAnnotations;

namespace CompanyService.Models;

public class Company
{
    [Key]
    public int CompanyId { get; set; }

    [Required, MaxLength(150)]
    public string CompanyName { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? City { get; set; }
}
