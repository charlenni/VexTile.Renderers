using NetTopologySuite.Features;

namespace VexTile.Common.Filter;

public class NullFilter : Filter
{
    public override bool Evaluate(IFeature feature)
    {
        return true;
    }
}
