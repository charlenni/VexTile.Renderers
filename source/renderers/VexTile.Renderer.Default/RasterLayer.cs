using SkiaSharp;
using VexTile.Common.Primitives;
using VexTile.Renderer.Common.Interfaces;

namespace VexTile.Renderer.Default;

public class RasterLayer : IStyledLayer
{
    readonly SKRect _tileRect;
    readonly IPaint _paint;
    readonly SKBitmap _bitmap;

    public RasterLayer(SKRect rect, IPaint paint, SKBitmap bitmap)
    {
        _tileRect = rect;
        _paint = paint;
        _bitmap = bitmap.Copy();
    }

    public void Draw(SKCanvas canvas, EvaluationContext context)
    {
        var skPaints = _paint.CreateSKPaint(context);

        foreach (var skPaint in skPaints)
        {
            canvas.DrawBitmap(_bitmap, _tileRect, skPaint);
        }
    }
}
