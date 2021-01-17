// <copyright file="WrappingPool.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>Defines the Atom.Collections.Pooling.WrappingPool{T} class.</summary>
// <author>Paul Ennemoser</author>

namespace Atom.Collections.Pooling
{
    using Atom.Diagnostics.Contracts;

    /// <summary>
    /// Represents a Pool that stores <see cref="IPooledObjectWrapper{T}"/>s instead
    /// of directly storing <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the items the Pool stores within its <see cref="IPooledObjectWrapper{T}"/>s.
    /// </typeparam>
    public class WrappingPool<T> : Pool<IPooledObjectWrapper<T>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WrappingPool{T}"/> class.
        /// </summary>
        /// <param name="initialSize">
        /// The total number of items the new WrappingPool{T} initially contains.
        /// </param>
        /// <param name="creator">
        /// The method that creates and initializes new items for the new WrappingPool{T}.
        /// </param>
        public WrappingPool( int initialSize, PooledObjectCreator<IPooledObjectWrapper<T>> creator )
            : base( initialSize, creator )
        {
            Contract.Requires( initialSize >= 0 );
            Contract.NotNull( creator, nameof( creator ) );
        }

        /// <summary>
        /// Called when the given PoolNode{IPooledObjectWrapper{T}} has been created
        /// for this WrappingPool{T}.
        /// </summary>
        /// <param name="node">
        /// The PoolNode{IPooledObjectWrapper{T}} that has just been created.
        /// This value is never null.
        /// </param>
        protected override void OnCreated( PoolNode<IPooledObjectWrapper<T>> node )
        {
            IPooledObjectWrapper<T> wrapper = node.Item;
            wrapper.PoolNode = node;
        }
    }
}
