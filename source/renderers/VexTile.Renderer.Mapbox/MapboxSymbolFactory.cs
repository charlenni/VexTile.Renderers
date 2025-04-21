using NetTopologySuite.Features;
using NetTopologySuite.IO.VectorTiles.Tiles;
using SkiaSharp;
using VexTile.Common.Enums;
using VexTile.Common.Interfaces;
using VexTile.Common.Primitives;
using VexTile.Renderer.Common.Interfaces;
using VexTile.Style.Mapbox;

namespace VexTile.Renderer.Mapbox;

public class MapboxSymbolFactory : ISymbolFactory
{
    Func<string, SKImage?> _spriteFactory;

    public MapboxSymbolFactory(MapboxSpriteFile? spriteFile)
    {
        if (spriteFile == null)
            throw new ArgumentNullException(nameof(spriteFile));

        _spriteFactory = (name) =>
        {
            if (string.IsNullOrEmpty(name) || !spriteFile.Sprites.ContainsKey(name))
            {
                return null;
            }

            var bitmap = spriteFile.Bitmap;
            var sprite = spriteFile.Sprites[name];

            if (bitmap.Native == null)
                // Convert byte array to SKImage
                bitmap.Native = SKImage.FromEncodedData(bitmap.Binary);

            return ((SKImage)bitmap.Native).Subset(new SKRectI(sprite.X, sprite.Y, sprite.X + sprite.Width, sprite.Y + sprite.Height));
        };
    }

    public ISymbol? CreateSymbol(Tile tile, ITileStyle style, EvaluationContext context, IFeature feature)
    {
        var mapboxTileStyle = (MapboxTileStyle)style;

        return mapboxTileStyle.Layout.SymbolPlacement.Evaluate(context) switch
        {
            SymbolPlacement.Point => CreatePointSymbol(tile, mapboxTileStyle, _spriteFactory, context, feature),
            SymbolPlacement.Line => CreateLineSymbol(tile, mapboxTileStyle, _spriteFactory, context, feature),
            SymbolPlacement.LineCenter => CreateLineCenterSymbol(tile, mapboxTileStyle, _spriteFactory, context, feature)
        };
    }

    private static ISymbol? CreatePointSymbol(Tile tile, MapboxTileStyle style, Func<string, SKImage> spriteFactory, EvaluationContext context, IFeature feature)
    {
        if (feature.Geometry.GeometryType != "Point" || feature.Geometry.Coordinates.Length != 1)
            throw new ArgumentException($"GeometryType of symbol with SymbolPlacement 'point' is not 'Point', but {feature.Geometry.GeometryType}'");

        var symbol = new MapboxPointSymbol(tile, feature.Geometry.Centroid, style, spriteFactory, context, feature);

        if (!symbol.HasIcon && !symbol.HasText)
        {
            return null;
        }

        return symbol;
    }

    private static ISymbol? CreateLineSymbol(Tile tile, MapboxTileStyle style, Func<string, SKImage> spriteFactory, EvaluationContext context, IFeature feature)
    {
        // TODO
        return null;
    }

    private static ISymbol? CreateLineCenterSymbol(Tile tile, MapboxTileStyle style, Func<string, SKImage> spriteFactory, EvaluationContext context, IFeature feature)
    {
        // TODO
        return null;
    }
}
