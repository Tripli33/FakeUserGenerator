using FakeUserGenerator.Models;
using FakeUserGenerator.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FakeUserGenerator.Pages;

public class IndexModel(IUserGenerator userGenerator, IErrorInjector errorInjector) : PageModel
{
    public IEnumerable<User>? Users { get; set; }
    [BindProperty]
    public int Seed { get; set; }
    [BindProperty]
    public string Region { get; set; }
    [BindProperty]
    public double ErrorCount { get; set; }

    public void OnGet(string region = "US", int seed = 0, double errorCount = 0)
    {
        Seed = seed;
        Region = region;
        ErrorCount = errorCount;
        var users = userGenerator.GenerateUsers(region, seed, 20);
        Users = errorInjector.InjectErrors(users, errorCount, region);
    }

    public IActionResult OnPostSeed()
    {
        return RedirectToPage(new { region = Region, seed = Seed });
    }

    public IActionResult OnPostErrorInject()
    {
        return RedirectToPage(new { region = Region, seed = Seed, errorCount = ErrorCount});
    }
}
