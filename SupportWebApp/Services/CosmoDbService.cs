using Microsoft.Azure.Cosmos;
using SupportWebApp.Models;

namespace SupportWebApp.Services;

public class CosmosDbService
{
    private readonly CosmosClient _client;
    private readonly string _databaseName;
    private readonly string _containerName;
    private Container? _container;

    public CosmosDbService(string connectionString, string databaseName, string containerName)
    {
        _databaseName = databaseName;
        _containerName = containerName;

        _client = new CosmosClient(connectionString, new CosmosClientOptions
        {
            SerializerOptions = new CosmosSerializationOptions
            {
                PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase
            }
        });
    }

    // Opretter database og container, hvis de ikke findes
    public async Task InitializeAsync()
    {
        var database = await _client.CreateDatabaseIfNotExistsAsync(_databaseName);
        var container = await database.Database.CreateContainerIfNotExistsAsync(
            _containerName, "/category");
        _container = container.Container;
    }

    public async Task AddSupportMessageAsync(SupportMessage message)
    {
        if (_container is null)
            await InitializeAsync();

        await _container!.CreateItemAsync(
            message,
            new PartitionKey(message.Category.ToString()));
    }
    
    public async Task<List<SupportMessage>> GetSupportMessagesAsync()
    {
        if (_container is null)
            await InitializeAsync();

        var query = new QueryDefinition("SELECT * FROM c ORDER BY c.createdAt DESC");
        var messages = new List<SupportMessage>();

        using var iterator = _container!.GetItemQueryIterator<SupportMessage>(query);
        while (iterator.HasMoreResults)
        {
            var response = await iterator.ReadNextAsync();
            messages.AddRange(response);
        }

        return messages;
    }
}