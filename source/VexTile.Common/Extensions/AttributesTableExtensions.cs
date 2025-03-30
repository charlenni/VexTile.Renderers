using NetTopologySuite.Features;

namespace VexTile.Common.Extensions;

public static class AttributesTableExtensions
{
    /// <summary>
    /// Returns true, if the tags collection contains given key
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public static bool ContainsKey(this IAttributesTable attributes, string key)
    {
        return attributes.GetNames().Contains(key);
    }

    /// <summary>
    /// Returns true if the given key-value pair is found in this tags collection.
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static bool ContainsKeyValue(this IAttributesTable attributes, string key, object value)
    {
        if (!attributes.ContainsKey(key))
            return false;

        return attributes[key].Equals(value);
    }

}
