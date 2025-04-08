using NetTopologySuite.IO.VectorTiles.Tiles;

namespace VexTile.Common.Interfaces;

/// <summary>
/// Data source that is used as tile source
/// </summary>
public interface ITileDataSource
{
    Task<byte[]?> GetTileAsync(Tile tile);
}
