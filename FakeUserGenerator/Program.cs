using FakeUserGenerator.Services;
using FakeUserGenerator.Utilities.Settings;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddSession();
builder.Services.AddScoped<IUserGenerator, UserGenerator>();
builder.Services.AddScoped<IErrorInjector, ErrorInjector>();
builder.Services.AddScoped<IExportCsvService, ExportCsvService>();
builder.Services.Configure<RegionOptions>(builder.Configuration.GetSection(nameof(RegionOptions)));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthorization();

app.MapRazorPages();

app.Run();