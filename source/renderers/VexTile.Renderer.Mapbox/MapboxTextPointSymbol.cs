using NetTopologySuite.Features;
using NetTopologySuite.Geometries;
using NetTopologySuite.Index.Strtree;
using NetTopologySuite.IO.VectorTiles.Tiles;
using SkiaSharp;
using VexTile.Common.Primitives;
using VexTile.Renderer.Common.Interfaces;
using VexTile.Style.Mapbox;

namespace VexTile.Renderer.Mapbox;

public class MapboxTextPointSymbol : MapboxSymbol
{
    public MapboxTextPointSymbol(Tile tile, Point point, MapboxTileStyle style, EvaluationContext context, IFeature feature) : base(tile)
    {
        context.Feature = feature;

        CreateCommon(style, context);
        CreateText(style, context);
    }

    /// <summary>
    /// Point where symbol is placed in tile coordinates
    /// </summary>
    public Point Point { get; }

    public bool CheckForSpace(SKCanvas canvas, EvaluationContext context, STRtree<ISymbol> tree)
    {
        // Check, if there is space for icon
        var envText = CalcEnvelope(canvas, context);

        var symbols = tree.Query(envText);

        foreach (var symbol in symbols)
        {
            if (!symbol.AllowOthers)
            {
                return false;
            }
        }

        return true;
    }

    public void Draw(SKCanvas canvas, EvaluationContext context, ref STRtree<ISymbol> tree)
    {
        Envelope? envText = null;

        DrawText(canvas, context);

        envText = CalcEnvelope(canvas, context);

        if (envText != null)
        {
            tree.Insert(envText, this);
        }
    }

    private void CreateText(MapboxTileStyle style, EvaluationContext context)
    {

    }

    private Envelope CalcEnvelope(SKCanvas canvas, EvaluationContext context)
    {
        var result = new Envelope();

        return result;
    }

    private void DrawText(SKCanvas canvas, EvaluationContext context)
    {
    }
}
