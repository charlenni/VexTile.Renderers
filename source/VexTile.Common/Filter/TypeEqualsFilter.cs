using NetTopologySuite.Features;
using NetTopologySuite.IO;

namespace VexTile.Common.Filter;

public class TypeEqualsFilter : Filter
{
    public GeometryType Type { get; }

    public TypeEqualsFilter(GeometryType type)
    {
        Type = type;
    }

    public override bool Evaluate(IFeature feature)
    {
        return feature != null && feature.Geometry.GeometryType.Equals(Type);
    }
}
