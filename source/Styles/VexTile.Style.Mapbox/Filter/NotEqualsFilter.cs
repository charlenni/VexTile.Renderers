using NetTopologySuite.Features;
using VexTile.Common.Extensions;

namespace VexTile.Style.Mapbox.Filter;

public class NotEqualsFilter : Filter
{
    public string Key { get; }
    public object Value { get; }

    public NotEqualsFilter(string key, object value)
    {
        Key = key;
        Value = value;
    }

    public override bool Evaluate(IFeature feature)
    {
        return feature != null && !feature.Attributes.ContainsKeyValue(Key, Value);
    }
}
