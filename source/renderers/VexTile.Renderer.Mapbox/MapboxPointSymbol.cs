using VexTile.Renderer.Common.Interfaces;

namespace VexTile.Renderer.Mapbox;

public class MapboxPointSymbol : ISymbol
{
    public MapboxPointSymbol(ulong tileId)
    {
        TileId = tileId;
    }

    public ulong TileId { get; private set; }
}
