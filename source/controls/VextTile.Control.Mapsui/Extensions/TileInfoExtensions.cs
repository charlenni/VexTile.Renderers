using BruTile;
using NetTopologySuite.IO.VectorTiles.Tiles;

namespace VextTile.Control.Mapsui.Extensions;

public static class TileInfoExtensions
{
    public static Tile ToTile(this TileInfo tileInfo)
    {
        return new Tile(tileInfo.Index.Col, tileInfo.Index.Row, tileInfo.Index.Level);
    }
}
