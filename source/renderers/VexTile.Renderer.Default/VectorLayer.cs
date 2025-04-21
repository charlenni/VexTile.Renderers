using SkiaSharp;
using VexTile.Common.Primitives;
using VexTile.Renderer.Common.Interfaces;

namespace VexTile.Renderer.Picture;

public class VectorLayer : IStyledLayer
{
    readonly IEnumerable<SKPath> _paths;
    readonly bool _clip;
    readonly IPaint _paint;

    public VectorLayer(IEnumerable<SKPath> paths, bool clip, IPaint paint)
    {
        _paths = paths;
        _clip = clip;
        _paint = paint;
    }

    public void Draw(SKCanvas canvas, EvaluationContext context)
    {
        var skPaints = _paint.CreateSKPaint(context);

        // Draw features that belong to a fill style (draw path by path)
        foreach (var path in _paths!)
        {
            canvas.Save();
            if (_clip) canvas.ClipPath(path);

            foreach (var skPaint in skPaints)
            {
                canvas.DrawPath(path, skPaint);
            }

            canvas.Restore();
        }
    }
}
