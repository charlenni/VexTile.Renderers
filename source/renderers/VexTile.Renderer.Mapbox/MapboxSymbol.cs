using NetTopologySuite.IO.VectorTiles.Tiles;
using VexTile.Common.Primitives;
using VexTile.Renderer.Common.Interfaces;
using VexTile.Style.Mapbox;
using VexTile.Style.Mapbox.Enums;

namespace VexTile.Renderer.Mapbox;

public class MapboxSymbol : ISymbol
{
    public MapboxSymbol(Tile tile)
    {
        Tile = tile;
    }

    /// <summary>
    /// Tile Id to which this symbol belongs
    /// </summary>
    public Tile Tile { get; }

    public double SortOrder { get; internal set; }

    internal void CreateCommon(MapboxTileStyle style, EvaluationContext context)
    {
        switch (style.Layout.SymbolZOrder)
        {
            case SymbolZOrder.Source:
                SortOrder = (double)(style.Layout.SymbolSortKey?.Evaluate(context) ?? 0.0);
                break;
            case SymbolZOrder.ViewportY:
                if (style.Layout.IconAllowOverlap || style.Layout.TextAllowOverlap || style.Layout.IconIgnorePlacement || style.Layout.TextIgnorePlacement)
                {
                    SortOrder = Tile.Y * 512.0 + context.Feature.Geometry.Centroid.Y;
                }
                break;
            case SymbolZOrder.Auto:
                if (style.Layout.SymbolSortKey != null)
                {
                    SortOrder = (double)(style.Layout.SymbolSortKey?.Evaluate(context) ?? 0.0);
                }
                else if (style.Layout.IconAllowOverlap || style.Layout.TextAllowOverlap || !style.Layout.IconIgnorePlacement || !style.Layout.TextIgnorePlacement)
                {
                    SortOrder = Tile.Y * 512.0 + context.Feature.Geometry.Centroid.Y;
                }
                else
                {
                    SortOrder = 0;
                }
                break;
        }
    }
}
