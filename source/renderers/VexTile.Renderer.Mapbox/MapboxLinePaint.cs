using SkiaSharp;
using VexTile.Common.Interfaces;
using VexTile.Renderer.Mapbox.Extensions;
using VexTile.Style.Mapbox;

namespace VexTile.Renderer.Mapbox;

public class MapboxLinePaint : MapboxBasePaint
{
    public MapboxLinePaint(ITileStyle style, Func<string, SKImage> spriteFactory)
    {
        var mapboxStyle = (MapboxTileStyle)style;

        var layout = mapboxStyle.Layout;
        var paint = mapboxStyle.Paint;

        var line = new MapboxPaint(mapboxStyle.Id);

        // Set defaults
        line.SetFixColor(new SKColor(0, 0, 0, 255));
        line.SetFixStyle(SKPaintStyle.Stroke);
        line.SetFixStrokeWidth(1);
        line.SetFixStrokeCap(SKStrokeCap.Butt);
        line.SetFixStrokeJoin(SKStrokeJoin.Miter);

        // If we don't have a paint, than there isn't anything that we could do
        if (paint == null)
        {
            _paints = new List<MapboxPaint>() { line };
            return;
        }

        // line-cap
        //   Optional enum. One of butt, round, square. Defaults to butt. Interval.
        //   The display of line endings.
        if (layout?.LineCap != null)
        {
            switch (layout.LineCap)
            {
                case "butt":
                    line.SetFixStrokeCap(SKStrokeCap.Butt);
                    break;
                case "round":
                    line.SetFixStrokeCap(SKStrokeCap.Round);
                    break;
                case "square":
                    line.SetFixStrokeCap(SKStrokeCap.Square);
                    break;
                default:
                    line.SetFixStrokeCap(SKStrokeCap.Butt);
                    break;
            }
        }

        // line-join
        //   Optional enum. One of bevel, round, miter. Defaults to miter.
        //   The display of lines when joining.
        if (layout?.LineJoin != null)
        {
            switch (layout.LineJoin)
            {
                case "bevel":
                    line.SetFixStrokeJoin(SKStrokeJoin.Bevel);
                    break;
                case "round":
                    line.SetFixStrokeJoin(SKStrokeJoin.Round);
                    break;
                case "mitter":
                    line.SetFixStrokeJoin(SKStrokeJoin.Miter);
                    break;
                default:
                    line.SetFixStrokeJoin(SKStrokeJoin.Miter);
                    break;
            }
        }

        // line-color
        //   Optional color. Defaults to #000000. Disabled by line-pattern. Exponential.
        //   The color with which the line will be drawn.
        if (paint?.LineColor != null)
        {
            if (paint.LineColor.Stops != null)
            {
                line.SetVariableColor((context) => paint.LineColor.Evaluate(context.Zoom).ToSKColor());
            }
            else
            {
                line.SetFixColor(paint.LineColor.SingleVal?.ToSKColor() ?? SKColor.Empty);
            }
        }

        // line-width
        //   Optional number.Units in pixels.Defaults to 1. Exponential.
        //   Stroke thickness.
        if (paint?.LineWidth != null)
        {
            if (paint.LineWidth.Stops != null)
            {
                line.SetVariableStrokeWidth((context) => paint.LineWidth.Evaluate(context.Zoom));
            }
            else
            {
                line.SetFixStrokeWidth(paint.LineWidth.SingleVal);
            }
        }

        // line-opacity
        //   Optional number. Defaults to 1. Exponential.
        //   The opacity at which the line will be drawn.
        if (paint?.LineOpacity != null)
        {
            if (paint.LineOpacity.Stops != null)
            {
                line.SetVariableOpacity((context) => paint.LineOpacity.Evaluate(context.Zoom));
            }
            else
            {
                line.SetFixOpacity(paint.LineOpacity.SingleVal);
            }
        }

        // line-dasharray
        //   Optional array. Units in line widths. Disabled by line-pattern. Interval.
        //   Specifies the lengths of the alternating dashes and gaps that form the dash pattern. 
        //   The lengths are later scaled by the line width.To convert a dash length to pixels, 
        //   multiply the length by the current line width.
        if (paint?.LineDashArray != null)
        {
            if (paint.LineDashArray.Stops != null)
            {
                line.SetVariableDashArray((context) => paint.LineDashArray.Evaluate(context.Zoom));
            }
            else
            {
                line.SetFixDashArray(paint.LineDashArray.SingleVal);
            }
        }

        // line-miter-limit
        //   Optional number. Defaults to 2. Requires line-join = miter. Exponential.
        //   Used to automatically convert miter joins to bevel joins for sharp angles.

        // line-round-limit
        //   Optional number. Defaults to 1.05. Requires line-join = round. Exponential.
        //   Used to automatically convert round joins to miter joins for shallow angles.

        // line-translate
        //   Optional array. Units in pixels.Defaults to 0,0. Exponential.
        //   The geometry's offset. Values are [x, y] where negatives indicate left and up, 
        //   respectively.

        // line-translate-anchor
        //   Optional enum. One of map, viewport.Defaults to map. Requires line-translate. Interval.
        //   Control whether the translation is relative to the map (north) or viewport (screen)

        // line-gap-width
        //   Optional number.Units in pixels.Defaults to 0. Exponential.
        //   Draws a line casing outside of a line's actual path.Value indicates the width of 
        //   the inner gap.

        // line-offset
        //   Optional number. Units in pixels. Defaults to 0. Exponential.
        //   The line's offset perpendicular to its direction. Values may be positive or negative, 
        //   where positive indicates "rightwards" (if you were moving in the direction of the line) 
        //   and negative indicates "leftwards".

        // line-blur
        //   Optional number. Units in pixels.Defaults to 0. Exponential.
        //   Blur applied to the line, in pixels.

        // line-pattern
        //   Optional string. Interval.
        //   Name of image in sprite to use for drawing image lines. For seamless patterns, image 
        //   width must be a factor of two (2, 4, 8, …, 512).

        _paints = new List<MapboxPaint>() { line };
    }
}
