using NetTopologySuite.Features;
using VexTile.Common.Interfaces;
using VexTile.Common.Primitives;

namespace VexTile.Renderer.Common.Interfaces;

public interface ISymbolFactory
{
    ISymbol CreateSymbol(ITileStyle style, EvaluationContext context, IFeature feature);
}
