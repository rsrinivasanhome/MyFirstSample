using EmployeeSite.Data;
using EmployeeSite.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EmployeeSite.Pages.Employees;

public class EditModel(EmployeeContext db) : PageModel
{
    [BindProperty]
    public Employee Employee { get; set; } = new();

    public bool IsNew => Employee.EmployeeId == 0;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id is null) return Page();

        var employee = await db.Employees.FindAsync(id.Value);
        if (employee is null) return NotFound();

        Employee = employee;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int? id)
    {
        Employee.EmployeeId = id ?? 0;
        if (!ModelState.IsValid) return Page();

        if (IsNew)
        {
            db.Employees.Add(Employee);
        }
        else
        {
            var stored = await db.Employees.FindAsync(Employee.EmployeeId);
            if (stored is null) return NotFound();
            stored.EmployeeName = Employee.EmployeeName;
            stored.Age = Employee.Age;
        }

        await db.SaveChangesAsync();
        return RedirectToPage("/Index");
    }
}
