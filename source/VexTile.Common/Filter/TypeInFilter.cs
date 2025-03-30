using NetTopologySuite.Features;
using NetTopologySuite.IO;

namespace VexTile.Common.Filter;

public class TypeInFilter : Filter
{
    public IList<GeometryType> Types { get; }

    public TypeInFilter(IEnumerable<GeometryType> types)
    {
        Types = new List<GeometryType>();

        foreach (var type in types)
            Types.Add(type);
    }

    public override bool Evaluate(IFeature feature)
    {
        if (feature == null)
            return false;

        foreach (var type in Types)
            if (feature.Geometry.GeometryType.Equals(type))
                return true;

        return false;
    }
}
