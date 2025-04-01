using VexTile.Common.Primitives;

namespace VexTile.Style.Mapbox.Expressions;

public class Expression : IExpression
{
    public virtual object? Evaluate(EvaluationContext ctx)
    {
        return null;
    }

    public virtual object? PossibleOutputs()
    {
        return null;
    }
}
