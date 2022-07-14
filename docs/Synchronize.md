# Synchronize

This method synchronizes the data between memory and Azure Table Storage.

## Signature

```csharp
public async Task Synchronize(CacheServiceMergeMode mode = CacheServiceMergeMode.LocalMerge);
```

## Parameters

### (Optional) `CacheServiceMergeMode mode`

The way the synchronization will happen. There are four modes:

| Mode            | Description                                                   | Default |
| --------------- | ------------------------------------------------------------- | ------- |
| `LocalMerge`    | Local data will have precedence over remote data on conflicts | Yes     |
| `RemoteMerge`   | Remote data will have precedence over local data on conflicts | No      |
| `LocalReplace`  | All remote data will be overwritten with local data           | No      |
| `RemoteReplace` | All local data will be overwritten with remote data           | No      |

This parameter is optional.

## Returns

A Task that will be completed when the merge finish.