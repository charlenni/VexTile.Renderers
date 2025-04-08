using SkiaSharp;
using VexTile.Common.Interfaces;
using VexTile.Renderer.Mapbox.Extensions;
using VexTile.Style.Mapbox;

namespace VexTile.Renderer.Mapbox;

public class MapboxFillPaint : MapboxBasePaint
{
    public MapboxFillPaint(ITileStyle style, Func<string, SKImage> spriteFactory)
    {
        var mapboxStyle = (MapboxTileStyle)style;

        var layout = mapboxStyle.Layout;
        var paint = mapboxStyle.Paint;

        var hasOutline = false;

        var area = new MapboxPaint(mapboxStyle.Id);
        var line = new MapboxPaint(mapboxStyle.Id);

        // Set defaults
        area.SetFixColor(new SKColor(0, 0, 0, 255));
        area.SetFixOpacity(1);
        area.SetFixStyle(SKPaintStyle.Fill);
        line.SetFixColor(new SKColor(0, 0, 0, 255));
        line.SetFixOpacity(1);
        line.SetFixStyle(SKPaintStyle.Stroke);
        line.SetFixStrokeWidth(1);

        // If we don't have a paint, than there isn't anything that we could do
        if (paint == null)
        {
            _paints = new List<MapboxPaint>() { area, line };
            return;
        }

        // fill-color
        //   Optional color. Defaults to #000000. Disabled by fill-pattern. Exponential.
        //   The color of the filled part of this layer. This color can be specified as 
        //   rgba with an alpha component and the color's opacity will not affect the 
        //   opacity of the 1px stroke, if it is used.
        if (paint.FillColor != null)
        {
            if (paint.FillColor.Stops != null)
            {
                area.SetVariableColor((context) => mapboxStyle.Paint.FillColor.Evaluate(context.Zoom).ToSKColor());
                line.SetVariableColor((context) => mapboxStyle.Paint.FillColor.Evaluate(context.Zoom).ToSKColor());
            }
            else
            {
                area.SetFixColor((SKColor)mapboxStyle.Paint.FillColor.SingleVal.ToSKColor());
                line.SetFixColor((SKColor)mapboxStyle.Paint.FillColor.SingleVal.ToSKColor());
            }
        }

        // fill-opacity
        //   Optional number. Defaults to 1. Exponential.
        //   The opacity of the entire fill layer. In contrast to the fill-color, this 
        //   value will also affect the 1px stroke around the fill, if the stroke is used.
        if (paint.FillOpacity != null)
        {
            if (paint.FillOpacity.Stops != null)
            {
                area.SetVariableOpacity((context) => mapboxStyle.Paint.FillOpacity.Evaluate(context.Zoom));
                line.SetVariableOpacity((context) => mapboxStyle.Paint.FillOpacity.Evaluate(context.Zoom));
            }
            else
            {
                area.SetFixOpacity(mapboxStyle.Paint.FillOpacity.SingleVal);
                line.SetFixOpacity(mapboxStyle.Paint.FillOpacity.SingleVal);
            }
        }

        // fill-antialias
        //   Optional boolean. Defaults to true. Interval.
        //   Whether or not the fill should be antialiased.
        if (paint.FillAntialias != null)
        {
            if (paint.FillAntialias.Stops != null)
            {
                area.SetVariableAntialias((context) => mapboxStyle.Paint.FillAntialias.Evaluate(context.Zoom));
                line.SetVariableAntialias((context) => mapboxStyle.Paint.FillAntialias.Evaluate(context.Zoom));
            }
            else
            {
                area.SetFixAntialias(mapboxStyle.Paint?.FillAntialias.SingleVal == null ? false : (bool)mapboxStyle.Paint.FillAntialias.SingleVal);
                line.SetFixAntialias(mapboxStyle.Paint?.FillAntialias.SingleVal == null ? false : (bool)mapboxStyle.Paint.FillAntialias.SingleVal);
            }
        }

        // fill-outline-color
        //   Optional color. Disabled by fill-pattern. Requires fill-antialias = true. Exponential. 
        //   The outline color of the fill. Matches the value of fill-color if unspecified.
        if (paint.FillOutlineColor != null)
        {
            hasOutline = true;
            if (paint.FillOutlineColor.Stops != null)
            {
                line.SetVariableColor((context) => mapboxStyle.Paint.FillOutlineColor.Evaluate(context.Zoom).ToSKColor());
            }
            else
            {
                line.SetFixColor((SKColor)mapboxStyle.Paint.FillOutlineColor.SingleVal.ToSKColor());
            }
        }

        // fill-translate
        //   Optional array. Units in pixels. Defaults to 0,0. Exponential.
        //   The geometry's offset. Values are [x, y] where negatives indicate left and up, 
        //   respectively.

        // TODO: Use matrix of paint object for this

        // fill-translate-anchor
        //   Optional enum. One of map, viewport. Defaults to map. Requires fill-translate. Interval.
        //   Control whether the translation is relative to the map (north) or viewport (screen)

        // TODO: Use matrix of paint object for this

        // fill-pattern
        //   Optional string. Interval.
        //   Name of image in sprite to use for drawing image fills. For seamless patterns, 
        //   image width and height must be a factor of two (2, 4, 8, …, 512).
        if (paint.FillPattern != null)
        {
            // FillPattern needs a color. Instead no pattern is drawn.
            area.SetFixColor(SKColors.Black);

            if (paint.FillPattern.Stops == null && !paint.FillPattern.SingleVal.Contains("{"))
            {
                area.SetVariableShader((context) =>
                {
                    var name = paint.FillPattern.SingleVal;

                    return spriteFactory(name).ToShader(SKShaderTileMode.Repeat, SKShaderTileMode.Repeat, SKMatrix.CreateScale(context.Scale, context.Scale));
                });
            }
            else
            {
                area.SetVariableShader((context) =>
                {
                    var name = mapboxStyle.Paint?.FillPattern.Evaluate(context.Zoom).ReplaceFields(context.Attributes);

                    return spriteFactory(name).ToShader(SKShaderTileMode.Repeat, SKShaderTileMode.Repeat, SKMatrix.CreateScale(context.Scale, context.Scale));
                });
            }
        }

        // We only have to draw line around areas, when color is different from fill color
        if (hasOutline)
            _paints = new List<MapboxPaint>() { area, line };
        else
            _paints = new List<MapboxPaint>() { area };
    }
}
