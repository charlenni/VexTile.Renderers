using VexTile.Common.Primitives;

namespace VexTile.Style.Mapbox.Expressions;

/// <summary>
/// Class holding StoppedString data
/// </summary>
public class StoppedEnum<T> : IExpression where T : struct, Enum
{
    public float Base { get; set; } = 1f;

    public IList<KeyValuePair<float, T>> Stops { get; set; } = [];

    public T SingleVal { get; set; } = default(T);

    /// <summary>
    /// Calculate the correct string for a stopped function
    /// No StoppsType needed, because strings couldn't interpolated :)
    /// </summary>
    /// <param name="contextZoom">Zoom factor for calculation </param>
    /// <returns>Value for this stopp respecting resolution factor and type</returns>
    public T Evaluate(float? contextZoom)
    {
        // Are there no stopps in array
        if (Stops.Count == 0)
            return SingleVal;

        float zoom = contextZoom ?? 0f;

        var lastZoom = Stops[0].Key;
        var lastValue = Stops[0].Value;

        if (lastZoom > zoom)
            return lastValue;

        for (int i = 1; i < Stops.Count; i++)
        {
            var nextZoom = Stops[i].Key;
            var nextValue = Stops[i].Value;

            if (zoom == nextZoom)
                return nextValue;

            if (lastZoom <= zoom && zoom < nextZoom)
            {
                return lastValue;
            }

            lastZoom = nextZoom;
            lastValue = nextValue;
        }

        return lastValue;
    }

    public object? Evaluate(EvaluationContext ctx)
    {
        return Evaluate(ctx.Zoom);
    }

    public object? PossibleOutputs()
    {
        throw new System.NotImplementedException();
    }
}
