using NetTopologySuite.Index.Strtree;
using SkiaSharp;
using VexTile.Renderer.Common.Interfaces;

namespace VexTile.Renderer.Default;

public class SymbolLayer : ISymbolLayer
{
    readonly IEnumerable<ISymbol> _symbols;

    public SymbolLayer(IEnumerable<ISymbol> symbols)
    {
        _symbols = symbols;
    }

    public void Draw(SKCanvas canvas, STRtree<ISymbol> tree)
    {
        foreach (var symbol in _symbols)
        {
        }
    }
}
