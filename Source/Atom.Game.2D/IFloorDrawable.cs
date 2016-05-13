// <copyright file="IFloorDrawable.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.IFloorDrawable interface.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom
{
    using Atom.Scene;

    /// <summary>
    /// Specifies the interface of a drawable object
    /// which is part of a floor, such as a TileMapFloor.
    /// </summary>
    public interface IFloorDrawable : IDrawable, IFloorObject
    {
        /// <summary>
        /// Gets a value that indicates whether this <see cref="IFloorDrawable"/> is drawn
        /// above or below other <see cref="IFloorDrawable"/>s on the same Floor as this <see cref="IFloorDrawable"/>.
        /// </summary>
        /// <value>
        /// The relative draw order of this IFloorDrawable.
        /// Is used when sorting the IDrawable objects for drawing.
        /// </value>
        float RelativeDrawOrder 
        {
            get;
        }
    }
}
