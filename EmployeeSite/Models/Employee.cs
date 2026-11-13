using System.ComponentModel.DataAnnotations;

namespace EmployeeSite.Models;

public class Employee
{
    [Key]
    public int EmployeeId { get; set; }

    [Required, MaxLength(100), Display(Name = "Employee Name")]
    public string EmployeeName { get; set; } = string.Empty;

    [Range(16, 100)]
    public int Age { get; set; }
}
