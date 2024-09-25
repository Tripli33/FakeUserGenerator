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

        var users = userGenerator.GenerateUsers(region, seed, 20, 0);
        Users = errorInjector.InjectErrors(users, errorCount, region);
    }

    public IActionResult OnGetMoreUsers([FromQuery]string region, [FromQuery] int seed, [FromQuery] double errorCount, [FromQuery] int page) 
    {
        Console.WriteLine(region);
        Seed = seed;
        Region = region;
        ErrorCount = errorCount;
        var users = userGenerator.GenerateUsers(region, seed, 10, page);
        var injectedUsers = errorInjector.InjectErrors(users, errorCount, region);

        return new JsonResult(injectedUsers);
    }

    public IActionResult OnPostChangeRegion()
    {
        return RedirectToPage(new { region = Region, seed = Seed, errorCount = ErrorCount });
    }

    public IActionResult OnPostSeed()
    {
        return RedirectToPage(new { region = Region, seed = Seed, errorCount = ErrorCount });
    }

    public IActionResult OnPostRandomSeed()
    {
        var random = new Random();
        Seed = random.Next(int.MaxValue);

        return RedirectToPage(new { region = Region, seed = Seed, errorCount = ErrorCount });
    }

    public IActionResult OnPostErrorInject()
    {
        return RedirectToPage(new { region = Region, seed = Seed, errorCount = ErrorCount });
    }
}
