using NetTopologySuite.IO.VectorTiles;
using NetTopologySuite.IO.VectorTiles.Tiles;
using SkiaSharp;
using VexTile.Common.Enums;
using VexTile.Common.Interfaces;
using VexTile.Common.Primitives;
using VexTile.Renderer.Common.Extensions;
using VexTile.Renderer.Common.Interfaces;

namespace VexTile.Renderer.Picture;

/// <summary>
/// A renderer, that converts data from different tile sources with the given styles
/// to a SkiaSharp SKPicture tile
/// </summary>
public class PictureRenderer
{
    static SKRect _backgroundRect = new SKRect(0, 0, 512, 512);

    IEnumerable<ITileSource> _sources;
    IEnumerable<ITileStyle> _styles;
    Dictionary<ITileStyle, IPaint> _paints;
    
    /// <summary>
    /// Create renderer
    /// </summary>
    /// <param name="sources">Tile sources to use for data</param>
    /// <param name="styles">Tile styles to use to render </param>
    /// <param name="paintFactory">Factory to create paints from tile styles</param>
    public PictureRenderer(IEnumerable<ITileSource> sources, IEnumerable<ITileStyle> styles, IPaintFactory paintFactory)
    {
        _sources = sources;
        _styles = styles;

        _paints = new Dictionary<ITileStyle, IPaint>(styles.Count());

        // Create for each style a IPaint, which then creates a SKPaint for a given evaluation context
        foreach (var style in styles)
        {
            _paints.Add(style, paintFactory.CreatePaint(style));
        }
    }

    public async Task<SKPicture> Render(Tile tile)
    {
        var pictureRecorder = new SKPictureRecorder();
        var canvas = pictureRecorder.BeginRecording(_backgroundRect);
        var tiles = new Dictionary<string, object>();

        // Get tiles from all sources
        foreach (var source in _sources)
        {
            byte[]? binaryTileData = null;

            if (source.MinZoom <= tile.Zoom && source.MaxZoom >= tile.Zoom)
                binaryTileData = await source.DataSource.GetTileAsync(tile);

            if (binaryTileData == null)
                continue;

            switch (source.SourceType)
            {
                case SourceType.Raster:
                    tiles.Add(source.Name, binaryTileData);
                    break;
                case SourceType.Vector:
                    var tileData = await source.TileConverter.Convert(tile, binaryTileData);
                    tiles.Add(source.Name, tileData);
                    break;
            }
        }

        var context = new EvaluationContext(tile.Zoom);

        // Draw tiles data style after style
        foreach (var style in _styles)
        {
            if (!IsVisible(tile.Zoom, style))
                continue;

            switch (style.StyleType)
            {
                case "background":
                    RenderAsBackground(canvas, context, style, _paints[style]);
                    break;
                case "raster":
                    RenderTileAsRaster(canvas, context, (byte[])tiles[style.Source], style, _paints[style]);
                    break;
                case "fill":
                case "line":
                    if (tiles[style.Source] != null)
                        RenderTileAsVector(canvas, context, (VectorTile)tiles[style.Source], style, _paints[style]);
                    break;
                case "symbol":
                case "fill-extrusion":
                    break;
                default:
                    throw new NotImplementedException($"Style with type '{style.StyleType}' is unknown");
            }
        }

        // Draw symbols in revers order, because last style layer is the top most layer
        foreach (var style in _styles.Reverse())
        {
            if (!IsVisible(tile.Zoom, style))
                continue;
        }

        return pictureRecorder.EndRecording();
    }

    private static void RenderAsBackground(SKCanvas canvas, EvaluationContext context, ITileStyle style, IPaint paint)
    {
        var skPaints = paint.CreateSKPaint(context);

        foreach (var skPaint in skPaints)
            canvas.DrawRect(_backgroundRect, skPaint);
    }

    private static void RenderTileAsRaster(SKCanvas canvas, EvaluationContext context, byte[] data, ITileStyle style, IPaint paint)
    {
        if (data == null || data.Length == 0) 
            throw new ArgumentException("Image data is empty", nameof(data));

        using var bitmap = SKBitmap.Decode(data);

        if (bitmap == null) 
            throw new Exception("Not possible to decode image");

        var skPaints = paint.CreateSKPaint(context);

        foreach (var skPaint in skPaints)
        {
            canvas.DrawBitmap(bitmap, _backgroundRect, skPaint);
        }
    }

    private static void RenderTileAsVector(SKCanvas canvas, EvaluationContext context, VectorTile data, ITileStyle style, IPaint paint)
    {
        var layer = data.Layers.Where(l => l.Name == style.SourceLayer)?.FirstOrDefault();

        if (layer == null)
            return;

        var features = layer.Features.Where((f) => style.Filter.Evaluate(f));

        if (features == null || features.Count() == 0)
            return;

        var skPaints = paint.CreateSKPaint(context);

        if (style.StyleType == "fill")
        {
            // Draw features that belong to a fill style (draw path by path)
            foreach (var feature in features)
            {
                var path = feature.ToSKPath();

                canvas.Save();
                canvas.ClipPath(path);

                foreach (var skPaint in skPaints)
                    canvas.DrawPath(path, skPaint);

                canvas.Restore();
            }
        }
        else
        {
            var path = new SKPath();

            // Draw features that belong to a line style (add path by path and draw them at the end together)
            foreach (var feature in features)
                path.AddPath(feature.ToSKPath());

            foreach (var skPaint in skPaints)
                canvas.DrawPath(path, skPaint);
        }
    }

    private static bool IsVisible(int zoom, ITileStyle style)
    {
        if (!style.Visible)
            return false;
        // Unset zoom values are per default -1. So, if zoom value is bigger than -1 it is set by style file.
        if (style.MinZoom > -1 && style.MinZoom > zoom)
            return false;
        if (style.MaxZoom > -1 && style.MaxZoom <= zoom)
            return false;

        return true;
    }
}
