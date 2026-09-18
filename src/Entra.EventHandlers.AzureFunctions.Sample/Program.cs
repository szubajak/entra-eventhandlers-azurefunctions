using Entra.EventHandlers.AzureFunctions.DI;
using Entra.EventHandlers.AzureFunctions.Sample.Services;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

// Register your handler ecosystem
builder.Services.AddEntraEventHandlers();

// Register sample services
builder.Services.AddTransient<IEmailSender, ConsoleEmailSender>();

await builder.Build().RunAsync();
