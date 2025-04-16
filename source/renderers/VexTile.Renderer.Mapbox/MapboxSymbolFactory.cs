using NetTopologySuite.Features;
using SkiaSharp;
using VexTile.Common.Enums;
using VexTile.Common.Interfaces;
using VexTile.Common.Primitives;
using VexTile.Renderer.Common.Interfaces;
using VexTile.Style.Mapbox;

namespace VexTile.Renderer.Mapbox;

public class MapboxSymbolFactory : ISymbolFactory
{
    Func<string, SKImage> _spriteFactory;

    public MapboxSymbolFactory(MapboxSpriteFile? spriteFile)
    {
        if (spriteFile == null)
            throw new ArgumentNullException(nameof(spriteFile));

        _spriteFactory = (name) =>
        {
            var bitmap = spriteFile.Bitmap;
            var sprite = spriteFile.Sprites[name];

            if (bitmap.Native == null)
                // Convert byte array to SKImage
                bitmap.Native = SKImage.FromEncodedData(bitmap.Binary);

            return ((SKImage)bitmap.Native).Subset(new SKRectI(sprite.X, sprite.Y, sprite.X + sprite.Width, sprite.Y + sprite.Height));
        };
    }

    public ISymbol CreateSymbol(ulong tileId, ITileStyle style, EvaluationContext context, IFeature feature)
    {
        var mapboxTileStyle = (MapboxTileStyle)style;

        return mapboxTileStyle.Layout.SymbolPlacement.Evaluate(context) switch
        {
            SymbolPlacement.Point => CreatePointSymbol(tileId, mapboxTileStyle, context, feature),
            SymbolPlacement.Line => CreateLineSymbol(tileId, mapboxTileStyle, context, feature),
            SymbolPlacement.LineCenter => CreateLineCenterSymbol(tileId, mapboxTileStyle, context, feature)
        };
    }

    private static ISymbol CreatePointSymbol(ulong tileId, MapboxTileStyle style, EvaluationContext context, IFeature feature)
    {
        return new MapboxPointSymbol(tileId);
    }

    private static ISymbol CreateLineSymbol(ulong tileId, MapboxTileStyle style, EvaluationContext context, IFeature feature)
    {
        return new MapboxPointSymbol(tileId);
    }

    private static ISymbol CreateLineCenterSymbol(ulong tileId, MapboxTileStyle style, EvaluationContext context, IFeature feature)
    {
        return new MapboxPointSymbol(tileId);
    }
}
