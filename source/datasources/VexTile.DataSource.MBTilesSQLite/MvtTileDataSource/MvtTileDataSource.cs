﻿using NetTopologySuite.IO.VectorTiles.Tiles;
using VexTile.Common.Interfaces;

namespace VexTile.TileDataSource.MvtTileDataSource;

public class MvtTileDataSource : ITileDataSource
{
    public MvtTileDataSource(string path = "")
    {
        if (path.Equals(""))
            path = Directory.GetCurrentDirectory();

        Path = path;
    }

    public string Path { get; }

    public async Task<byte[]?> GetTileAsync(Tile tile)
    {
        string qualifiedPath = Path
                .Replace("{x}", tile.X.ToString())
                .Replace("{y}", tile.Y.ToString())
                .Replace("{z}", tile.Zoom.ToString());

        if (!File.Exists(qualifiedPath))
            return null;

        byte[]? result = null;

        await new Task(() => new Task(() => File.ReadAllBytes(qualifiedPath)).RunSynchronously());

        return result;
    }
}
