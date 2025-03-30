using NetTopologySuite.Features;

namespace VexTile.Common.Filter;

public class ExpressionFilter : Filter
{
    public override bool Evaluate(IFeature feature)
    {
        return false;
    }
}
