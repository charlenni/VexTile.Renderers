using SkiaSharp;
using VexTile.Common.Interfaces;
using VexTile.Renderer.Mapbox.Extensions;
using VexTile.Style.Mapbox;

namespace VexTile.Renderer.Mapbox;

public class MapboxBackgroundPaint : MapboxBasePaint
{
    public MapboxBackgroundPaint(ITileStyle style, Func<string, SKImage> spriteFactory)
    {
        var mapboxStyle = (MapboxTileStyle)style;

        // Background has only properties in Paint, no in Layout
        var paint = mapboxStyle.Paint;

        var brush = new MapboxPaint(mapboxStyle.Name);

        brush.SetFixStyle(SKPaintStyle.Fill);
        brush.SetFixColor(new SKColor(0, 0, 0, 255));
        brush.SetFixOpacity(1);

        // background-color
        //   Optional color. Defaults to #000000. Disabled by background-pattern. Transitionable.
        //   The color with which the background will be drawn.
        if (paint?.BackgroundColor != null)
        {
            if (paint.BackgroundColor.Stops != null && paint.BackgroundColor.Stops.Count > 0)
            {
                brush.SetVariableColor((context) => paint.BackgroundColor.Evaluate(context.Zoom).ToSKColor());
            }
            else
            {
                brush.SetFixColor(paint.BackgroundColor.SingleVal?.ToSKColor() ?? SKColor.Empty);
            }
        }

        // background-pattern
        //   Optional string. Interval.
        //   Name of image in sprite to use for drawing image background. For seamless patterns, 
        //   image width and height must be a factor of two (2, 4, 8, …, 512). Note that 
        //   zoom -dependent expressions will be evaluated only at integer zoom levels.
        if (paint?.BackgroundPattern != null)
        {
            if (!string.IsNullOrEmpty(paint?.BackgroundPattern.SingleVal) || paint?.BackgroundPattern.Stops.Count != 0)
            {
                if (paint.BackgroundPattern.Stops == null && !paint.BackgroundPattern.SingleVal.Contains("{"))
                {
                    brush.SetFixShader(spriteFactory(paint.BackgroundPattern.SingleVal).ToShader(SKShaderTileMode.Repeat, SKShaderTileMode.Repeat));
                }
                else
                {
                    brush.SetVariableShader((context) =>
                    {
                        var name = paint.BackgroundPattern.Evaluate(context.Zoom).ReplaceFields(null);

                        return spriteFactory(name).ToShader(SKShaderTileMode.Repeat, SKShaderTileMode.Repeat);
                    });
                }
            }
        }

        // background-opacity
        //   Optional number. Defaults to 1.
        //   The opacity at which the background will be drawn.
        if (paint?.BackgroundOpacity != null)
        {
            if (paint.BackgroundOpacity.Stops != null && paint.BackgroundOpacity.Stops.Count > 0)
            {
                brush.SetVariableOpacity((context) => paint.BackgroundOpacity.Evaluate(context.Zoom));
            }
            else
            {
                brush.SetFixOpacity(paint.BackgroundOpacity.SingleVal);
            }
        }

        _paints = new List<MapboxPaint> { brush };
    }
}
