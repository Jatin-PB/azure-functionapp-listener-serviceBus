using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ServiceBusListener2.Data;
using Microsoft.EntityFrameworkCore;

var builder = FunctionsApplication.CreateBuilder(args);

//builder.ConfigureFunctionsWebApplication();

// Application Insights isn't enabled by default. See https://aka.ms/AAt8mw4.
// builder.Services
//     .AddApplicationInsightsTelemetryWorkerService()
//     .ConfigureFunctionsApplicationInsights();


//builder.Build().Run();


// Register your DbContext with the connection string from environment variable
var sqlConnection = Environment.GetEnvironmentVariable("SqlConnection");

if (string.IsNullOrWhiteSpace(sqlConnection))
{
    throw new InvalidOperationException("Environment variable 'SqlConnection' is not set.");
}

builder.Services.AddDbContext<MessagesDbContext>(options =>
    options.UseSqlServer(sqlConnection));

// Optional: enable Application Insights if needed
// builder.Services
//     .AddApplicationInsightsTelemetryWorkerService()
//     .ConfigureFunctionsApplicationInsights();

var app = builder.Build();
app.Run();

