using BruTile;
using BruTile.Predefined;
using Mapsui;
using VexTile.Renderer.Default;
using VexTile.Renderer.Mapbox;
using VexTile.Style.Mapbox;
using VextTile.Control.Mapsui.Extensions;

namespace VextTile.Control.Mapsui;

public class MapboxRenderedTileSource : IRenderedTileSource
{
    Renderer _renderer;

    public MapboxRenderedTileSource(Stream stream)
    {
        var mapboxStyleFile = MapboxStyleFileLoader.Load(stream).Result;

        var minZoom = 0;
        var maxZoom = 19;

        _renderer = new Renderer(mapboxStyleFile.Sources.Select(s => (VexTile.Common.Interfaces.ITileSource)s.Value), mapboxStyleFile.Layers, new MapboxPaintFactory(mapboxStyleFile.Sprites), new MapboxSymbolFactory(mapboxStyleFile.Sprites));

        Schema = new GlobalSphericalMercator512(YAxis.OSM, minZoom, maxZoom);
        Name = mapboxStyleFile.Name;
        Attribution = new Attribution(string.Empty);
    }

    public ITileSchema Schema { get; private set; }

    public string Name { get; private set; }

    public Attribution Attribution { get; private set; }

    public async Task<IFeature?> GetTileAsync(TileInfo tileInfo)
    {
        var renderedTile = await _renderer.Render(tileInfo.ToTile());
        var feature = new RenderedTileFeature(renderedTile);
        return feature;
    }
}
