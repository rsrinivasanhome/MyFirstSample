using System.Net;
using EmployeeSite.Models;

namespace EmployeeSite.Services;

public class CompanyServiceClient(HttpClient http)
{
    public async Task<IReadOnlyList<Company>> GetCompaniesAsync(CancellationToken ct = default)
    {
        var companies = await http.GetFromJsonAsync<List<Company>>("api/companies", ct);
        return companies ?? [];
    }

    public async Task<Company?> GetCompanyAsync(int id, CancellationToken ct = default)
    {
        using var response = await http.GetAsync($"api/companies/{id}", ct);
        if (response.StatusCode == HttpStatusCode.NotFound) return null;
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<Company>(ct);
    }

    public async Task CreateCompanyAsync(Company company, CancellationToken ct = default)
    {
        using var response = await http.PostAsJsonAsync("api/companies", company, ct);
        response.EnsureSuccessStatusCode();
    }

    public async Task UpdateCompanyAsync(Company company, CancellationToken ct = default)
    {
        using var response = await http.PutAsJsonAsync($"api/companies/{company.CompanyId}", company, ct);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteCompanyAsync(int id, CancellationToken ct = default)
    {
        using var response = await http.DeleteAsync($"api/companies/{id}", ct);
        if (response.StatusCode != HttpStatusCode.NotFound) response.EnsureSuccessStatusCode();
    }
}
