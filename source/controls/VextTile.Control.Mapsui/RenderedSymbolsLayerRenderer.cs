using Mapsui;
using Mapsui.Layers;
using Mapsui.Rendering.Skia.Cache;
using SkiaSharp;

namespace VextTile.Control.Mapsui;

public static class RenderedSymbolsLayerRenderer
{
    public static void Draw(SKCanvas canvas, Viewport viewport, ILayer layer, RenderService renderService)
    {
        if (layer is not RenderedSymbolsLayer renderedLayer)
        {
            return;
        }
    }
}
