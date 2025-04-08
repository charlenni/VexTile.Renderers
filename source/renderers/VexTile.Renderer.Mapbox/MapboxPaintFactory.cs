using SkiaSharp;
using VexTile.Common.Interfaces;
using VexTile.Renderer.Common.Interfaces;
using VexTile.Style.Mapbox;

namespace VexTile.Renderer.Mapbox
{
    public class MapboxPaintFactory : IPaintFactory
    {
        Func<string, SKImage> _spriteFactory;

        public MapboxPaintFactory(MapboxSpriteFile? spriteFile) 
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

        public IPaint CreatePaint(ITileStyle style)
        {
            switch (style.StyleType)
            {
                case "background":
                    return new MapboxBackgroundPaint(style, _spriteFactory);
                case "raster":
                    return new MapboxRasterPaint(style, _spriteFactory);
                case "fill":
                    return new MapboxFillPaint(style, _spriteFactory);
                case "line":
                    return new MapboxLinePaint(style, _spriteFactory);
                case "symbol":
                    return null;
                case "fill-extrusion":
                    return null;
                default:
                    throw new NotImplementedException($"Style with type '{style.StyleType}' is unknown");
            }
        }
    }
}
