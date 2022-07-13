[Serializable]
/// <summary>
/// Represents errors that occur when trying to access a inexistent cache item.
/// </summary>
/// 
public class CacheItemNotFoundException : System.Exception
{
    public CacheItemNotFoundException() { }
    public CacheItemNotFoundException(string key) : base($"Cache item with key \"{key}\" not found") { }
    public CacheItemNotFoundException(string key, System.Exception inner) : base($"Cache item with key \"{key}\" not found", inner) { }
    protected CacheItemNotFoundException(
        System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}