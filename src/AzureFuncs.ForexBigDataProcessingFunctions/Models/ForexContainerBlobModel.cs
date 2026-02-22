namespace AzureFuncs.ForexBigDataProcessingFunctions.Models;

public class ForexContainerBlobModel
{
    public ForexContainerBlobModel(string containerName, string blobName) => (ContainerName, BlobName) = (containerName, blobName);

    public ForexContainerBlobModel()
    {

    }
    public string ContainerName { get; set; } = default!;
    public string BlobName { get; set; } = default!;
}
