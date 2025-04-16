using NetTopologySuite.IO.VectorTiles.Tiles;

namespace VexTile.Renderer.Common.Interfaces;

public interface ISymbol
{
    Tile Tile { get; }

    double SortOrder { get; }

    bool AllowOthers { get; }
}
