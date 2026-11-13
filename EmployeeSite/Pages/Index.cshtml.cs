using EmployeeSite.Data;
using EmployeeSite.Models;
using EmployeeSite.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace EmployeeSite.Pages;

public class IndexModel(EmployeeContext db, CompanyServiceClient companyService, ILogger<IndexModel> logger) : PageModel
{
    public IList<Employee> Employees { get; private set; } = [];

    public IList<Company> Companies { get; private set; } = [];

    public bool CompanyServiceUnavailable { get; private set; }

    [TempData]
    public string? StatusMessage { get; set; }

    public async Task<IActionResult> OnPostDeleteEmployeeAsync(int id, CancellationToken ct)
    {
        await db.Employees.Where(e => e.EmployeeId == id).ExecuteDeleteAsync(ct);
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostDeleteCompanyAsync(int id, CancellationToken ct)
    {
        try
        {
            await companyService.DeleteCompanyAsync(id, ct);
        }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException)
        {
            logger.LogWarning(ex, "Company service call failed.");
            StatusMessage = "The company could not be deleted because the company service is unavailable.";
        }

        return RedirectToPage();
    }

    public async Task OnGetAsync(CancellationToken ct)
    {
        Employees = await db.Employees.AsNoTracking().OrderBy(e => e.EmployeeId).ToListAsync(ct);

        try
        {
            await SyncCompaniesAsync(await companyService.GetCompaniesAsync(ct), ct);
        }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException or System.Text.Json.JsonException)
        {
            // Fall back to the last stored companies.
            logger.LogWarning(ex, "Company service call failed.");
            CompanyServiceUnavailable = true;
        }

        Companies = await db.Companies.AsNoTracking().OrderBy(c => c.CompanyId).ToListAsync(ct);
    }

    private async Task SyncCompaniesAsync(IReadOnlyList<Company> fromService, CancellationToken ct)
    {
        var existing = await db.Companies.ToDictionaryAsync(c => c.CompanyId, ct);

        foreach (var company in fromService)
        {
            if (existing.Remove(company.CompanyId, out var stored))
            {
                stored.CompanyName = company.CompanyName;
                stored.City = company.City;
            }
            else
            {
                db.Companies.Add(company);
            }
        }

        db.Companies.RemoveRange(existing.Values);
        await db.SaveChangesAsync(ct);
    }
}
