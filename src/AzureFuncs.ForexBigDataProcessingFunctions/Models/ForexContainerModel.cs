namespace AzureFuncs.ForexBigDataProcessingFunctions.Models;

public class ForexContainerModel
{
    public ForexContainerModel(string containerName, string subContainerName)
    {
        (ContainerName, SubContainerName) = (containerName, subContainerName);
    }
    public ForexContainerModel()
    {

    }
    public string? ContainerName { get; set; }
    public string? SubContainerName { get; set; }
}
