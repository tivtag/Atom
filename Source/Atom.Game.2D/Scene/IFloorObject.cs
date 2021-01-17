// <copyright file="IFloorObject.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Scene.IFloorObject interface.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Scene
{
    /// <summary>
    /// Represents an object that is part of a Floor (such as <see cref="Atom.Scene.Tiles.TileMapFloor"/>) in a Scene.
    /// </summary>
    public interface IFloorObject
    {
        /// <summary>
        /// Gets the number that uniquely identifies the floor this IFloorObject is on.
        /// </summary>
        /// <value>
        /// A zero-based value that can be used to index into the list of floors.
        /// </value>
        int FloorNumber 
        {
            get;
        }
    }
}
