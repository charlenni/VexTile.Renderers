using NetTopologySuite.Features;
using NetTopologySuite.IO;

namespace VexTile.Common.Filter;

public class TypeNotEqualsFilter : Filter
{
    public GeometryType Type { get; }

    public TypeNotEqualsFilter(GeometryType type)
    {
        Type = type;
    }

    public override bool Evaluate(IFeature feature)
    {
        return feature != null && !feature.Geometry.GeometryType.Equals(Type);
    }
}
