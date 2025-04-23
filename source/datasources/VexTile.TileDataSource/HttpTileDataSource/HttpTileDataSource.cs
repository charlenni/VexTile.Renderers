using BruTile.Predefined;
using BruTile.Web;
using NetTopologySuite.IO.VectorTiles.Tiles;
using VexTile.Common.Interfaces;

namespace VexTile.TileDataSource.HttpTileDataSource
{
    public class HttpTileDataSource : ITileDataSource
    {
        HttpTileSource _httpTileSource;
        HttpClientHandler _httpHandler;
        HttpClient _httpClient;


        public HttpTileDataSource(string[] sources, int minZoom, int maxZoom) 
        {
            _httpTileSource = new HttpTileSource(new GlobalSphericalMercator(minZoom, maxZoom), sources[0]);
            _httpHandler = new HttpClientHandler { AllowAutoRedirect = true };
            _httpClient = new HttpClient(_httpHandler);
            _httpClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", $"user-agent-of-{Path.GetFileNameWithoutExtension(System.AppDomain.CurrentDomain.FriendlyName)}");
        }

        public Task<byte[]?> GetTileAsync(Tile tile)
        {
            return _httpTileSource.GetTileAsync(_httpClient, new BruTile.TileInfo { Index = new BruTile.TileIndex(tile.X, tile.Y, tile.Zoom) });
        }
    }
}
