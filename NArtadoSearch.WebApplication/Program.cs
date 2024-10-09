using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using NArtadoSearch.WebApplication;
using NArtadoSearch.WebApplication.Services;
using NArtadoSearch.WebApplication.Services.Abstractions;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7105/") });
builder.Services.AddScoped<ISearchService, SearchService>();
await builder.Build().RunAsync();