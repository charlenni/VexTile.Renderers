using BenchmarkDotNet.Attributes;
using NetTopologySuite.IO.VectorTiles.Tiles;
using VexTile.Common.Interfaces;
using VexTile.Renderer.Mapbox;
using VexTile.Style.Mapbox;

namespace VexTile.Benchmarks;

[MemoryDiagnoser]
public class BenchmarksMapboxRenderer
{
    readonly string _path = "files";

    Renderer.Picture.Renderer? _renderer;
    List<Tile> _tiles = new List<Tile> { new Tile(1, 0, 1), new Tile(33, 22, 6), new Tile(1072, 717, 11), new Tile(8580, 5738, 14), new Tile(8581, 5738, 14) };

    [GlobalSetup]
    public void Setup()
    {
        var stream = File.Open(Path.Combine(_path, "osm-liberty.json"), FileMode.Open, FileAccess.Read);
        var mapboxStyleFile = MapboxStyleFileLoader.Load(stream).Result;

        _renderer = new Renderer.Picture.Renderer(mapboxStyleFile.Sources.Select(s => (ITileSource)s.Value), mapboxStyleFile.Layers, new MapboxPaintFactory(mapboxStyleFile.Sprites), new MapboxSymbolFactory(mapboxStyleFile.Sprites));
    }

    [Benchmark]
    [Arguments(0)]
    [Arguments(1)]
    [Arguments(2)]
    [Arguments(3)]
    public void RenderTileAsSvg(int i)
    {
        _renderer?.Render(_tiles[i])
            .ConfigureAwait(false)
            .GetAwaiter()
            .GetResult();
    }

    [Benchmark]
    [Arguments(0)]
    [Arguments(1)]
    [Arguments(2)]
    [Arguments(3)]
    public void RenderTileAsPng(int i)
    {
        _renderer?.Render(_tiles[i])
            .ConfigureAwait(false)
            .GetAwaiter()
            .GetResult();
    }
}
