// <copyright file="GameStateManager.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.GameStateManager class.
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
    /// Implements a mechanism that allows the management of the <see cref="IGameState"/>s of a game.
    /// </summary>
    public class GameStateManager : IGameStateManager
    {
        /// <summary>
        /// Gets the current IGameState.
        /// </summary>
        /// <value>
        /// The <see cref="IGameState"/> that is at the top of the IGameState stack;
        /// or null.
        /// </value>
        public IGameState Current
        {
            get
            {
                if( this.stateStack.Count == 0 )
                    return null;

                return this.stateStack.Peek();
            }
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="GameStateManager"/> class.
        /// </summary>
        /// <param name="initialStateCapacity"> 
        /// The initial number of states the <see cref="GameStateManager"/> can contain. 
        /// </param>
        public GameStateManager( int initialStateCapacity )
        {
            this.availableStates = new Dictionary<Type, IGameState>( initialStateCapacity );
        }

        /// <summary>
        /// Adds the specified <see cref="IGameState"/> to the list of aviable states of this <see cref="GameStateManager"/>.
        /// </summary>
        /// <param name="state"> The state to add. </param>
        /// <exception cref="System.ArgumentNullException">If <paramref name="state"/> is null.</exception>
        /// <exception cref="ArgumentException">
        /// If there already exists a state with the same name as the specified.
        /// </exception>
        public void Add( IGameState state )
        {
            this.availableStates.Add( state.GetType(), state );
        }

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
        public bool Remove( Type type )
        {
            return this.availableStates.Remove( type );
        }

        /// <summary>
        /// Replaces the current state with the one with the specified <paramref name="type"/>.
        /// </summary>
        /// <param name="type">
        /// The type of the state.
        /// </param>
        public void Replace( Type type )
        {
            if( this.stateStack.Count > 0 )
            {
                if( this.stateStack.Peek().GetType() == type )
                {
                    // The state is already on top.
                    return;
                }
            }

            IGameState state = this.Get( type );
            IGameState oldState = this.stateStack.Pop();
            this.stateStack.Push( state );

            oldState.ChangedTo( state );
            state.ChangedFrom( oldState );
        }

        /// <summary>
        /// Pushes the state with the specified <paramref name="type"/> ontop of the stack.
        /// </summary>
        /// <param name="type">
        /// The type of the state.
        /// </param>
        public void Push( Type type )
        {
            if( this.stateStack.Count != 0 )
            {
                if( this.stateStack.Peek().GetType() == type )
                { 
                    // The state is already on top.
                    return;
                }
            }

            IGameState state = this.Get( type );       

            IGameState oldState = null;
            if( stateStack.Count != 0 )
                oldState = this.stateStack.Peek();
            this.stateStack.Push( state );

            if( oldState != null )
                oldState.ChangedTo( state );
            state.ChangedFrom( oldState );
        }

        /// <summary>
        /// Pops the current state from the stack and changes to the one before.
        /// </summary>
        /// <returns>
        /// Returns <see langword="true"/> if a state has been poped from the stack;
        /// otherwise <see langword="false"/>.
        /// </returns>
        public bool Pop()
        {
            if( this.stateStack.Count == 0 )
                return false;

            IGameState oldState = this.stateStack.Pop();

            if( stateStack.Count == 0 )
            {
                oldState.ChangedTo( null );
            }
            else
            {
                oldState.ChangedTo( this.stateStack.Peek() );
                this.stateStack.Peek().ChangedFrom( oldState );
            }

            return true;
        }

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
        public IGameState Get( Type type )
        {
            return this.availableStates[type];
        }

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
        public bool Contains( Type type )
        {
            return this.availableStates.ContainsKey( type );
        }

        /// <summary>
        /// Pops all <see cref="IGameState"/> and then clears the list
        /// of available states.
        /// </summary>
        public void Clear()
        {
            this.PopAll();
            this.availableStates.Clear();
        }

        /// <summary>
        /// Returns an enumerator that iterates over the
        /// <see cref="IGameState"/>s available to this <see cref="GameStateManager"/>.
        /// </summary>
        /// <returns>
        /// A new enumerator.
        /// </returns>
        public IEnumerator<IGameState> GetEnumerator()
        {
            return this.availableStates.Values.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates over the
        /// <see cref="IGameState"/>s available to this <see cref="GameStateManager"/>.
        /// </summary>
        /// <returns>
        /// A new enumerator.
        /// </returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.availableStates.Values.GetEnumerator();
        }

        /// <summary>
        /// The current stack of states. The uppermost is the current.
        /// </summary>
        private readonly Stack<IGameState> stateStack = new Stack<IGameState>();

        /// <summary>
        /// All aviable states sorted by name.
        /// </summary>
        private readonly Dictionary<Type, IGameState> availableStates;
    }
}
