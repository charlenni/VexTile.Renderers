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

    [Fact]
    public async Task RenderTileTest1()
    {
        var data = await _renderer.Render(new NetTopologySuite.IO.VectorTiles.Tiles.Tile(8580, 10649, 14));

        Assert.True(data != null);
        Assert.True(!data.IsEmpty);
        Assert.True(data.Size == 2217);

        using (var stream = new FileStream(Path.Combine(_path, "8580-10649-14.png"), FileMode.OpenOrCreate))
            data.SaveTo(stream);
    }

    [Fact]
    public async Task RenderTileTest2()
    {
        var data = await _renderer.Render(new NetTopologySuite.IO.VectorTiles.Tiles.Tile(0, 0, 0));

        Assert.True(data != null);
        Assert.True(!data.IsEmpty);
        //Assert.True(data.Size == 36138);

        using (var stream = new FileStream(Path.Combine(_path, "0-0-0.png"), FileMode.OpenOrCreate))
            data.SaveTo(stream);
    }

}
