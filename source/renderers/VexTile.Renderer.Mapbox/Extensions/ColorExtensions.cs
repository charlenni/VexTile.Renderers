using SkiaSharp;
using VexTile.Common.Primitives;

namespace VexTile.Renderer.Mapbox.Extensions;

public static class ColorExtensions
{
    public static SKColor ToSKColor(this Color color)
    {
        return new SKColor(color.R, color.G, color.B, color.A);
    }
}
