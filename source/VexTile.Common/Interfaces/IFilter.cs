using NetTopologySuite.Features;

namespace VexTile.Common.Interfaces;

public interface IFilter
{
    bool Evaluate(IFeature feature);
}
