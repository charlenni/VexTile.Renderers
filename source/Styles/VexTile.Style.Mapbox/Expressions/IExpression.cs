using VexTile.Common.Primitives;

namespace VexTile.Style.Mapbox.Expressions;

public interface IExpression
{
    object? Evaluate(EvaluationContext ctx);

    object? PossibleOutputs();
}
