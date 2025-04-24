using Mapsui;
using Mapsui.Extensions;
using Mapsui.Layers;
using Mapsui.Logging;
using Mapsui.Manipulations;
using Mapsui.Rendering.Skia.Cache;
using Mapsui.Rendering.Skia.SkiaStyles;
using Mapsui.Styles;
using SkiaSharp;

namespace VextTile.Control.Mapsui;

public class RenderedTileStyleRenderer : ISkiaStyleRenderer
{
    SKRect _tileRect = new SKRect(0, 0, 512, 512);
    SKPoint _tileInformationText = new SKPoint(20, 40);
    SKFont _tileInformationFont = new SKFont(SKTypeface.Default, 16);
    SKPaint _tileInformationPaint = new SKPaint { Style = SKPaintStyle.Stroke, StrokeWidth = 2, Color = SKColors.Red };

    public bool Draw(SKCanvas canvas, Viewport viewport, ILayer layer, IFeature feature, IStyle style, RenderService renderService, long iteration)
    {
        try
        {
            var renderedTileFeature = feature as RenderedTileFeature;
            var renderedTile = renderedTileFeature?.RenderedTile;

            if (renderedTile == null)
                return false;

            var opacity = (float)(layer.Opacity * style.Opacity);

            if (style is not RenderedTileStyle renderedTileStyle)
                throw new ArgumentException("Excepted a RenderedTileStyle in the RenderedTileStyleRenderer");

            var extent = feature.Extent;

            if (extent == null)
                return false;

            canvas.Save();

            var scale = CreateMatrix(canvas, viewport, extent);
            var context = new VexTile.Common.Primitives.EvaluationContext(ResolutionToZoomLevel(viewport.Resolution), 1f / scale, (float)viewport.Rotation);

            canvas.ClipRect(_tileRect);

            foreach (var pair in renderedTile.RenderedLayers)
            {
                pair.Value.Draw(canvas, context);
            }

            if (renderedTileStyle.TileInformation != null)
            {
                DrawTileInformation(canvas, renderedTile, renderedTileStyle);
            }

            canvas.Restore();
        }
        catch (Exception ex)
        {
            Logger.Log(LogLevel.Error, ex.Message, ex);
        }

        return true;
    }

    private float CreateMatrix(SKCanvas canvas, Viewport viewport, MRect extent)
    {
        var destinationTopLeft = viewport.WorldToScreen(extent.Left, extent.Top);
        var destinationTopRight = viewport.WorldToScreen(extent.Right, extent.Top);

        var dx = destinationTopRight.X - destinationTopLeft.X;
        var dy = destinationTopRight.Y - destinationTopLeft.Y;

        var scale = (float)Math.Sqrt(dx * dx + dy * dy) / 512f;

        canvas.Translate((float)destinationTopLeft.X, (float)destinationTopLeft.Y);
        if (viewport.IsRotated())
            canvas.RotateDegrees((float)viewport.Rotation);
        canvas.Scale(scale);

        return scale;
    }

    private void DrawTileInformation(SKCanvas canvas, VexTile.Renderer.Common.Interfaces.IRenderedTile renderedTile, RenderedTileStyle renderedTileStyle)
    {
        _tileInformationPaint.Color = renderedTileStyle.TileInformation.Color;

        if (renderedTileStyle.TileInformation.Border)
        {
            _tileInformationPaint.StrokeWidth = renderedTileStyle.TileInformation.BorderSize;
            canvas.DrawRect(_tileRect, _tileInformationPaint);
        }
        if (renderedTileStyle.TileInformation.Text)
        {
            _tileInformationFont.Size = renderedTileStyle.TileInformation.TextSize;
            canvas.DrawText($"Tile {renderedTile.Tile.ToString()}", _tileInformationText, SKTextAlign.Left, _tileInformationFont, _tileInformationPaint);
        }
    }

    private static MPoint RoundToPixel(ScreenPosition point)
    {
        return new MPoint(
            (float)Math.Round(point.X),
            (float)Math.Round(point.Y));
    }

    private static SKRect RoundToPixel(SKRect boundingBox)
    {
        return new SKRect(
            (float)Math.Round(boundingBox.Left),
            (float)Math.Round(Math.Min(boundingBox.Top, boundingBox.Bottom)),
            (float)Math.Round(boundingBox.Right),
            (float)Math.Round(Math.Max(boundingBox.Top, boundingBox.Bottom)));
    }

    private static float ResolutionToZoomLevel(double resolution)
    {
        double initialResolution = 78271.51696402031; // With 512px tiles
        return (float)Math.Log(initialResolution / resolution, 2);
    }
}
