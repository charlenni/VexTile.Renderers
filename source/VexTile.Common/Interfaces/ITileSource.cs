using System;
using System.Collections.Generic;
using System.Text;
using VexTile.Common.Enums;

namespace VexTile.Common.Interfaces
{
    public interface ITileSource
    {
        /// <summary>
        /// Name for this source
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Type of this source
        /// </summary>
        SourceType Type { get; }

        /// <summary>
        /// DataSource used for this tile source
        /// </summary>
        IDataSource DataSource { get; }

        /// <summary>
        /// TileConverter used for this tile source
        /// </summary>
        ITileConverter TileConverter { get; }
    }
}
