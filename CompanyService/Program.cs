using CompanyService.Data;
using CompanyService.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddValidation();
builder.Services.AddDbContext<CompanyContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CompanyDb")));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

var companies = app.MapGroup("/api/companies");

companies.MapGet("/", async (CompanyContext db) =>
    await db.Companies.AsNoTracking().OrderBy(c => c.CompanyId).ToListAsync())
.WithName("GetCompanies");

companies.MapGet("/{id:int}", async (int id, CompanyContext db) =>
    await db.Companies.FindAsync(id) is { } company ? Results.Ok(company) : Results.NotFound())
.WithName("GetCompany");

companies.MapPost("/", async (Company company, CompanyContext db) =>
{
    company.CompanyId = 0;
    db.Companies.Add(company);
    await db.SaveChangesAsync();
    return Results.CreatedAtRoute("GetCompany", new { id = company.CompanyId }, company);
});

companies.MapPut("/{id:int}", async (int id, Company input, CompanyContext db) =>
{
    var company = await db.Companies.FindAsync(id);
    if (company is null) return Results.NotFound();

    company.CompanyName = input.CompanyName;
    company.City = input.City;
    await db.SaveChangesAsync();
    return Results.NoContent();
});

companies.MapDelete("/{id:int}", async (int id, CompanyContext db) =>
    await db.Companies.Where(c => c.CompanyId == id).ExecuteDeleteAsync() > 0
        ? Results.NoContent()
        : Results.NotFound());

app.Run();
