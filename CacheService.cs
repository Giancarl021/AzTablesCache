using AzTablesCache.Models;
using AzTablesCache.Models.Internal;
using Azure.Data.Tables;

namespace AzTablesCache;
/// <summary>
/// Key-Value Cache system using Azure Table Storage as persistence
/// </summary>
public class CacheService
{
    private static string DefaultTableName = "CacheServiceData";
    private static string DefaultPartitionKey = "CacheService::PK";
    private static string ExpirationNullValue = "CacheService::Expiration::NULL";
    private readonly Dictionary<string, CacheServiceInternalItem> _data;
    private readonly TableServiceClient _client;
    private readonly string _tableName;
    private readonly string _partitionKey;
    private readonly TimeSpan? _defaultExpirationTime;
    /// <summary>
    /// Initializes a new instance of the <see cref="CacheService"/> class
    /// </summary>
    /// <param name="client">Azure Table Storage Client from Nuget package Azure.Data.Tables</param>
    /// <param name="tableName">The name of the Azure Table in which the cache will be stored</param>
    /// <param name="partitionKey">The partition key of the cache data</param>
    /// <param name="defaultExpirationTime">The default expiration time for keys without expiration time set</param>
    public CacheService(TableServiceClient client, string tableName, string partitionKey, TimeSpan? defaultExpirationTime)
    {
        _data = new Dictionary<string, CacheServiceInternalItem>();
        _client = client;
        _tableName = tableName;
        _partitionKey = partitionKey;
        _defaultExpirationTime = defaultExpirationTime;
    }
    /// <summary>
    /// Initializes a new instance of the <see cref="CacheService"/> class. The default expiration time is set to null
    /// </summary>
    /// <param name="client">Azure Table Storage Client from Nuget package Azure.Data.Tables</param>
    /// <param name="tableName">The name of the Azure Table in which the cache will be stored</param>
    /// <param name="partitionKey">The partition key of the cache data</param>
    public CacheService(TableServiceClient client, string tableName, string partitionKey): this(client, tableName, partitionKey, null) {}
    /// <summary>
    /// Initializes a new instance of the <see cref="CacheService"/> class. The partition key is set to an internal default
    /// </summary>
    /// <param name="client">Azure Table Storage Client from Nuget package Azure.Data.Tables</param>
    /// <param name="tableName">The name of the Azure Table in which the cache will be stored</param>
    /// <param name="defaultExpirationTime">The default expiration time for keys without expiration time set</param>
    public CacheService(TableServiceClient client, string tableName, TimeSpan? defaultExpirationTime): this(client, tableName, DefaultPartitionKey, defaultExpirationTime) {}
    /// <summary>
    /// Initializes a new instance of the <see cref="CacheService"/> class. The table name and partition key are set to internal defaults
    /// </summary>
    /// <param name="client">Azure Table Storage Client from Nuget package Azure.Data.Tables</param>
    /// <param name="defaultExpirationTime">The default expiration time for keys without expiration time set</param>
    public CacheService(TableServiceClient client, TimeSpan? defaultExpirationTime): this(client, DefaultTableName, DefaultPartitionKey, defaultExpirationTime) {}
    /// <summary>
    /// Initializes a new instance of the <see cref="CacheService"/> class. The partition key and is set to internal default and the default expiration time to null
    /// </summary>
    /// <param name="client">Azure Table Storage Client from Nuget package Azure.Data.Tables</param>
    /// <param name="tableName">The name of the Azure Table in which the cache will be stored</param>
    public CacheService(TableServiceClient client, string tableName): this(client, tableName, DefaultPartitionKey, null) {}
    /// <summary>
    /// Initializes a new instance of the <see cref="CacheService"/> class. The table name and partition key are set to internal defaults and the default expiration time to null
    /// </summary>
    /// <param name="client">Azure Table Storage Client from Nuget package Azure.Data.Tables</param>
    public CacheService(TableServiceClient client): this(client, DefaultTableName, DefaultPartitionKey, null) {}
    /// <summary>
    /// Synchronize the local cache with the Azure Table Storage
    /// </summary>
    /// <param name="mode">A enum value defining the behavior of the synchronization</param>
    /// <returns>A Task that will be completed when the synchronization finish</returns>
    public async Task Synchronize(CacheServiceMergeMode mode = CacheServiceMergeMode.LocalMerge)
    {
        switch (mode)
        {
            case CacheServiceMergeMode.LocalReplace:
                await ForEachRemoteItem(
                    (table, item) => table.DeleteEntityAsync(item.PartitionKey, item.RowKey)
                );
                
                await ForEachLocalItem(
                    (table, key, value) => table.AddEntityAsync(SerializeValue(key, value))
                );
                break;
            case CacheServiceMergeMode.RemoteReplace:
                _data.Clear();
                // Sync
                break;
            case CacheServiceMergeMode.LocalMerge:

                break;
            case CacheServiceMergeMode.RemoteMerge:
                break;
        }
    }
    /// <summary>
    /// Clear the local cache data
    /// </summary>
    public void Clear() => _data.Clear();
    /// <summary>
    /// Expire a key in the local cache
    /// </summary>
    /// <param name="key">The key of the cache value</param>
    public void Expire(string key)
    {
        _data.Remove(key);
    }
    /// <summary>
    /// Check if a key exist in the local cache
    /// </summary>
    /// <param name="key">The key of the cache value</param>
    /// <returns>The check result</returns>
    public bool Has(string key)
    {
        if (!_data.ContainsKey(key)) return false;

        var item = _data[key];

        if (item.Expiration == null) return true;

        if (DateTime.Now >= item.Expiration.GetValueOrDefault())
        {
            Expire(key);
            return false;
        }

        return true;
    }
    /// <summary>
    /// Get a value from the local cache
    /// </summary>
    /// <param name="key">The key of the cache value</param>
    /// <typeparam name="T">The type of the requested item, must be a subtype of <see cref="CacheServiceItem"></see></typeparam>
    /// <returns>The requested cache value</returns>
    /// <exception cref="CacheItemNotFoundException"></exception>
    public T Get<T>(string key) where T : CacheServiceItem
    {
        if (!Has(key))
            throw new CacheItemNotFoundException($"Cache item with key \"{key}\" not found");
        
        var item = _data[key];

        return (T)item.Value;
    }
    /// <summary>
    /// Set or update a value in the local cache
    /// </summary>
    /// <param name="key">The key of the cache value</param>
    /// <param name="value">The value of the cache value</param>
    /// <param name="expiration">The expiration time of the cache value</param>
    /// <typeparam name="T">The type of the requested item, must be a subtype of <see cref="CacheServiceItem"></see></typeparam>
    public void Set<T>(string key, T value, TimeSpan? expiration) where T : CacheServiceItem
    {
        var item = new CacheServiceInternalItem(
            value: value,
            expiration: expiration != null ?
                DateTime.Now.Add(expiration.GetValueOrDefault()) :
                null
        );

        _data[key] = item;
    }
    /// <summary>
    /// Set or update a value in the local cache with the default expiration time
    /// </summary>
    /// <param name="key">The key of the cache value</param>
    /// <param name="value">The value of the cache value</param>
    /// <typeparam name="T">The type of the requested item, must be a subtype of <see cref="CacheServiceItem"></see></typeparam>
    public void Set<T>(string key, T value) where T : CacheServiceItem => Set(key, value, _defaultExpirationTime);
    private async Task<TableClient> GetTableClient()
    {
        TableClient tableClient = _client.GetTableClient(_tableName);
        await tableClient.CreateIfNotExistsAsync();

        return tableClient;
    }
    private TableEntity SerializeValue(string key, CacheServiceInternalItem item)
    {
        var entity = new TableEntity(_partitionKey, key);
        
        entity.Add("Expiration", item.Expiration?.ToString("o") ?? ExpirationNullValue);

        var keys = item.Value.FlattenKeys();
        
        foreach (var (name, data) in keys)
        {
            if (name == null || data == null) continue;
            entity.Add(name, data);
        }

        return entity;
    }
    private async Task ForEachRemoteItem(Func<TableClient, TableEntity, Task> predicate)
    {
        TableClient table = await GetTableClient();

        var iterator = table
            .QueryAsync<TableEntity>(_ => true)
            .AsPages();
        
        await foreach (var items in iterator)
        {
            if (items == null) continue;
            foreach (var item in items.Values)
            {
                if (item == null) continue;
                await predicate(table, item);
            }
        }
    }
    private async Task ForEachLocalItem(Func<TableClient, string, CacheServiceInternalItem, Task> predicate)
    {
        TableClient table = await GetTableClient();

        foreach (var item in _data)
        {
            await predicate(table, item.Key, item.Value);
        }
    }
}