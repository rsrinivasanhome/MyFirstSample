using EmployeeSite.Models;
using EmployeeSite.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EmployeeSite.Pages.Companies;

public class EditModel(CompanyServiceClient companyService, ILogger<EditModel> logger) : PageModel
{
    [BindProperty]
    public Company Company { get; set; } = new();

    public bool IsNew => Company.CompanyId == 0;

    public async Task<IActionResult> OnGetAsync(int? id, CancellationToken ct)
    {
        if (id is null) return Page();

        try
        {
            var company = await companyService.GetCompanyAsync(id.Value, ct);
            if (company is null) return NotFound();
            Company = company;
        }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException)
        {
            logger.LogWarning(ex, "Company service call failed.");
            ModelState.AddModelError(string.Empty, "The company service is currently unavailable.");
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int? id, CancellationToken ct)
    {
        Company.CompanyId = id ?? 0;
        if (!ModelState.IsValid) return Page();

        try
        {
            if (IsNew)
                await companyService.CreateCompanyAsync(Company, ct);
            else
                await companyService.UpdateCompanyAsync(Company, ct);
        }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException)
        {
            logger.LogWarning(ex, "Company service call failed.");
            ModelState.AddModelError(string.Empty, "The company could not be saved because the company service is unavailable.");
            return Page();
        }

        return RedirectToPage("/Index");
    }
}
