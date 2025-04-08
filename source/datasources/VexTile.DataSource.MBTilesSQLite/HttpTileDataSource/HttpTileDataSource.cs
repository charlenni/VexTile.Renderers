using BruTile.Predefined;
using BruTile.Web;
using NetTopologySuite.IO.VectorTiles.Tiles;
using VexTile.Common.Interfaces;

namespace VexTile.TileDataSource.HttpTileDataSource
{
    public class HttpTileDataSource : ITileDataSource
    {
        HttpTileSource _httpTileSource;

        public HttpTileDataSource(string[] sources, int minZoom, int maxZoom) 
        {
            _httpTileSource = new HttpTileSource(new GlobalSphericalMercator(minZoom, maxZoom), sources[0]);
        }

        public Task<byte[]?> GetTileAsync(Tile tile)
        {
            return _httpTileSource.GetTileAsync(new BruTile.TileInfo { Index = new BruTile.TileIndex(tile.X, tile.Y, tile.Zoom) });
        }
    }
}
