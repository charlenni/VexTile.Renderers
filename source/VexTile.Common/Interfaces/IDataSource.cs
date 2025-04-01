using NetTopologySuite.IO.VectorTiles.Tiles;

namespace VexTile.Common.Interfaces;

public interface IDataSource
{
    Task<byte[]?> GetTileAsync(Tile tile);
}
