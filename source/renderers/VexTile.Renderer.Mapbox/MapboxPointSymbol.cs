using NetTopologySuite.Features;
using NetTopologySuite.Geometries;
using NetTopologySuite.Index.Strtree;
using NetTopologySuite.IO.VectorTiles.Tiles;
using SkiaSharp;
using VexTile.Common.Primitives;
using VexTile.Renderer.Common.Interfaces;
using VexTile.Style.Mapbox;

namespace VexTile.Renderer.Mapbox;

public class MapboxPointSymbol : MapboxSymbol
{
    MapboxIconPointSymbol? _iconPointSymbol;
    MapboxTextPointSymbol? _textPointSymbol;
    bool _drawIconWithoutText;
    bool _drawTextWithoutIcon;

    public MapboxPointSymbol(Tile tile, Point point, MapboxTileStyle style, Func<string, SKImage> spriteFactory, EvaluationContext context, IFeature feature) : base(tile)
    {
        Point = point;

        try
        {
            _iconPointSymbol = new MapboxIconPointSymbol(tile, point, style, spriteFactory, context, feature);
        }
        catch (Exception e)
        {
            _iconPointSymbol = null;
        }
        try
        {
            _textPointSymbol = new MapboxTextPointSymbol(tile, point, style, context, feature);
        }
        catch (Exception e)
        {
            _textPointSymbol = null;
        }

        _drawIconWithoutText = style.Layout.TextOptional;
        _drawTextWithoutIcon = style.Layout.IconOptional;
    }

    /// <summary>
    /// Point where symbol is placed in tile coordinates
    /// </summary>
    public Point Point { get; }

    public bool HasIcon => _iconPointSymbol != null;

    public bool HasText => _textPointSymbol != null;

    public void Draw(SKCanvas canvas, EvaluationContext context, ref STRtree<ISymbol> tree)
    {
        bool spaceForIconAvailable = _iconPointSymbol?.CheckForSpace(canvas, context, tree) ?? false;
        bool spaceForTextAvailable = _textPointSymbol?.CheckForSpace(canvas, context, tree) ?? false;

        if (spaceForIconAvailable && (spaceForTextAvailable || _drawIconWithoutText) && HasIcon)
        {
            _iconPointSymbol?.Draw(canvas, context, ref tree);
        }

        if (spaceForTextAvailable && (spaceForIconAvailable || _drawTextWithoutIcon) && HasText)
        {
            _textPointSymbol?.Draw(canvas, context, ref tree);
        }
    }
}
