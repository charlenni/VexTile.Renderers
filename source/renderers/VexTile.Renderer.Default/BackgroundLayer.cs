using SkiaSharp;
using VexTile.Common.Primitives;
using VexTile.Renderer.Common.Interfaces;

namespace VexTile.Renderer.Picture;

public class BackgroundLayer : IStyledLayer
{
    readonly SKRect _tileRect;
    readonly IPaint _paint;

    public BackgroundLayer(SKRect rect, IPaint paint)
    {
        _tileRect = rect;
        _paint = paint;
    }

    public void Draw(SKCanvas canvas, EvaluationContext context)
    {
        var skPaints = _paint.CreateSKPaint(context);

        foreach (var skPaint in skPaints)
        {
            canvas.DrawRect(_tileRect, skPaint);
        }
    }
}
