using NetTopologySuite.Features;
using VexTile.Common.Interfaces;

namespace VexTile.Renderer.Common.Interfaces;

public interface ISymbolFactory
{
    ISymbol CreateSymbol(ITileStyle style, IFeature feature);
}
