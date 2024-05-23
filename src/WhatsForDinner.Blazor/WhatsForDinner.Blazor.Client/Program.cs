using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using WhatsForDinner.Api.HttpClient;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services
    .AddScoped(_ => new HttpClient { BaseAddress = new(builder.HostEnvironment.BaseAddress) })
    .AddScoped<ApiClient>();

await builder.Build().RunAsync();