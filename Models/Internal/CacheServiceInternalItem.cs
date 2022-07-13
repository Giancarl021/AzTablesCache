namespace AzTablesCache.Models.Internal;

internal class CacheServiceInternalItem
{
    public CacheServiceItem Value { get; set; }
    public DateTime? Expiration { get; set; }
    public CacheServiceInternalItem(CacheServiceItem value, DateTime? expiration)
    {
        Value = value;
        Expiration = expiration;
    }
}