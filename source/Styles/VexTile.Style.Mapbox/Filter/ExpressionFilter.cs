using NetTopologySuite.Features;

namespace VexTile.Style.Mapbox.Filter;

public class ExpressionFilter : Filter
{
    public override bool Evaluate(IFeature feature)
    {
        return false;
    }
}
