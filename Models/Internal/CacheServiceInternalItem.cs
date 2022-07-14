namespace AzTablesCache.Models.Internal;

internal class CacheServiceInternalItem
{
    public Dictionary<string, string> Value { get; set; }
    public DateTime? Expiration { get; set; }
    public CacheServiceInternalItem(Dictionary<string, string> value, DateTime? expiration)
    {
        Value = value;
        Expiration = expiration;
    }
}