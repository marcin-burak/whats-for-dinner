using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.FluentUI.AspNetCore.Components;
using WhatsForDinner.Api.HttpClient;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services
	.AddScoped(_ => new HttpClient { BaseAddress = new(builder.HostEnvironment.BaseAddress) })
	.AddScoped<ApiClient>()
	.AddFluentUIComponents();

await builder.Build().RunAsync();