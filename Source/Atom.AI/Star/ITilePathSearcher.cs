// <copyright file="ITilePathSearcher.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.AI.ITilePathSearcher interface.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.AI
{
    using Atom.Scene.Tiles;

    /// <summary>
    /// Provides a mechanism for finding <see cref="TilePath"/>s between two locations.
    /// </summary>
    public interface ITilePathSearcher
    {
        /// <summary>
        /// Tries to find a path from the starting point to the target point.
        /// </summary>
        /// <typeparam name="TCallerType">
        /// The type of the object that a path is searched for.
        /// </typeparam>
        /// <param name="startX">
        /// The starting point on the x-axis.
        /// </param>
        /// <param name="startY">
        /// The starting point on the y-axis.
        /// </param>
        /// <param name="targetX">
        /// The end point on the x-axis.
        /// </param>
        /// <param name="targetY">
        /// The end point on the y-axis.
        /// </param>
        /// <param name="caller">
        /// The object to search a path for.
        /// </param>
        /// <param name="tileHandler">
        /// The <see cref="ITileHandler&lt;TCallerType&gt;"/> 
        /// to use when checking whether a tile is walkable.
        /// </param>
        /// <returns>
        /// The path from the starting point to the target point.
        /// </returns>
        TilePath FindPath<TCallerType>( int startX, int startY, int targetX, int targetY, TCallerType caller, ITileHandler<TCallerType> tileHandler );

        /// <summary>
        /// Tries to find a path from the starting tile to the target tile.
        /// </summary>
        /// <typeparam name="TCallerType">
        /// The type of the object that a path is searched for.
        /// </typeparam>
        /// <param name="startX">
        /// The starting point on the x-axis in tile-space.
        /// </param>
        /// <param name="startY">
        /// The starting point on the y-axis in tile-space.
        /// </param>
        /// <param name="targetX">
        /// The end point on the x-axis in tile-space.
        /// </param>
        /// <param name="targetY">
        /// The end point on the y-axis in tile-space.
        /// </param>
        /// <param name="caller">
        /// The object to search a path for.
        /// </param>
        /// <param name="tileHandler">
        /// The <see cref="ITileHandler&lt;TCallerType&gt;"/> 
        /// to use when checking whether a tile is walkable.
        /// </param>
        /// <returns>
        /// The path from the starting tile to the target tile.
        /// </returns>
        TilePath FindPathTile<TCallerType>( int startX, int startY, int targetX, int targetY, TCallerType caller, ITileHandler<TCallerType> tileHandler );
    }
}
