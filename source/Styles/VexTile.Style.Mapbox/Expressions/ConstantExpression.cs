using VexTile.Common.Primitives;

namespace VexTile.Style.Mapbox.Expressions
{
    public class ConstantExpression<T> : Expression
    {
        T value;

        public ConstantExpression(T v)
        {
            value = v;
        }

        public override object? Evaluate(EvaluationContext feature)
        {
            return value;
        }

        public override object? PossibleOutputs()
        {
            return (T)new object();
        }
    }
}
