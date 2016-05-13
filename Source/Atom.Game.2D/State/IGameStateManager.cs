// <copyright file="IGameStateManager.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.IGameStateManager interface.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;

    /// <summary> 
    /// Provides a mechanism that allows the management of the <see cref="IGameState"/>s of a game.
    /// </summary>
    [ContractClass( typeof( IGameStateManagerContracts ) )]
    public interface IGameStateManager : IEnumerable<IGameState>
    {
        /// <summary>
        /// Gets the current IGameState.
        /// </summary>
        /// <value>
        /// The <see cref="IGameState"/> that is at the top of the IGameState stack;
        /// or null.
        /// </value>
        IGameState Current
        {
            get; 
        }

        /// <summary>
        /// Adds the specified <see cref="IGameState"/> to the list of aviable states of this <see cref="GameStateManager"/>.
        /// </summary>
        /// <param name="state"> The state to add. </param>
        /// <exception cref="System.ArgumentNullException">If <paramref name="state"/> is null.</exception>
        /// <exception cref="ArgumentException">
        /// If there already exists a state with the same name as the specified.
        /// </exception>
        void Add( IGameState state );

        /// <summary>
        /// Removes the <see cref="IGameState"/> that has the given <paramref name="type"/>
        /// from the list of aviable states of the <see cref="GameStateManager"/>.
        /// </summary>
        /// <param name="type">
        /// The type of the state to remove.
        /// </param>
        /// <returns>
        /// true if the state has been removed;
        /// -or- otherwise false.
        /// </returns>
        bool Remove( Type type );

        /// <summary>
        /// Gets a value indicating whether this GameStateManager owns an <see cref="IGameState"/>
        /// of the given <paramref name="type"/>.
        /// </summary>
        /// <param name="type">
        /// The type of the state to test for.
        /// </param>
        /// <returns>
        /// True if this GameStateManager contains an <see cref="IGameState"/> of the given <paramref name="type"/>;
        /// otherwise false.
        /// </returns>
        [Pure]
        bool Contains( Type type );

        /// <summary>
        /// Tries to get the <see cref="IGameState"/> with the specified <paramref name="type"/>.
        /// </summary>
        /// <param name="type">
        /// The type of the <see cref="IGameState"/> to get.
        /// </param>
        /// <returns>
        /// The requested IGameState.
        /// </returns>
        [Pure]
        IGameState Get( Type type );

        /// <summary>
        /// Pops the current <see cref="IGameState"/> from the stack and changes to the one before.
        /// </summary>
        /// <returns>
        /// Returns <see langword="true"/> if a state has been poped from the stack;
        /// otherwise <see langword="false"/>.
        /// </returns>
        bool Pop();

        /// <summary>
        /// Pushes the <see cref="IGameState"/> with the specified <paramref name="type"/> ontop of the stack.
        /// </summary>
        /// <param name="type">
        /// The type of the state.
        /// </param>
        void Push( Type type );
        
        /// <summary>
        /// Replaces the current <see cref="IGameState"/> with the one with the specified <paramref name="type"/>.
        /// </summary>
        /// <param name="type">
        /// The type of the state.
        /// </param>
        void Replace( Type type );
    }
}
