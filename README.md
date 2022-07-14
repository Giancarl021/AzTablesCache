# AzTablesCache

![](docs/assets/icon.png)

Key-Value cache system using [Azure Table Storage](https://azure.microsoft.com/services/storage/tables/) as persistence

## Asynchronous cache

This package provides a simple cache system, with wide-column support.

The data can be represented as follows:

```json
{
  "key1": {
    "column1": "value1",
    "column2": "value2",
    "column3": "value3"
  },
  "key2": {
    "column1": "value1",
    "column2": "value2",
    "column3": "value3"
  }
}
```

The difference is that the data is stored in Azure Table Storage asynchronously.

That way it is possible to use the cache with memory speed operations, but persistance when needed.

To avoid slow operations, the data will be synchronized only when calling the `Synchronize` method, with different merge options.

## Installation

You can get this package on [Nuget](https://www.nuget.org/packages/AzTablesCache/).

## Usage

### Importing

Fiirst you need to import the package using the `using` keyword:

```csharp
using AzTablesCache;
```

Then you will have access to the `CacheService` class, which is the main class of this package.

### Initialization

This class have six constructors:

```csharp
public CacheService(TableServiceClient client, string tableName, string partitionKey, TimeSpan? defaultExpirationTime);
public CacheService(TableServiceClient client, string tableName, TimeSpan? defaultExpirationTime);
public CacheService(TableServiceClient client, string tableName, string partitionKey);
public CacheService(TableServiceClient client, TimeSpan? defaultExpirationTime);
public CacheService(TableServiceClient client, string tableName);
public CacheService(TableServiceClient client);
```

Parameters:
* `client`: Azure Table Storage client, used to manage the remote cache data;
* `tableName`: Name of the table where the data will be stored. Default `CacheServiceData`;
* `partitionKey`: Partition key of the table, used to partition the data. Default `CacheService::PK`.
* `defaultExpirationTime`: Default expiration time for the cache entries. Default `null`.

### Methods

* [Get](docs/Get.md) - Get a cache item from memory;
* [Set](docs/Set.md) - Set or update a cache value on memory;
* [Has](docs/Has.md) - Checks if a cache item exists on memory;
* [Expire](docs/Expire.md) - Manually expire a cache item from memory;
* [Clear](docs/Clear.md) - Clears all memory data;
* [Synchronize](docs/Synchronize.md) - Synchronize data between memory and Azure Table Storage.