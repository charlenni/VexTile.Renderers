using NetTopologySuite.Features;

namespace VexTile.Common.Filter;

public interface IFilter
{
    bool Evaluate(IFeature feature);
}
