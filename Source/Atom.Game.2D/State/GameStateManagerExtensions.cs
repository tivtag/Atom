// <copyright file="GameStateManagerExtensions.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.GameStateManagerExtensions class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom
{
    /// <summary>
    /// Defines extension methods for the <see cref="IGameStateManager"/> interface.
    /// </summary>
    public static class GameStateManagerExtensions
    {
        /// <summary>
        /// Pops off all of the <see cref="IGameState"/>s of this <see cref="IGameStateManager"/>.
        /// </summary>
        /// <param name="manager">
        /// The IGameStateManager from which all <see cref="IGameState"/>s
        /// should be popped.
        /// </param>
        public static void PopAll( this IGameStateManager manager )
        {
            while( manager.Pop() ) 
            {
            }
        }

        /// <summary>
        /// Pushes the <see cref="IGameState"/> with the specified <typeparamref name="T"/>ype ontop of the stack.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the IGameState to push.
        /// </typeparam>
        /// <param name="manager">
        /// The IGameStateManager onto which a <see cref="IGameState"/>
        /// should be pushed.
        /// </param>
        public static void Push<T>( this IGameStateManager manager )
            where T : IGameState
        {
            manager.Push( typeof( T ) );
        }

        /// <summary>
        /// Replaces the current <see cref="IGameState"/> with the specified <typeparamref name="T"/>ype 
        /// from this IGameStateManager.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the IGameState to remove.
        /// </typeparam>
        /// <param name="manager">
        /// The IGameStateManager onto which a <see cref="IGameState"/>
        /// should be pushed.
        /// </param>
        public static void Replace<T>( this IGameStateManager manager )
            where T : IGameState
        {
            manager.Replace( typeof( T ) );
        }

        /// <summary>
        /// Attempts to remove the <see cref="IGameState"/> with the specified <typeparamref name="T"/>ype 
        /// from this IGameStateManager.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the IGameState to remove.
        /// </typeparam>
        /// <param name="manager">
        /// The IGameStateManager onto which a <see cref="IGameState"/>
        /// should be pushed.
        /// </param>
        /// <returns>
        /// True if the state was removed;
        /// -or- otherwise false.
        /// </returns>
        public static bool Remove<T>( this IGameStateManager manager )
            where T : IGameState
        {
            return manager.Remove( typeof( T ) );
        }
    }
}
