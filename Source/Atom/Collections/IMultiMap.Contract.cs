////// <copyright file="IMultiMap.Contract.cs" company="federrot Software">
//////     Copyright (c) federrot Software. All rights reserved.
////// </copyright>
////// <summary>Defines code contracts for the Atom.Collections.IMultiMap{Tkey, TElement} interface.</summary>
////// <author>Paul Ennemoser (Tick)</author>

////namespace Atom.Collections
////{
////    using System;
////    using System.Collections.Generic;
////    using Atom.Diagnostics.Contracts;

////    /// <summary>
////    /// Defines code contracts for the IMultiMap interface.
////    /// </summary>
////    /// <typeparam name="TKey">
////    /// The type of the key that identifies a group of elements.
////    /// </typeparam>
////    /// <typeparam name="TElement">
////    /// The type of the elements stored in the IMultiMap.
////    /// </typeparam>
////    // [ContractClassFor( typeof( IMultiMap<,> ) )]
////    [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
////        Justification = "Contract classes for interfaces aren't required to be documented." )] 
////    internal abstract class IMultiMapContract<TKey, TElement> : IMultiMap<TKey, TElement>
////    {
////        public void Add( TKey key, TElement value )
////        {
////            Contract.Requires<ArgumentNullException>( key != null );
////            Contract.Ensures( this.Contains( key, value ) );
////        }

////        public bool Remove( TKey key, TElement value )
////        {
////            Contract.Ensures( !(key == null) || !Contract.Result<bool>() );
////            return default( bool );
////        }

////        [Pure]
////        public bool TryGet( TKey key, out IEnumerable<TElement> elements )
////        {
////            Contract.Requires<ArgumentNullException>( key != null );
////            Contract.Ensures( !(Contract.Result<bool>() == false) || Contract.ValueAtReturn( out elements ) == null );
////            Contract.Ensures( !(Contract.Result<bool>() == true) || Contract.ValueAtReturn( out elements ) != null );

////            elements = null;
////            return default( bool );
////        }

////        [Pure]
////        public bool Contains( TKey key, TElement value )
////        {
////            Contract.Ensures( !(key == null) || !Contract.Result<bool>() );
////            return default( bool );
////        }
////    }
////}
