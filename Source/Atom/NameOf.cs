// <copyright file="NameOf.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.NameOf{T} class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Linq.Expressions;

    /// <summary>
    /// Implements a fluid operator that receives the
    /// name of various elements of a type.
    /// </summary>
    /// <typeparam name="T">
    /// The type to inspect.
    /// </typeparam>
    public static class NameOf<T>
    {
        /// <summary>
        /// Gets the name of a property of the type <typeparamref name="T"/>.
        /// </summary>
        /// <example>
        /// <code>
        /// string name = NameOf&lt;String&gt;.Property( e => e.Length );
        /// Assert.Equal( "Length", name );
        /// </code>
        /// </example>
        /// <typeparam name="TProperty">
        /// The type of the property.
        /// </typeparam>
        /// <param name="expression">
        /// The expression that returns the property.
        /// </param>
        /// <returns>
        /// The name of the property.
        /// </returns>
        public static string Property<TProperty>( Expression<Func<T, TProperty>> expression )
        {
            Contract.Requires<ArgumentNullException>( expression != null );
            Contract.Requires( expression.Body is MemberExpression );

            var member = (MemberExpression)expression.Body;
            return member.Member.Name;
        }
    }
}
