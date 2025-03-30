using NetTopologySuite.Features;
using VexTile.Common.Extensions;

namespace VexTile.Common.Filter;

public class HasFilter : Filter
{
    public string Key { get; }

    public HasFilter(string key)
    {
        Key = key;
    }

    public override bool Evaluate(IFeature feature)
    {
        return feature != null && feature.Attributes.ContainsKey(Key);
    }
}
