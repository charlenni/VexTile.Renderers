using NetTopologySuite.IO.VectorTiles;
using NetTopologySuite.IO.VectorTiles.Tiles;
using SkiaSharp;
using VexTile.Common.Enums;
using VexTile.Common.Interfaces;
using VexTile.Common.Primitives;
using VexTile.Renderer.Common.Interfaces;

namespace VexTile.Renderer.Bitmap;

public class Renderer
{
    public static async Task<SKBitmap> Render(Tile tile, IEnumerable<ITileSource> sources, IEnumerable<ITileStyle> styles, IPaintFactory paintFactory)
    {
        var bitmap = new SKBitmap(512, 512);
        var canvas = new SKCanvas(bitmap);
        var tiles = new Dictionary<string, object>();

        // Get tiles from all sources
        foreach (var source in sources)
        {
            var binaryTileData = await source.DataSource.GetTileAsync(tile);

            if (binaryTileData == null)
                continue;

            switch (source.Type)
            {
                case SourceType.Raster:
                    tiles.Add(source.Name, binaryTileData);
                    break;
                case SourceType.Vector:
                    var tileData = source.TileConverter.Convert(tile, binaryTileData);
                    tiles.Add(source.Name, tileData);
                    break;
            }
        }

        var context = new EvaluationContext(tile.Zoom);

        // Draw tiles data style after style
        foreach (var style in styles)
        {
            if (!IsVisible(tile.Zoom, style))
                continue;

            switch (style.Type)
            {
                case "background":
                    RenderAsBackground(canvas, context, style, paintFactory);
                    break;
                case "raster":
                    RenderTileAsRaster(canvas, context, (byte[])tiles[style.Source], style, paintFactory);
                    break;
                case "fill":
                case "line":
                    if (tiles[style.Source] != null)
                        RenderTileAsVector(canvas, context, (VectorTile)tiles[style.Source], style, paintFactory);
                    break;
                default:
                    throw new NotImplementedException($"Style.Type '{style.Type}' is unknown");
            }
        }

        // Draw symbols in revers order, because last style layer is the top most layer
        foreach (var style in styles.Reverse())
        {
            if (!IsVisible(tile.Zoom, style))
                continue;
        }

        // Return ready rendered bitmap
        return bitmap;
    }

    private static void RenderAsBackground(SKCanvas canvas, EvaluationContext context, ITileStyle style, IPaintFactory paintFactory)
    {
        var paints = paintFactory.CreateOrUpdatePaint(style, context);
    }

    private static void RenderTileAsRaster(SKCanvas canvas, EvaluationContext context, byte[] data, ITileStyle style, IPaintFactory paintFactory)
    {
        var paints = paintFactory.CreateOrUpdatePaint(style, context);
    }

    private static void RenderTileAsVector(SKCanvas canvas, EvaluationContext context, VectorTile data, ITileStyle style, IPaintFactory paintFactory)
    {
        var features = data.Layers.Where(l => l.Name == style.SourceLayer)
            .First()
            .Features
            .Where(f => style.Filter.Evaluate(f));

        if (features == null || features.Count() == 0)
            return;

        var paints = paintFactory.CreateOrUpdatePaint(style, context);

        // Draw features that belong to this style
        foreach (var feature in features)
        {
            foreach (var paint in paints)
            {
                canvas.DrawPath(feature.ToSKPath(), paint);
            }
        }
    }

    private static bool IsVisible(int zoom, ITileStyle style)
    {
        if (!style.Visible)
            return false;
        if (style.MinZoom > zoom || style.MaxZoom <= zoom)
            return false; ;

        return true;
    }
}
