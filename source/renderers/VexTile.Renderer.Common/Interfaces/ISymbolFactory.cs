using NetTopologySuite.Features;
using NetTopologySuite.IO.VectorTiles.Tiles;
using VexTile.Common.Interfaces;
using VexTile.Common.Primitives;

namespace VexTile.Renderer.Common.Interfaces;

public interface ISymbolFactory
{
    ISymbol CreateSymbol(Tile tile, ITileStyle style, EvaluationContext context, IFeature feature);
}
