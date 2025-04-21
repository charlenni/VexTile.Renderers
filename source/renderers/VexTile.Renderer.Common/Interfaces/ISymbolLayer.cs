using NetTopologySuite.Index.Strtree;
using SkiaSharp;

namespace VexTile.Renderer.Common.Interfaces;

public interface ISymbolLayer
{
    void Draw(SKCanvas canvas, STRtree<ISymbol> tree);
}
