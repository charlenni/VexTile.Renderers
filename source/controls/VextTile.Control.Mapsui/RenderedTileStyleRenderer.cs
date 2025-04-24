using ExCSS;
using Mapsui;
using Mapsui.Extensions;
using Mapsui.Layers;
using Mapsui.Logging;
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

            var context = new VexTile.Common.Primitives.EvaluationContext(ResolutionToZoomLevel(viewport.Resolution));

            canvas.Save();

            if (viewport.IsRotated())
            {
                var priorMatrix = canvas.TotalMatrix;

                var matrix = CreateRotationMatrix(viewport, extent, priorMatrix);

                canvas.SetMatrix(matrix);

                var destination = new SKRect(0.0f, 0.0f, (float)extent.Width, (float)extent.Height);

                foreach (var pair in renderedTile.RenderedLayers)
                {
                    pair.Value.Draw(canvas, context);
                }

                canvas.SetMatrix(priorMatrix);
            }
            else
            {
                var destination = RoundToPixel(WorldToScreen(viewport, extent));

                var scaleX = destination.Width / 512;
                var scaleY = destination.Height / 512;

                var matrix = canvas.TotalMatrix
                    .PreConcat(SKMatrix.CreateTranslation(destination.Left, destination.Top))
                    .PreConcat(SKMatrix.CreateScale(scaleX, scaleY));

                canvas.SetMatrix(matrix);
                canvas.ClipRect(_tileRect);

                foreach (var pair in renderedTile.RenderedLayers)
                {
                    pair.Value.Draw(canvas, context);
                }

                if (renderedTileStyle.TileInformation != null)
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
            }

            canvas.Restore();
        }
        catch (Exception ex)
        {
            Logger.Log(LogLevel.Error, ex.Message, ex);
        }

        return true;
    }

    private static SKMatrix CreateRotationMatrix(Viewport viewport, MRect rect, SKMatrix priorMatrix)
    {
        // The front-end sets up the canvas with a matrix based on screen scaling (e.g. retina).
        // We need to retain that effect by combining our matrix with the incoming matrix.

        // We'll create four matrices in addition to the incoming matrix. They perform the
        // zoom scale, focal point offset, user rotation and finally, centering in the screen.

        var userRotation = SKMatrix.CreateRotationDegrees((float)viewport.Rotation);
        var focalPointOffset = SKMatrix.CreateTranslation(
            (float)(rect.Left - viewport.CenterX),
            (float)(viewport.CenterY - rect.Top));
        var zoomScale = SKMatrix.CreateScale((float)(1.0 / viewport.Resolution), (float)(1.0 / viewport.Resolution));
        var centerInScreen = SKMatrix.CreateTranslation((float)(viewport.Width / 2.0), (float)(viewport.Height / 2.0));

        // We'll concatenate them like so: incomingMatrix * centerInScreen * userRotation * zoomScale * focalPointOffset

        var matrix = SKMatrix.Concat(zoomScale, focalPointOffset);
        matrix = SKMatrix.Concat(userRotation, matrix);
        matrix = SKMatrix.Concat(centerInScreen, matrix);
        matrix = SKMatrix.Concat(priorMatrix, matrix);

        return matrix;
    }

    private static SKRect WorldToScreen(Viewport viewport, MRect rect)
    {
        var first = viewport.WorldToScreen(rect.Min.X, rect.Min.Y);
        var second = viewport.WorldToScreen(rect.Max.X, rect.Max.Y);
        return new SKRect
        (
            (float)Math.Min(first.X, second.X),
            (float)Math.Min(first.Y, second.Y),
            (float)Math.Max(first.X, second.X),
            (float)Math.Max(first.Y, second.Y)
        );
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
