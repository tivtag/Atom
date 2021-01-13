// <copyright file="Anonymous.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Anonymous class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
//     original version by Rinat Abdullin 
//         (http://abdullin.com/journal/2009/10/6/zen-development-practices-c-maybe-monad.html)
// </author>

namespace Atom
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Contains static helper methods related to anonymous types.
    /// </summary>
    public static class Anonymous
    {
        /// <summary>
        /// Creates a new IList{a'} of the given anonymous type.
        /// </summary>
        /// <example>
        /// <code>
        /// var list = Anonymous.CreateList( 
        ///     () => new { Name = default( string ), Id = default( int ) }
        /// );
        ///
        /// list.Add( new { Name = "Meow", Id = 123 } );
        /// list.Add( new { Name = "Cat", Id = 555 } );
        /// list.Add( new { Name = "Dies", Id = -1 } ); 
        /// </code>
        /// </example>
        /// <typeparam name="T">
        /// The type of the list. This is inferred.
        /// </typeparam>
        /// <param name="template">
        /// The function that returns schema object that descripes the anonymous type.
        /// </param>
        /// <returns>
        /// The newly created IList{a'}.
        /// </returns>
        public static IList<T> CreateList<T>( Func<T> template )
            where T : class
        {
            return new List<T>();
        }
    }
}
