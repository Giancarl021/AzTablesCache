# AzTablesCache
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

## Documentation and Source-Code

You can find the full documentation and the source-code of this package [here](https://github.com/Giancarl021/AzTablesCache).