using NetTopologySuite.Features;

namespace VexTile.Common.Filter;

public class NotHasIdentifierFilter : Filter
{
    public NotHasIdentifierFilter()
    {
    }

    public override bool Evaluate(IFeature feature)
    {
        return feature != null && string.IsNullOrWhiteSpace(feature.Attributes["id"].ToString());
    }
}
