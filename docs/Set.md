# Set

This method creates or updates a cache item in memory.

## Signatures

```csharp
public void Set(string key, Dictionary<string, string> value, TimeSpan? expiration);
public void Set(string key, Dictionary<string, string> value);
```

## Parameters

### `string key`

The unique identifier of the cache item.

This parameter is required.

### `Dictionary<string, string> value`

The values of the cache item.

This parameter is required.


### (Optional) `TimeSpan? expiration`

The lifetime of the cache item. If `null` the item will never expire, if omitted will use the `defaultExpirationTime` set in the constructor.

This parameter is optional.

## Returns

A Dictionary containing the cache item value.