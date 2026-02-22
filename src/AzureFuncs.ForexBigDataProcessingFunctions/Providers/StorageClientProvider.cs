namespace AzureFuncs.ForexBigDataProcessingFunctions.Providers;

public class StorageClientProvider(string connectionString)
{
    private readonly string _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));

    public BlobContainerClient CreateBlobContainerClient(string containerName)
    {
        return new BlobContainerClient(_connectionString, containerName);
    }
    public TableClient CreateTableClient(string tableName)
    {
        var client = new TableClient(_connectionString, tableName);

        return client;
    }
}
