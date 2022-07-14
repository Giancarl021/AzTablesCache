/// <summary>
/// Represents errors that occur when trying to access a inexistent cache item.
/// </summary>
[Serializable]
public class CacheItemNotFoundException : System.Exception
{
    /// <summary>
    /// Empty constructor
    /// </summary>
    public CacheItemNotFoundException() { }
    /// <summary>
    /// Constructor with the key of the inexistent cache item
    /// </summary>
    /// <param name="key">Key of inexistent cache item</param>
    public CacheItemNotFoundException(string key) : base($"Cache item with key \"{key}\" not found") { }
    /// <summary>
    /// Constructor with the key of the inexistent cache item and the inner exception
    /// </summary>
    /// <param name="key">Key of inexistent cache item</param>
    /// <param name="inner">Inner exception</param>
    public CacheItemNotFoundException(string key, System.Exception inner) : base($"Cache item with key \"{key}\" not found", inner) { }
    /// <summary>
    /// Constructor with the information and context
    /// </summary>
    /// <param name="info">Information about serialization</param>
    /// <param name="context">Context about data streaming</param>
    /// <returns></returns>
    protected CacheItemNotFoundException(
        System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}