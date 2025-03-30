using NetTopologySuite.Features;

namespace VexTile.Common.Filter;

public abstract class Filter : IFilter
{
    public abstract bool Evaluate(IFeature feature);
}
