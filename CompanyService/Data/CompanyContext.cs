using CompanyService.Models;
using Microsoft.EntityFrameworkCore;

namespace CompanyService.Data;

public class CompanyContext(DbContextOptions<CompanyContext> options) : DbContext(options)
{
    public DbSet<Company> Companies => Set<Company>();
}
