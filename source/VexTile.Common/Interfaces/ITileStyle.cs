﻿using System.Reflection;
using VexTile.Common.Primitives;

namespace VexTile.Common.Interfaces;

public interface ITileStyle
{
    /// <summary>
    /// Type of this style
    /// </summary>
    string Type { get; }

    /// <summary>
    /// Minimal zoom from which this style layer is used
    /// </summary>
    int MinZoom { get; }

    /// <summary>
    /// Maximal zoom up to which this style layer is used
    /// </summary>
    int MaxZoom { get; }

    /// <summary>
    /// Is style layer visible
    /// </summary>
    bool Visible { get; }

    /// <summary>
    /// Filter to get all features for which this style is valid
    /// </summary>
    IFilter Filter { get; }

    /// <summary>
    /// Name of source this style belongs to 
    /// </summary>
    string Source { get; }

    /// <summary>
    /// Name of source layer this style belongs to 
    /// </summary>
    string SourceLayer { get; }

    /// <summary>
    /// Paint to use to draw the features
    /// </summary>
    IEnumerable<IVectorPaint> Paints { get; }

    /// <summary>
    /// Update the paints in the style with the new values in context
    /// </summary>
    /// <param name="context"></param>
    void Update(EvaluationContext context);
}
