## Example of Enabling OTEL and Distrbuted tracing in an Azure Durable Function

### 1. Add Distributed Tracing and OTEL to the host.json file
```json
{
  "telemetryMode": "openTelemetry",
  "extensions": {
    "durableTask": {
      "tracing": {
        "traceInputsAndOutputs": true,
        "traceReplayEvents": true,
        "DistributedTracingEnabled": true,
        "Version": "V2"
      }
     
    }
  }
}
```

### 2. Configure OTEL tracing in the code
```csharp
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
```

### 3. Set up the OTLP URL and the Digma Environment
```
OTEL_EXPORTER_OTLP_ENDPOINT=OTEL_COLLECTOR_URL
OTEL_RESOURCE_ATTRIBUTES=digma.environment={YOUR_ENV_NAME},digma.environment.type=Public

```

The complete trace:
<img width="863" alt="image" src="https://github.com/user-attachments/assets/cb6dc913-799e-4a28-adfb-0a012e9cc639" />

