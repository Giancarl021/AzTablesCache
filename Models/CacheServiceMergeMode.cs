namespace AzTablesCache.Models;

/// <summary>
/// Synchronization Merging Mode for dealing with cache conflicts
/// </summary>
public enum CacheServiceMergeMode
{
    /// <summary>
    /// All remote data will be overwritten with local data.
    /// </summary>
    LocalReplace,
    /// <summary>
    /// All local data will be overwritten with remote data.
    /// </summary>
    RemoteReplace,
    /// <summary>
    /// Local data will have precedence over remote data on conflicts.
    /// </summary>
    LocalMerge,
    /// <summary>
    /// Remote data will have precedence over local data on conflicts.
    /// </summary>
    RemoteMerge
};