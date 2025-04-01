using NetTopologySuite.Features;

namespace VexTile.Style.Mapbox.Filter;

public class IdentifierEqualsFilter : Filter
{
    public string Identifier { get; }

    public IdentifierEqualsFilter(string identifier)
    {
        Identifier = identifier;
    }

    public override bool Evaluate(IFeature feature)
    {
        return feature != null && feature.Attributes["id"].ToString() == Identifier;
    }
}
