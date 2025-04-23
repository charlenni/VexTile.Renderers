using Mapsui;
using Mapsui.Layers;
using Mapsui.Projections;
using VexTile.Renderer.Common.Interfaces;

namespace VextTile.Control.Mapsui;

public class RenderedTileFeature : BaseFeature
{
    public RenderedTileFeature(IRenderedTile renderedTile)
    {
        RenderedTile = renderedTile;
        (var left, var top) = SphericalMercator.FromLonLat(RenderedTile.Tile.Left, RenderedTile.Tile.Top);
        (var right, var bottom) = SphericalMercator.FromLonLat(RenderedTile.Tile.Right, RenderedTile.Tile.Bottom);
        Extent = new MRect(left, top, right, bottom);
    }

    public IRenderedTile RenderedTile { get; private set; }

    public override MRect? Extent { get; }

    public override void CoordinateVisitor(Action<double, double, CoordinateSetter> visit)
    {
        throw new NotImplementedException();
    }

    public override object Clone()
    {
        throw new NotImplementedException();
    }
}
