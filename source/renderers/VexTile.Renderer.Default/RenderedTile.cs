using NetTopologySuite.IO.VectorTiles.Tiles;
using SkiaSharp;
using System.Data;
using VexTile.Common.Primitives;
using VexTile.Renderer.Common.Interfaces;

namespace VexTile.Renderer.Picture;

public class RenderedTile : IRenderedTile
{
    public RenderedTile(Tile tile)
    {
        Tile = tile;
        RenderedLayers = new Dictionary<string, IStyledLayer>();
        RenderedSymbols = new Dictionary<string, ISymbolLayer>();
    }

    public Tile Tile { get; private set; }

    public IDictionary<string, IStyledLayer> RenderedLayers { get; private set; }

    public IDictionary<string, ISymbolLayer> RenderedSymbols { get; private set; }

    public void Draw(SKCanvas canvas, EvaluationContext context)
    {
        foreach (KeyValuePair<string, IStyledLayer> pair in RenderedLayers)
        {
            var layerName = pair.Key;
            var layer = pair.Value;

            layer.Draw(canvas, context);
        }
    }
}
