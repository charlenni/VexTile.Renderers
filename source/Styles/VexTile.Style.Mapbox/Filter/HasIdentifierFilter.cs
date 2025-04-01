using NetTopologySuite.Features;

namespace VexTile.Style.Mapbox.Filter;

public class HasIdentifierFilter : Filter
{
    public HasIdentifierFilter()
    {
    }

    public override bool Evaluate(IFeature feature)
    {
        return feature != null && !string.IsNullOrWhiteSpace(feature.Attributes["id"].ToString());
    }
}
