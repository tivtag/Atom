// <copyright file="IScene.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Scene.IScene interface.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Scene
{
    using Atom.Components;

    /// <summary>
    /// A scene represents as an aggregation of <see cref="IEntity"/>s the main
    /// meeting point of a game.
    /// </summary>
    public interface IScene
    {
        /// <summary>
        /// Adds the given <see cref="IEntity"/> to this IScene.
        /// </summary>
        /// <param name="entity">
        /// The entity to add.
        /// </param>
        void Add( IEntity entity );

        /// <summary>
        /// Tries to remove the given <see cref="IEntity"/> from this IScene.
        /// </summary>
        /// <param name="entity">The Entity to remove.</param>
        /// <returns>
        /// Returns <see langword="true"/> if the Entity has been removed;
        /// otherwise <see langword="false"/>.
        /// </returns>
        bool Remove( IEntity entity );
    }
}
