using NetTopologySuite.IO.VectorTiles;
using NetTopologySuite.IO.VectorTiles.Tiles;
using SkiaSharp;
using VexTile.Common.Enums;
using VexTile.Common.Interfaces;

namespace VexTile.Renderer.Bitmap;

public class Renderer
{
    public static async Task<SKBitmap> Render(Tile tile, IEnumerable<ITileSource> sources, IEnumerable<ITileStyle> styles)
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

        // Draw tiles data style after style
        foreach (var style in styles)
        {
            if (!IsVisible(tile.Zoom, style))
                continue;

            switch (style.Type)
            {
                case "background":
                    RenderAsBackground(canvas, style);
                    break;
                case "raster":
                    RenderTileAsRaster(canvas, (byte[])tiles[style.Source], style);
                    break;
                case "fill":
                case "line":
                    if (tiles[style.Source] != null)
                        RenderTileAsVector(canvas, (VectorTile)tiles[style.Source], style);
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

    private static void RenderAsBackground(SKCanvas canvas, ITileStyle style)
    {

    }

    private static void RenderTileAsRaster(SKCanvas canvas, byte[] data, ITileStyle style)
    {

    }

    private static void RenderTileAsVector(SKCanvas canvas, VectorTile data, ITileStyle style)
    {
        var features = data.Layers.Where(l => l.Name == style.SourceLayer)
            .First()
            .Features
            .Where(f => style.Filter.Evaluate(f));

        if (features == null || features.Count() == 0)
            return;

        // Draw features that belong to this style
        foreach (var feature in features)
        {
            foreach (var paint in style.Paints)
            {
                canvas.DrawPath(feature.ToSKPath(), paint.ToSKPaint());
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
