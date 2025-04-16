using VexTile.Common.Interfaces;
using VexTile.Renderer.Common.Extensions;
using VexTile.Renderer.Mapbox;
using VexTile.Renderer.Picture;
using VexTile.Style.Mapbox;
using Xunit;

namespace VexTile.Styles.MapboxTests;

public class MapboxRenderTests
{
    readonly string _path = "files";
    PictureRenderer _renderer;

    public MapboxRenderTests()
    {
        var stream = File.Open(Path.Combine(_path, "osm-liberty.json"), FileMode.Open, FileAccess.Read);

        var mapboxStyleFile = MapboxStyleFileLoader.Load(stream).Result;
        
        _renderer = new PictureRenderer(mapboxStyleFile.Sources.Select(s => (ITileSource)s.Value), mapboxStyleFile.Layers, new MapboxPaintFactory(mapboxStyleFile.Sprites));
    }

    [Theory]
    [InlineData(0, 0, 0)]
    [InlineData(1, 0, 1)]
    [InlineData(2, 1, 2)]
    [InlineData(33, 22, 6)]
    [InlineData(1072, 717, 11)]
    [InlineData(8580, 5738, 14)]
    [InlineData(8580, 5733, 14)]
    [InlineData(8581, 5733, 14)]
    public async Task RenderTilePngTest(int x, int y, int z)
    {
        (var picture, var symbols) = await _renderer.Render(new NetTopologySuite.IO.VectorTiles.Tiles.Tile(x, y, z));

        Assert.True(picture != null);

        using (var stream = new FileStream(Path.Combine(_path, $"{x}-{y}-{z}.png"), FileMode.OpenOrCreate))
        {
            picture.ToPng(100).SaveTo(stream);
        }
    }

    [Theory]
    [InlineData(0, 0, 0)]
    [InlineData(1, 0, 1)]
    [InlineData(2, 1, 2)]
    [InlineData(33, 22, 6)]
    [InlineData(1072, 717, 11)]
    [InlineData(8580, 5738, 14)]
    [InlineData(8580, 5733, 14)]
    [InlineData(8581, 5733, 14)]
    public async Task RenderTileSvgTest(int x, int y, int z)
    {
        (var picture, var symbols) = await _renderer.Render(new NetTopologySuite.IO.VectorTiles.Tiles.Tile(x, y, z));

        Assert.True(picture != null);

        using (var stream = new FileStream(Path.Combine(_path, $"{x}-{y}-{z}.svg"), FileMode.OpenOrCreate))
        {
            picture.ToSvg(stream);
        }
    }
}
