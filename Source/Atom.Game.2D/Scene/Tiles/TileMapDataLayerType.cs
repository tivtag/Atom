// <copyright file="TileMapDataLayerType.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Scene.Tiles.TileMapDataLayerType enumeration.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Scene.Tiles
{
    /// <summary>
    /// Enumerates the different default tile-map layer types.
    /// </summary>
    public enum TileMapDataLayerType
    {
        /// <summary>
        /// A normal layer which contains graphical data.
        /// </summary>
        Normal,

        /// <summary>
        /// An action layer which contains action data, such as collision information.
        /// </summary>
        Action
    }
}
