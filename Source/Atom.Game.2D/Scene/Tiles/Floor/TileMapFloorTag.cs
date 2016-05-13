// <copyright file="TileMapFloorTag.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Scene.Tiles.TileMapFloorTag class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Scene.Tiles
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Defines a tag which can be applied to a <see cref="TileMapFloor"/>
    /// which stores the <see cref="IFloorDrawable"/>s that are currently visible on that specific floor.
    /// </summary>
    public class TileMapFloorTag
    {
        /// <summary>
        /// Gets the list of <see cref="IDrawable"/>s that are visible on the <see cref="TileMapFloor"/>.
        /// </summary>
        /// <value>The reference of the actual list stored in this TileMapFloorTag.</value>
        public List<IFloorDrawable> VisibleDrawables
        {
            get 
            {
                return visibleDrawables;
            }
        }

        /// <summary>
        /// The list of <see cref="IFloorDrawable"/>s that are visible on the TileMapFloor.
        /// </summary>
        private readonly List<IFloorDrawable> visibleDrawables = new List<IFloorDrawable>();
    }
}
