using SkiaSharp;
using VexTile.Common.Interfaces;
using VexTile.Common.Primitives;
using VexTile.Style.Mapbox;

namespace VexTile.Renderer.Mapbox;

public class MapboxBackgroundPaint
{
    IEnumerable<MapboxPaint> _paint;
    EvaluationContext? _lastContext;
    IEnumerable<SKPaint>? _lastPaints;

    public MapboxBackgroundPaint(ITileStyle style)
    {
        // Background has only properties in Paint, no in Layout
        var paint = ((MapboxTileStyle)style).Paint;
        var mapboxStyle = (MapboxTileStyle)style;

        var brush = new MapboxPaint(mapboxStyle.Id);

        brush.SetFixStyle(SKPaintStyle.Fill);
        brush.SetFixColor(new SKColor(0, 0, 0, 255));
        brush.SetFixOpacity(1);

        // background-color
        //   Optional color. Defaults to #000000. Disabled by background-pattern. Transitionable.
        //   The color with which the background will be drawn.
        if (paint.BackgroundColor != null)
        {
            if (paint.BackgroundColor.Stops != null)
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
        if (paint.BackgroundPattern != null)
        {
            if (paint.BackgroundPattern.Stops == null && !paint.BackgroundPattern.SingleVal.Contains("{"))
            {
                var sprite = spriteAtlas.GetSprite(paint.BackgroundPattern.SingleVal);
                if (sprite != null)
                    brush.SetFixShader(sprite.ToSKImage().ToShader(SKShaderTileMode.Repeat, SKShaderTileMode.Repeat));
            }
            else
            {
                brush.SetVariableShader((context) =>
                {
                    var name = ReplaceFields(paint.BackgroundPattern.Evaluate(context.Zoom), null);

                    var sprite = spriteAtlas.GetSprite(name);
                    if (sprite != null)
                    {
                        return sprite.ToSKImage().ToShader(SKShaderTileMode.Repeat, SKShaderTileMode.Repeat);
                    }
                    else
                    {
                        // Log information
                        Logging.Logger.Log(Logging.LogLevel.Information, $"Fill pattern {name} not found");
                        // No sprite found
                        return null;
                    }
                });
            }
        }

        // background-opacity
        //   Optional number. Defaults to 1.
        //   The opacity at which the background will be drawn.
        if (paint?.BackgroundOpacity != null)
        {
            if (paint.BackgroundOpacity.Stops != null)
            {
                brush.SetVariableOpacity((context) => paint.BackgroundOpacity.Evaluate(context.Zoom));
            }
            else
            {
                brush.SetFixOpacity(paint.BackgroundOpacity.SingleVal);
            }
        }

        _paint = new List<MapboxPaint> { brush };
    }

    public IEnumerable<SKPaint> CreateOrUpdate(ITileStyle style, EvaluationContext context)
    {
        if (_lastContext != null && context.Equals(_lastContext))
            return _lastPaints;

        _lastContext = context;
        _lastPaints = _lastPaints;

        return _lastPaints;
    }
}
