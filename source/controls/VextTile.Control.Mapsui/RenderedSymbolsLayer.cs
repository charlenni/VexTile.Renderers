using Mapsui;
using Mapsui.Layers;
using Mapsui.Rendering.Skia;

namespace VextTile.Control.Mapsui;

public class RenderedSymbolsLayer : BaseLayer
{
    const string customLayerRendererName = "rendered-symbols-layer";

    public RenderedSymbolsLayer(IRenderedTileSource tileSource)
    {
        MapRenderer.RegisterLayerRenderer(customLayerRendererName, RenderedSymbolsLayerRenderer.Draw);

        TileSource = tileSource;
        CustomLayerRendererName = customLayerRendererName;
    }

    public IRenderedTileSource TileSource { get; private set; }

    public override IEnumerable<IFeature> GetFeatures(MRect box, double resolution)
    {
        // TODO
        // Calc all tiles, that are inside of box
        // Load all tiles

        return null;
    }
}
