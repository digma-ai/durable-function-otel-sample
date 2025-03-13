using System.Numerics;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Hosting;
using OpenTelemetry;
using Microsoft.Azure.Functions.Worker.OpenTelemetry;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Exporter;
using OpenTelemetry.Extensions.Hosting;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

// var host = new HostBuilder();
// host.ConfigureFunctionsWebApplication();
// host.ConfigureServices(services=>{
//     services.AddOpenTelemetry()
//             .UseFunctionsWorkerDefaults()
//             .UseOtlpExporter();
// }).Build().Run();

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

builder.Services.AddOpenTelemetry()
    .ConfigureResource(resource => resource.AddService("vanilla-function"))
    .UseFunctionsWorkerDefaults()
    .WithTracing(tracingBuilder =>
    {
        tracingBuilder
            .SetSampler<AlwaysOnSampler>()
            .AddSource("*")  // Ensure exact match
            .AddHttpClientInstrumentation()
            .AddAspNetCoreInstrumentation()
            .AddConsoleExporter();  // Debugging
    })
    .UseOtlpExporter();

builder.Build().Run();
