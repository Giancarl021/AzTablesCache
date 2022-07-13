namespace AzTablesCache.Models;

/// <summary>
/// Model of a Item used in the Cache Service
/// </summary>
public abstract class CacheServiceItem
{
    /// <summary>
    /// Serialize all the internal data of the instance into a dictionary of string-string key-value pairs. It is highly recommended to override this method
    /// </summary>
    /// <returns>A dictionary containing all the instance relevant data</returns>
    public virtual Dictionary<string, string> FlattenKeys()
    {
        var dict = new Dictionary<string, string>();

        var props = this.GetType()
            .GetProperties()
            .Where(p => p.CanRead);

        foreach (var prop in props)
        {
            var value = prop.GetValue(this);
            if (value == null) continue;
            dict.Add(prop.Name, value?.ToString() ?? "");
        }

        return dict;
    }
}