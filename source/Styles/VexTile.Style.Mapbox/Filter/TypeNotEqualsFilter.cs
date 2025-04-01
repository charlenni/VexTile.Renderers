﻿using NetTopologySuite.Features;
using VexTile.Common.Enums;

namespace VexTile.Style.Mapbox.Filter;

public class TypeNotEqualsFilter : Filter
{
    public string Type { get; }

    public TypeNotEqualsFilter(GeometryType type)
    {
        Type = type.ToString();
    }

    public override bool Evaluate(IFeature feature)
    {
        return feature != null && !feature.Geometry.GeometryType.Equals(Type);
    }
}
