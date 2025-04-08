using SkiaSharp;
using VexTile.Common.Primitives;

namespace VexTile.Renderer.Common.Interfaces;

/// <summary>
/// Interface for a style file independent class, that could produce one or more SKPaint
/// </summary>
public interface IPaint
{
    IEnumerable<SKPaint> CreateSKPaint(EvaluationContext context);
}
