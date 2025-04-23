using BruTile;
using Mapsui;

namespace VextTile.Control.Mapsui;

public interface IRenderedTileSource : ITileSource
{
    Task<IFeature?> GetTileAsync(TileInfo tileInfo);
}
