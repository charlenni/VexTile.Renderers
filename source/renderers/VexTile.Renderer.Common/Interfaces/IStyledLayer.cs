using SkiaSharp;
using VexTile.Common.Primitives;

namespace VexTile.Renderer.Common.Interfaces;

public interface IStyledLayer
{
    void Draw(SKCanvas canvas, EvaluationContext context);
}
