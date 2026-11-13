using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeSite.Models;

public class Company
{
    // Id is assigned by the external company service, not the database.
    [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int CompanyId { get; set; }

    [Required, MaxLength(150), Display(Name = "Company Name")]
    public string CompanyName { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? City { get; set; }
}
