using NetTopologySuite.Features;

namespace VexTile.Style.Mapbox.Filter;

public class NullFilter : Filter
{
    public override bool Evaluate(IFeature feature)
    {
        return true;
    }
}
