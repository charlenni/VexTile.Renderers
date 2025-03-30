using NetTopologySuite.Features;

namespace VexTile.Common.Primitives;

/// <summary>
/// Context for which the style should be evaluated
/// </summary>
public class EvaluationContext
{
    public float? Zoom { get; set; }

    public float Scale { get; set; }

    public float Rotation { get; set; }

    public AttributesTable? Attributes { get; set; }

    public EvaluationContext(float? zoom, float scale = 1, float rotation = 0, AttributesTable? attributes = null)
    {
        Zoom = zoom;
        Scale = scale;
        Rotation = rotation;
        Attributes = attributes;
    }

    public override bool Equals(object? other) => other is EvaluationContext context && Equals(context);

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = (int)(Zoom ?? 0);
            hashCode = (hashCode * 587) ^ (int)Scale;
            hashCode = (hashCode * 587) ^ (int)Rotation;
            hashCode = (hashCode * 587) ^ (Attributes != null ? Attributes.GetHashCode() : 0);
            return hashCode;
        }
    }

    public bool Equals(EvaluationContext context)
    {
        return this == context || (context != null && context.Zoom == Zoom && context.Scale == Scale && ((context.Attributes == null && Attributes == null) || context.Attributes.Equals(Attributes)));
    }
}
