// <copyright file="ITileHandler.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Scene.Tiles.ITileHandler{TCallerType} interface.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Scene.Tiles
{
    /// <summary>
    /// Descripes the interface of an object that handles actions.
    /// </summary>
    /// <typeparam name="TCallerType">
    /// The type of object whos actions are handled.
    /// </typeparam>
    public interface ITileHandler<TCallerType>
    {
        /// <summary>
        /// Handles the action.
        /// </summary>
        /// <param name="x">
        /// The x-coordinate of the tile (in tile-space).
        /// </param>
        /// <param name="y">
        /// The y-coordinate of the tile (in tile-space).
        /// </param>
        /// <param name="id">
        /// The id of the action.
        /// </param>
        /// <param name="caller">
        /// The object that created the event.
        /// </param>
        /// <returns>
        /// Returns <see lamg="true"/> if to stop handling actions; 
        /// otherwise <see lamg="false"/>.
        /// </returns>
        bool Handle( int x, int y, int id, TCallerType caller );

        /// <summary>
        /// Returns whether the specified tile is walkable by the specified caller.
        /// </summary>
        /// <param name="id">
        /// The id of the action.
        /// </param>
        /// <param name="caller">
        /// The object to test for.</param>
        /// <returns>
        /// Returns <see lamg="true"/> if the tile with the given <paramref name="id"/> is walkable; 
        /// otherwise <see lamg="false"/>.
        /// </returns>
        bool IsWalkable( int id, TCallerType caller );
    }
}
