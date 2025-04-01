using NetTopologySuite.Features;
using VexTile.Common.Extensions;

namespace VexTile.Style.Mapbox.Filter;

public class NotHasFilter : Filter
{
    public string Key { get; }

    public NotHasFilter(string key)
    {
        Key = key;
    }

    public override bool Evaluate(IFeature feature)
    {
        return feature != null && !feature.Attributes.ContainsKey(Key);
    }
}
