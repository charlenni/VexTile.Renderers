using VexTile.Common.Interfaces;

namespace VexTile.Renderer.Common.Interfaces;

/// <summary>
/// Interface to create a style file independent class for each style
/// </summary>
public interface IPaintFactory
{
    IPaint CreatePaint(ITileStyle style);
}
