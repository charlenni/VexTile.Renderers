using SkiaSharp;
using VexTile.Common.Interfaces;
using VexTile.Style.Mapbox;

namespace VexTile.Renderer.Mapbox;

public class MapboxRasterPaint : MapboxBasePaint
{
    public MapboxRasterPaint(ITileStyle style, Func<string, SKImage> spriteFactory)
    {
        var mapboxStyle = (MapboxTileStyle)style;

        // Raster has only properties in Paint, no in Layout
        var paint = mapboxStyle.Paint;

        var brush = new MapboxPaint(mapboxStyle.Id);

        brush.SetFixOpacity(1);

        // raster-opacity
        //   Optional number. Defaults to 1.
        //   The opacity at which the image will be drawn.
        if (paint?.RasterOpacity != null)
        {
            if (paint.RasterOpacity.Stops != null)
            {
                brush.SetVariableOpacity((context) => paint.RasterOpacity.Evaluate(context.Zoom));
            }
            else
            {
                brush.SetFixOpacity(paint.RasterOpacity.SingleVal);
            }
        }

        // raster-hue-rotate
        //   Optional number. Units in degrees. Defaults to 0.
        //   Rotates hues around the color wheel.

        // raster-brightness-min
        //   Optional number.Defaults to 0.
        //   Increase or reduce the brightness of the image. The value is the minimum brightness.

        // raster-brightness-max
        //   Optional number. Defaults to 1.
        //   Increase or reduce the brightness of the image. The value is the maximum brightness.

        // raster-saturation
        //   Optional number.Defaults to 0.
        //   Increase or reduce the saturation of the image.

        // raster-contrast
        //   Optional number. Defaults to 0.
        //   Increase or reduce the contrast of the image.

        // raster-fade-duration
        //   Optional number.Units in milliseconds.Defaults to 300.
        //   Fade duration when a new tile is added.

        _paints = new List<MapboxPaint>() { brush };
    }
}
