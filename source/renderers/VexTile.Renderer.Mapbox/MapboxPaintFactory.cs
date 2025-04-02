using SkiaSharp;
using VexTile.Common.Interfaces;
using VexTile.Common.Primitives;
using VexTile.Renderer.Common.Interfaces;

namespace VexTile.Renderer.Mapbox
{
    public class MapboxPaintFactory : IPaintFactory
    {
        public IEnumerable<SKPaint> CreateOrUpdatePaint(ITileStyle style, EvaluationContext context)
        {
            switch (style.Type)
            {
                case "background":
                    return MapboxBackgroundPaint.CreateOrUpdate(style, context);
                case "raster":
                    return MapboxRasterPaint.CreateOrUpdate(style, context);
                case "fill":
                    return MapboxVectorPaint.CreateOrUpdate(style, context);
                case "line":
                    return MapboxBackgroundPaint.CreateOrUpdate(style, context);
            }

            return new List<SKPaint> { new SKPaint() };
        }
    }
}
