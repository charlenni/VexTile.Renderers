using NetTopologySuite.IO.VectorTiles.Tiles;

namespace VexTile.Renderer.Common.Interfaces;

public interface IRenderedTile
{
    public Tile Tile { get; }

    public IDictionary<string, IStyledLayer> RenderedLayers { get; }

    public IDictionary<string, ISymbolLayer> RenderedSymbols { get; }
}
