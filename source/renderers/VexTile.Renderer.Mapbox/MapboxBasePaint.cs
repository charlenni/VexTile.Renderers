using SkiaSharp;
using VexTile.Common.Primitives;
using VexTile.Renderer.Common.Interfaces;

namespace VexTile.Renderer.Mapbox;

public abstract class MapboxBasePaint : IPaint
{
    internal IEnumerable<MapboxPaint> _paints;
    internal EvaluationContext? _lastContext;
    internal IEnumerable<SKPaint>? _lastSKPaints;

    public IEnumerable<SKPaint> CreateSKPaint(EvaluationContext context)
    {
        if (_lastContext != null && context.Equals(_lastContext) && _lastSKPaints != null)
            return _lastSKPaints;

        _lastSKPaints = new List<SKPaint>(_paints.Count());
        _lastContext = context;

        foreach (var paint in _paints)
            ((List<SKPaint>)_lastSKPaints).Add(paint.CreateSKPaint(context));

        return _lastSKPaints;
    }
}
