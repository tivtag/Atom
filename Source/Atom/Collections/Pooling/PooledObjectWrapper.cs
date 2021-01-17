// <copyright file="PooledObjectWrapper.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>Defines the Atom.Collections.Pooling.PooledObjectWrapper{T} class.</summary>
// <author>Paul Ennemoser</author>

namespace Atom.Collections.Pooling
{
    using System;
    using Atom.Diagnostics.Contracts;

    /// <summary>
    /// Defines an <see cref="IPooledObject{T}"/> that wraps 
    /// the concept of pooling around another Object.
    /// </summary>
    /// <seealso cref="Pool{T}"/>
    /// <remarks>
    /// This allows to seperate the concept of pooling from the
    /// implementation of an object.
    /// </remarks>
    /// <typeparam name="T">
    /// The type of the pooled Object.
    /// </typeparam>
    public class PooledObjectWrapper<T> : IPooledObjectWrapper<T>
    {
        /// <summary>
        /// Gets or sets the PoolNode{IPooledObjectWrapper{T}} which
        /// is associated with this PooledObjectWrapper{T}.
        /// </summary>
        public PoolNode<IPooledObjectWrapper<T>> PoolNode
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes a new instance of the PooledObjectWrapper class.
        /// </summary>
        /// <param name="pooledObject">
        /// The object this PooledObjectWrapper wraps around.
        /// </param>
        public PooledObjectWrapper( T pooledObject )
        {
            Contract.Requires<ArgumentNullException>( pooledObject != null );

            this.pooledObject = pooledObject;
        }

        /// <summary>
        /// Gets the Object this PooledObjectWrapper{T} wraps around.
        /// </summary>
        public T PooledObject
        {
            get
            {
                // Contract.Ensures( Contract.Result<T>() != null );

                return this.pooledObject;
            }
        }

        /// <summary>
        /// Stores the Object this PooledObjectWrapper{T} wraps around.
        /// </summary>
        private readonly T pooledObject;
    }
}
