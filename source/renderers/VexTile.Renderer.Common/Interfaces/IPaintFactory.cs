using SkiaSharp;
using VexTile.Common.Interfaces;
using VexTile.Common.Primitives;

namespace VexTile.Renderer.Common.Interfaces;

public interface IPaintFactory
{
    IEnumerable<SKPaint> CreateOrUpdatePaint(ITileStyle style, EvaluationContext context);
}
