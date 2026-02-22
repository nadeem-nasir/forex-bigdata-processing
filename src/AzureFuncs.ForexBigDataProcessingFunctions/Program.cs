using AzureFuncs.ForexBigDataProcessingFunctions.Extensions;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices((hostContext, services) => {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
        services.AddProviders();
        services.AddServices();
    })
    .Build();

await host.RunAsync();
