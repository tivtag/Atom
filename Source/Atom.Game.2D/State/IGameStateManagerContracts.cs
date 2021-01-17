////// <copyright file="IGameStateManagerContracts.cs" company="federrot Software">
//////     Copyright (c) federrot Software. All rights reserved.
////// </copyright>
////// <summary>
//////     Defines the Atom.IGameStateManagerContracts class.
////// </summary>
////// <author>
//////     Paul Ennemoser
////// </author>

////namespace Atom
////{
////    using System;
////    using System.Collections.Generic;
////    using Atom.Diagnostics.Contracts;

////    [ContractClassFor(typeof(IGameStateManager))]
////    internal abstract class IGameStateManagerContracts : IGameStateManager
////    {
////        IGameState IGameStateManager.Current
////        {
////            get
////            {
////                return default( IGameState );
////            }
////        }

////        void IGameStateManager.Add( IGameState state )
////        {
////            Contract.Requires<ArgumentNullException>( state != null );
////            Contract.Requires<ArgumentException>( !((IGameStateManager)this).Contains( state.GetType() ) );
////        }

////        bool IGameStateManager.Remove( Type type )
////        {
////            return default( bool );
////        }

////        [Pure]
////        bool IGameStateManager.Contains( Type type )
////        {
////            return default( bool );
////        }

////        [Pure]
////        IGameState IGameStateManager.Get( Type type )
////        {
////            return default( IGameState );
////        }

////        bool IGameStateManager.Pop()
////        {
////            throw new NotImplementedException();
////        }

////        void IGameStateManager.Push( Type type )
////        {
////            Contract.Requires<ArgumentNullException>( type != null );
////        }

////        void IGameStateManager.Replace( Type type )
////        {
////            Contract.Requires<ArgumentNullException>( type != null );
////        }

////        IEnumerator<IGameState> IEnumerable<IGameState>.GetEnumerator()
////        {
////            return default( IEnumerator<IGameState> );
////        }

////        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
////        {
////            return default( System.Collections.IEnumerator );
////        }
////    }
////}
