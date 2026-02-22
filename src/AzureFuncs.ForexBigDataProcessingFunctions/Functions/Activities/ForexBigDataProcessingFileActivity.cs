namespace AzureFuncs.ForexBigDataProcessingFunctions.Functions.Activities;
public sealed class ForexBigDataProcessingFileActivity(IBlobStorageService blobStorageService)
{
    private readonly IBlobStorageService _blobStorageService = blobStorageService ?? throw new ArgumentNullException(nameof(blobStorageService));

    /// <summary>
    /// A Activity function that returns a list of file names from a blob container with a given subcontainer name
    /// </summary>
    /// <param name="paras"></param>
    /// <param name="executionContext"></param>
    /// <returns></returns>
    [Function(nameof(GetFileListAsync))]
    public async Task<List<string>> GetFileListAsync([ActivityTrigger] ForexContainerModel paras, FunctionContext executionContext)
    {
        // Get the logger from the execution context
        ILogger logger = executionContext.GetLogger("GetFileListAsync");

        // Validate the parameters
        ArgumentNullException.ThrowIfNull(paras.ContainerName, nameof(ForexContainerModel.ContainerName));
        ArgumentNullException.ThrowIfNull(paras.SubContainerName, nameof(ForexContainerModel.SubContainerName));

        logger.LogInformation("getting blobNames {blobNames}", paras);

        // Call the blob storage service to get the list of file names
        return await _blobStorageService.GetblobNamesAsync(paras.ContainerName, paras.SubContainerName);
    }

    /// <summary>
    /// A function that reads a file from a blob container and returns a ForexExchangeRateModel object
    /// </summary>
    /// <param name="paras"></param>
    /// <param name="executionContext"></param>
    /// <returns></returns>
    [Function(nameof(ReadFileAsync))]
    public async Task<ForexExchangeRateModel> ReadFileAsync([ActivityTrigger] ForexContainerBlobModel paras, FunctionContext executionContext)
    {
        // Get the logger and the storage client provider from the execution context
        ILogger logger = executionContext.GetLogger("ReadFileListAsync");

        // Validate the parameters
        ArgumentNullException.ThrowIfNull(paras.ContainerName, nameof(ForexContainerBlobModel.ContainerName));
        ArgumentNullException.ThrowIfNull(paras.BlobName, nameof(ForexContainerBlobModel.BlobName));

        // Log the parameters
        logger.LogInformation("Saying hello to {name}.", paras);

        // Return the result
        return await _blobStorageService.ReadBlobFileAsync(paras.ContainerName, paras.BlobName);
    }

    [Function(nameof(WriteToBlob))]
    public async Task<bool> WriteToBlob([ActivityTrigger] string content)
    {
        return await _blobStorageService.UploadJsonConetentToBlobAsync(ForexContainerNames.ForexExchangeJsonConetentContainerName, content);
    }
}
