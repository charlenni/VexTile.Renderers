using SkiaSharp;
using VexTile.Common.Interfaces;
using VexTile.Common.Primitives;

namespace VexTile.Renderer.Mapbox;

public class MapboxVectorPaint
{
    public static IEnumerable<SKPaint> CreateOrUpdate(ITileStyle style, EvaluationContext context)
    {
        return new List<SKPaint> { new SKPaint() };
    }
}
