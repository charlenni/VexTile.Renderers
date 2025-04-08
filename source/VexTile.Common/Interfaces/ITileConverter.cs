using NetTopologySuite.IO.VectorTiles;
using NetTopologySuite.IO.VectorTiles.Tiles;

namespace VexTile.Common.Interfaces;

public interface ITileConverter
{
    /// <summary>
    /// Convert tile
    /// </summary>
    /// <param name="tile">Tile to load and convert</param>
    /// <param name="data">Data, if there is already one</param>
    /// <returns>Vector tile</returns>
    Task<VectorTile?> Convert(Tile tile, byte[]? data = null);
}
