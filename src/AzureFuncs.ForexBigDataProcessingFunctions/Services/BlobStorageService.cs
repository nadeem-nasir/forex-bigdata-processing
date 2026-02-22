using System.Text;

namespace AzureFuncs.ForexBigDataProcessingFunctions.Services;
public interface IBlobStorageService
{
    /// <summary>
    /// A method that returns a list of blob names from a blob container with a given prefix
    /// </summary>
    /// <param name="containerName"></param>
    /// <param name="prefix"></param>
    /// <returns></returns>

    Task<List<string>> GetblobNamesAsync(string containerName, string prefix);

    /// <summary>
    /// A method that reads a blob file and deserializes it into a ForexExchangeRateModel object
    /// </summary>
    /// <param name="containerName"></param>
    /// <param name="blobName"></param>
    /// <returns></returns>

    Task<ForexExchangeRateModel> ReadBlobFileAsync(string containerName, string blobName);

    /// <summary>
    /// A method that uploads a json string to a blob container
    /// </summary>
    /// <param name="containerName"></param>
    /// <param name="jsonToUpload"></param>
    /// <returns></returns>
    Task<bool> UploadJsonConetentToBlobAsync(string containerName, string jsonToUpload);
}
public class BlobStorageService : IBlobStorageService
{
    private readonly StorageClientProvider _storageClientProvider;
    private readonly BlobOpenReadOptions _readOptions;
    private readonly JsonSerializerOptions _jsonSerializerOptions;
    public BlobStorageService(StorageClientProvider storageClientProvider)
    {

        _storageClientProvider = storageClientProvider;

        _readOptions = new BlobOpenReadOptions(allowModifications: false);

        _jsonSerializerOptions = new JsonSerializerOptions {
            PropertyNameCaseInsensitive = true,
            NumberHandling = JsonNumberHandling.AllowReadingFromString
        };
    }

    /// <summary>
    /// A method that returns a list of blob names from a blob container with a given prefix
    /// </summary>
    /// <param name="containerName"></param>
    /// <param name="prefix"></param>
    /// <returns></returns>
    public async Task<List<string>> GetblobNamesAsync(string containerName, string prefix)
    {
        // Validate the parameters
        ArgumentNullException.ThrowIfNull(containerName, nameof(containerName));
        ArgumentNullException.ThrowIfNull(prefix, nameof(prefix));

        // Create a blob container client for the given container name
        var blobContainerClient = _storageClientProvider.CreateBlobContainerClient(containerName);

        // Create a list to store the results
        var results = new List<string>();

        // Iterate over the blobs in the container with the given prefix and add their names to the list
        await foreach (var blobItemName in GetBlobsAsync(blobContainerClient, prefix))
        {
            results.Add(blobItemName);
        };

        static async IAsyncEnumerable<string> GetBlobsAsync(BlobContainerClient blobContainerClient, string prefix)
        {
            await foreach (BlobItem blobItem in blobContainerClient.GetBlobsAsync(prefix: prefix))
            {
                yield return blobItem.Name;
            }
        }

        // Return the list of blob names
        return results;
    }

    /// <summary>
    /// A method that reads a blob file and deserializes it into a ForexExchangeRateModel object
    /// </summary>
    /// <param name="containerName"></param>
    /// <param name="blobName"></param>
    /// <returns></returns>
    public async Task<ForexExchangeRateModel> ReadBlobFileAsync(string containerName, string blobName)
    {
        // Create a blob container client and a blob client for the given container and blob names
        var blobContainerClient = _storageClientProvider.CreateBlobContainerClient(containerName);

        var blobClient = blobContainerClient.GetBlobClient(blobName);

        // Open a stream to read the blob content and deserialize it into a ForexExchangeRateModel object and empty object if the blob is empty or null
        using Stream stream = await blobClient.OpenReadAsync(_readOptions);

        var result = await JsonSerializer.DeserializeAsync<ForexExchangeRateModel>(stream, _jsonSerializerOptions) ?? new ForexExchangeRateModel();

        return result;
    }

    public async Task<bool> UploadJsonConetentToBlobAsync(string containerName, string jsonToUpload)
    {
        // Validate the input parameters
        if (string.IsNullOrEmpty(containerName))
        {
            throw new ArgumentNullException(nameof(containerName));
        }

        if (string.IsNullOrEmpty(jsonToUpload))
        {
            throw new ArgumentNullException(nameof(jsonToUpload));
        }

        // Create a blob container client
        var blobContainerClient = _storageClientProvider.CreateBlobContainerClient(containerName);

        // Create the container if it does not exist
        await blobContainerClient.CreateIfNotExistsAsync();

        string fileName = $"{DateTime.UtcNow:yyyy-MM-dd-HH-mm-ss}.json";
        // Create a blob client 
        var blobClient = blobContainerClient.GetBlobClient(fileName);

        // Convert the json string to a byte array
        var bytes = Encoding.UTF8.GetBytes(jsonToUpload);

        // Use a using statement to dispose the stream after use
        using var stream = new MemoryStream(bytes);

        // Upload the stream to the blob, overwriting any existing content
        var result = await blobClient.UploadAsync(stream);

        // Return true if the upload was successful
        return result != null;

    }
}
