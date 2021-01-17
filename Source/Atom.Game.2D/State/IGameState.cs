// <copyright file="IGameState.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.IGameState interface.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom
{
    /// <summary>
    /// Defines the common properties and methods of a GameState.
    /// </summary>
    /// <remarks> 
    /// Examples for game-states would be:
    /// "Title Screen", "Option Screen",
    /// "Mode Select Screen", "Ingame", ...
    /// </remarks>
    public interface IGameState
    {
        /// <summary>
        /// Draws this <see cref="IGameState"/>.
        /// </summary>
        /// <param name="drawContext">
        /// The current <see cref="IDrawContext"/>.
        /// </param>
        void Draw( IDrawContext drawContext );

        /// <summary>
        /// Updates this <see cref="IGameState"/>.
        /// </summary>
        /// <param name="updateContext">
        /// The current <see cref="IUpdateContext"/>.
        /// </param>
        void Update( IUpdateContext updateContext );

        /// <summary>
        /// Called when changing from the <paramref name="oldState"/> to this <see cref="IGameState"/>.
        /// </summary>
        /// <param name="oldState">
        /// The state before. Null means no state.
        /// </param>
        void ChangedFrom( IGameState oldState );

        /// <summary>
        /// Called when changing from this <see cref="IGameState"/> to the <paramref name="newState"/>.
        /// </summary>
        /// <param name="newState">
        /// The new state.
        /// </param>
        void ChangedTo( IGameState newState );

        /// <summary>
        /// Loads this <see cref="IGameState"/>, if required.
        /// </summary>
        void Load();

        /// <summary>
        /// Unloads this <see cref="IGameState"/>;
        /// while still being able to reload it.
        /// </summary>
        void Unload();
    }
}
