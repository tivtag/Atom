// <copyright file="IMultiFloorPathSearcherProvider.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.AI.IMultiFloorPathSearcherProvider interface.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.AI
{
    /// <summary>
    /// Provides a mechanism for receiving a <see cref="AStarTilePathSearcher"/> given a floor-number.
    /// </summary>
    /// <remarks>
    /// Each TileMapFloor has its own ActionLayer, that can be searched in with a AStarTilePathSearcher.
    /// </remarks>
    public interface IMultiFloorPathSearcherProvider
    {
        /// <summary>
        /// Gets the <see cref="ITilePathSearcher"/> for the floor with the specified <paramref name="floorNumber"/>.
        /// </summary>
        /// <param name="floorNumber">
        /// The number of the floor.
        /// </param>
        /// <returns>
        /// The <see cref="ITilePathSearcher"/> for the floor with the specified floorNumber.
        /// </returns>
        ITilePathSearcher GetTilePathSearcher( int floorNumber );
    }
}
