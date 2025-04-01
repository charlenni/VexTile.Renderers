using VexTile.Common.Primitives;

namespace VexTile.Common.Interfaces;

public interface IVectorPaint
{
    /// <summary>
    /// Creates a Paint for this ITileStyle
    /// </summary>
    /// <param name="context">Context to use to create this paint</param>
    /// <returns>Paint to use for drawing</returns>
    Paint CreatePaint(EvaluationContext context);
}
