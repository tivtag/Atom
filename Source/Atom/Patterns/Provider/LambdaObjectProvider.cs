// <copyright file="LambdaObjectProvider.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Patterns.Provider.LambdaObjectProvider class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Patterns.Provider
{
    using System;
    using Atom.Diagnostics.Contracts;

    /// <summary>
    /// Implements an <see cref="IObjectProvider{TObject}"/> that delegates the resolving
    /// of the object into delegate / lambda function.
    /// </summary>
    /// <typeparam name="TObject">
    /// The type of the object this IObjectProvider{TObject} provides.
    /// </typeparam>
    public sealed class LambdaObjectProvider<TObject> : IObjectProvider<TObject>
        where TObject : class
    {
        /// <summary>
        /// Initializes a new instance of the LambdaObjectProvider class.
        /// </summary>
        /// <param name="lambda">
        /// The function that is used to resolve the object.
        /// </param>
        public LambdaObjectProvider( Func<TObject> lambda )
        {
            Contract.Requires<ArgumentNullException>( lambda != null );

            this.lambda = container => lambda();
        }

        /// <summary>
        /// Initializes a new instance of the LambdaObjectProvider class.
        /// </summary>
        /// <param name="lambda">
        /// The function that is used to resolve the object.
        /// </param>
        /// <param name="container">
        /// The container that can be used by the lambda to further resolve different objects.
        /// </param>
        public LambdaObjectProvider( Func<IObjectProviderContainer, TObject> lambda, IObjectProviderContainer container )
        {
            Contract.Requires<ArgumentNullException>( lambda != null );
            Contract.Requires<ArgumentNullException>( container != null );

            this.lambda = lambda;
            this.container = container;
        }

        /// <summary>
        /// Gets the object this IObjectProvider{TObject}.
        /// </summary>
        /// <returns>
        /// The object this IObjectProvider{TObject} provides.
        /// </returns>
        public TObject TryResolve()
        {
            return this.lambda( this.container );
        }

        /// <summary>
        /// The function that is used to resolve the object.
        /// </summary>
        private readonly Func<IObjectProviderContainer, TObject> lambda;

        /// <summary>
        /// The container that can be used by the lambda to further resolve different objects.
        /// </summary>
        private readonly IObjectProviderContainer container;
    }
}
