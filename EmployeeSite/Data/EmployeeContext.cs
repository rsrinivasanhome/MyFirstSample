using EmployeeSite.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeSite.Data;

public class EmployeeContext(DbContextOptions<EmployeeContext> options) : DbContext(options)
{
    public DbSet<Employee> Employees => Set<Employee>();

    public DbSet<Company> Companies => Set<Company>();
}
