namespace AzureFuncs.ForexBigDataProcessingFunctions.Extensions;
public static class StartupExtensions
{
    public static IServiceCollection AddProviders(this IServiceCollection services)
    {
        var storageAccountConnectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
        ArgumentNullException.ThrowIfNull(storageAccountConnectionString, nameof(storageAccountConnectionString));
        services.AddSingleton(c => new StorageClientProvider(storageAccountConnectionString));

        return services;
    }
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddTransient<IBlobStorageService, BlobStorageService>();

        return services;
    }
}
