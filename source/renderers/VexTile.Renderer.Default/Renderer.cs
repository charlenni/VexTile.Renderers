using NetTopologySuite.Features;
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
/// to a ReneredTile
/// </summary>
public class Renderer
{
    static SKRect _tileRect = new SKRect(0, 0, 512, 512);

    IEnumerable<ITileSource> _sources;
    IEnumerable<ITileStyle> _styles;
    Dictionary<ITileStyle, IPaint> _paints;
    ISymbolFactory _symbolFactory;

    /// <summary>
    /// Create renderer
    /// </summary>
    /// <param name="sources">Tile sources to use for data</param>
    /// <param name="styles">Tile styles to use to render </param>
    /// <param name="paintFactory">Factory to create paints from tile styles</param>
    /// <param name="symbolFactory">Factory to create symbols from tile styles and features</param>
    public Renderer(IEnumerable<ITileSource> sources, IEnumerable<ITileStyle> styles, IPaintFactory paintFactory, ISymbolFactory symbolFactory)
    {
        _sources = sources;
        _styles = styles;
        _symbolFactory = symbolFactory;

        _paints = new Dictionary<ITileStyle, IPaint>(styles.Count());

        // Create for each style a IPaint, which then creates a SKPaint for a given evaluation context
        foreach (var style in styles)
        {
            _paints.Add(style, paintFactory.CreatePaint(style));
        }
    }

    public async Task<IRenderedTile> Render(Tile tile)
    {
        var rawTiles = new Dictionary<string, object>();

        // Get tiles from all sources
        foreach (var source in _sources)
        {
            byte[]? binaryTileData = null;

            if (source.MinZoom <= tile.Zoom && source.MaxZoom >= tile.Zoom)
            {
                binaryTileData = await source.DataSource.GetTileAsync(tile);
            }

            if (binaryTileData == null)
            {
                continue;
            }

            switch (source.SourceType)
            {
                case SourceType.Raster:
                    rawTiles.Add(source.Name, binaryTileData);
                    break;
                case SourceType.Vector:
                    var tileData = await source.TileConverter.Convert(tile, binaryTileData);
                    rawTiles.Add(source.Name, tileData);
                    break;
            }
        }

        var renderedTile = new RenderedTile(tile);
        var context = new EvaluationContext(tile.Zoom);

        // Create rendered tile style after style
        foreach (var style in _styles)
        {
            if (!IsVisible(tile.Zoom, style))
            {
                continue;
            }

            switch (style.StyleType)
            {
                case StyleType.Background:
                    RenderAsBackground(renderedTile, context, style, _paints[style]);
                    break;
                case StyleType.Raster:
                    if (rawTiles[style.Source] != null)
                    {
                        RenderTileAsRaster(renderedTile, context, (byte[])rawTiles[style.Source], style, _paints[style]);
                    }
                    break;
                case StyleType.Fill:
                    if (rawTiles[style.Source] != null)
                    {
                        RenderTilePartAsVectorFill(renderedTile, context, (VectorTile)rawTiles[style.Source], style, _paints[style]);
                    }
                    break;
                case StyleType.Line:
                    if (rawTiles[style.Source] != null)
                    {
                        RenderTilePartAsVectorLine(renderedTile, context, (VectorTile)rawTiles[style.Source], style, _paints[style]);
                    }
                    break;
                case StyleType.Symbol:
                case StyleType.FillExtrusion:
                    break;
                default:
                    throw new NotImplementedException($"Style with type '{style.StyleType}' is unknown");
            }
        }

        // Draw symbols in revers order, because last style layer is the top most layer
        foreach (var style in _styles.Reverse())
        {
            if (style.StyleType != StyleType.Symbol)
            {
                continue;
            }

            if (!IsVisible(tile.Zoom, style))
            {
                continue;
            }

            if (rawTiles[style.Source] != null)
            {
                RenderTilePartAsSymbol(renderedTile, tile, context, (VectorTile)rawTiles[style.Source], style, _paints[style], _symbolFactory);
            }
        }

        return renderedTile;
    }

    private static void RenderAsBackground(IRenderedTile renderedTile, EvaluationContext context, ITileStyle style, IPaint paint)
    {
        renderedTile.RenderedLayers.Add(style.Name, new BackgroundLayer(_tileRect, paint));
    }

    private static void RenderTileAsRaster(IRenderedTile renderedTile, EvaluationContext context, byte[] data, ITileStyle style, IPaint paint)
    {
        if (data == null || data.Length == 0)
        {
            throw new ArgumentException("Image data is empty", nameof(data));
        }

        using var bitmap = SKBitmap.Decode(data);

        if (bitmap == null)
        {
            throw new Exception("Not possible to decode image");
        }

        renderedTile.RenderedLayers.Add(style.Name, new RasterLayer(_tileRect, paint, bitmap));
    }

    private static void RenderTilePartAsVectorFill(IRenderedTile renderedTile, EvaluationContext context, VectorTile data, ITileStyle style, IPaint paint)
    {
        var layer = data.Layers.Where(l => l.Name == style.SourceLayer)?.FirstOrDefault();

        if (!ExtractFeatures(data, style, out var features))
        {
            return;
        }

        var paths = new List<SKPath>(features!.Count());

        // Draw features that belong to a fill style (draw path by path)
        foreach (var feature in features!)
        {
            var path = feature.ToSKPath();

            paths.Add(path);
        }

        renderedTile.RenderedLayers.Add(style.Name, new VectorLayer(paths, true, paint));
    }

    private static void RenderTilePartAsVectorLine(IRenderedTile renderedTile, EvaluationContext context, VectorTile data, ITileStyle style, IPaint paint)
    {
        if (!ExtractFeatures(data, style, out var features))
        {
            return;
        }

        var path = new SKPath();

        // Draw features that belong to a line style (add path by path and draw them at the end together)
        foreach (var feature in features!)
        {
            path.AddPath(feature.ToSKPath());
        }

        renderedTile.RenderedLayers.Add(style.Name, new VectorLayer([path], false, paint));
    }

    private static void RenderTilePartAsSymbol(IRenderedTile renderedTile, Tile tile, EvaluationContext context, VectorTile data, ITileStyle style, IPaint paint, ISymbolFactory symbolFactory)
    {
        var symbols = new List<ISymbol>();

        if (!ExtractFeatures(data, style, out var features))
        {
            return;
        }

        foreach (var feature in features!)
        {
            var symbol = symbolFactory.CreateSymbol(tile, style, context, feature);

            if (symbol != null)
            {
                symbols.Add(symbol);
            }
        }

        renderedTile.RenderedSymbols.Add(style.Name, new SymbolLayer(symbols));
    }

    private static bool IsVisible(int zoom, ITileStyle style)
    {
        if (!style.Visible)
        {
            return false;
        }
        // Unset zoom values are per default -1. So, if zoom value is bigger than -1 it is set by style file.
        if (style.MinZoom > -1 && style.MinZoom > zoom)
        {
            return false;
        }
        if (style.MaxZoom > -1 && style.MaxZoom <= zoom)
        {
            return false;
        }

        return true;
    }

    private static bool ExtractFeatures(VectorTile data, ITileStyle style, out IEnumerable<IFeature>? features)
    {
        var layer = data.Layers.Where(l => l.Name == style.SourceLayer)?.FirstOrDefault();

        if (layer == null)
        {
            features = null;
            return false;
        }

        features = layer.Features.Where((f) => style.Filter.Evaluate(f));

        if (features == null || features.Count() == 0)
        {
            return false;
        }

        return true;
    }
}
