using VexTile.Style.Mapbox;
using VexTile.Renderer.Bitmap;
using Xunit;
using VexTile.Renderer.Mapbox;
using VexTile.Common.Interfaces;

namespace VexTile.Styles.MapboxTests;

public class MapboxRenderTests
{
    readonly string _path = "files";
    Renderer.Bitmap.Renderer _renderer;

    public MapboxRenderTests()
    {
        var stream = File.Open(Path.Combine(_path, "osm-liberty.json"), FileMode.Open, FileAccess.Read);

        var mapboxStyleFile = MapboxStyleFileLoader.Load(stream).Result;
        
        _renderer = new Renderer.Bitmap.Renderer(mapboxStyleFile.Sources.Select(s => (ITileSource)s.Value), mapboxStyleFile.Layers, new MapboxPaintFactory(mapboxStyleFile.Sprites));
    }

    [Theory]
    [InlineData(0, 0, 0)]
    [InlineData(1, 1, 1)]
    [InlineData(2, 1, 2)]
    [InlineData(33, 22, 6)]
    [InlineData(1072, 717, 11)]
    [InlineData(8580, 5738, 14)]
    [InlineData(8580, 5733, 14)]
    [InlineData(8581, 5733, 14)]
    public async Task RenderTileTest1(int x, int y, int z)
    {
        var data = await _renderer.Render(new NetTopologySuite.IO.VectorTiles.Tiles.Tile(x, y, z));

        Assert.True(data != null);
        Assert.True(!data.IsEmpty);
        //Assert.True(data.Size == 244799);

        using (var stream = new FileStream(Path.Combine(_path, $"{x}-{y}-{z}.png"), FileMode.OpenOrCreate))
            data.SaveTo(stream);
    }

    [Fact]
    public async Task RenderTileTest2()
    {
        var data = await _renderer.Render(new NetTopologySuite.IO.VectorTiles.Tiles.Tile(0, 0, 0));

        Assert.True(data != null);
        Assert.True(!data.IsEmpty);
        //Assert.True(data.Size == 126561);

        using (var stream = new FileStream(Path.Combine(_path, "0-0-0.png"), FileMode.OpenOrCreate))
            data.SaveTo(stream);
    }

}
