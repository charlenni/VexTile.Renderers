using VexTile.Common.Enums;

namespace VexTile.Common.Interfaces
{
    public interface ITileSource
    {
        /// <summary>
        /// Name of source
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Minimal zoom level for tile source
        /// </summary>
        int MinZoom { get; }

        /// <summary>
        /// Maximal zoom level for tile source
        /// </summary>
        int MaxZoom { get; }

        /// <summary>
        /// Type of this source
        /// </summary>
        SourceType SourceType { get; }

        /// <summary>
        /// TileDataSource used for this tile source
        /// </summary>
        ITileDataSource DataSource { get; }

        /// <summary>
        /// TileConverter used for this tile source
        /// </summary>
        ITileConverter TileConverter { get; }
    }
}
