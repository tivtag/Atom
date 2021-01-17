// <copyright file="ITileMapProvider.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Scene.Tiles.ITileMapProvider interface.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Scene.Tiles
{
    /// <summary>
    /// Defines an interface which provides
    /// access to a <see cref="TileMap"/> object.
    /// </summary>
    public interface ITileMapProvider
    {
        /// <summary>
        /// Gets the <see cref="TileMap"/> object this <see cref="ITileMapProvider"/> provides.
        /// </summary>
        /// <value>
        /// The <see cref="TileMap"/> object this <see cref="ITileMapProvider"/> provides.
        /// </value>
        TileMap Map 
        { 
            get;
        }
    }
}
