using NetTopologySuite.Features;
using VexTile.Common.Interfaces;

namespace VexTile.Style.Mapbox.Filter;

public abstract class Filter : IFilter
{
    public abstract bool Evaluate(IFeature feature);
}
