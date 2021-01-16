// <copyright file="PoolNode.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>Defines the Atom.Collections.Pooling.PoolNode{T} class.</summary>
// <author>Paul Ennemoser (Tick)</author>

namespace Atom.Collections.Pooling
{
    using Atom.Diagnostics.Contracts;

    /// <summary>
    /// Represents an entry in the <see cref="Atom.Collections.Pooling.Pool{T}"/> collection.
    /// This class can't be inherited.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the item the PoolNode stores.
    /// </typeparam>
    public sealed class PoolNode<T>
    {
        #region [ Properties ]

        /// <summary>
        /// Gets the item stored in this PoolNode{T}.
        /// </summary>
        /// <value>The item this PoolNode{T} contains.</value>
        public T Item
        {
            get
            {
                return this.item; 
            }
        }

        /// <summary>
        /// Gets a value indicating whether this PoolNode{T} is currently
        /// used by the application and as such is not available for reuse.
        /// </summary>
        /// <value>
        /// Retirms true if this PoolNode{T} is currently
        /// used by the application;
        /// or otherwise false if the application may re.use the pool.
        /// </value>
        public bool IsActive
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the <see cref="Atom.Collections.Pooling.Pool{T}"/> that owns this PoolNode{T}.
        /// </summary>
        /// <value>
        /// The <see cref="Atom.Collections.Pooling.Pool{T}"/> that owns this PoolNode{T}.
        /// </value>
        public Pool<T> Pool
        {
            get
            {
                // Contract.Ensures( Contract.Result<Pool<T>>() != null );
                return this.pool; 
            }
        }

        /// <summary>
        /// Gets the index of this PoolNode{T} into its parent Pool{T}.
        /// </summary>
        internal int NodeIndex
        {
            get { return this.nodeIndex; }
        }

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Initializes a new instance of the <see cref="PoolNode{T}"/> class.
        /// </summary>
        /// <param name="item">
        /// The item that is stored in the new PoolNode{T}.
        /// </param>
        /// <param name="nodeIndex">
        /// The the index of the new PoolNode{T} into its parent <paramref name="pool"/>.
        /// </param>
        /// <param name="pool">
        /// The <see cref="Atom.Collections.Pooling.Pool{T}"/> that owns the new PoolNode{T}.
        /// </param>
        internal PoolNode( T item, int nodeIndex, Pool<T> pool )
        {
            Contract.NotNull( pool, nameof( pool ) );

            this.item      = item;
            this.nodeIndex = nodeIndex;
            this.pool      = pool;
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Defines the conditions that should hold true on each instance of a class
        /// whenever that object is visible to a client.
        /// </summary>
        ///  [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant( this.Pool != null );
        }

        #endregion

        #region [ Fields ]

        /// <summary>
        /// The item that is stored in this PoolNode{T}.
        /// </summary>
        private readonly T item;

        /// <summary>
        /// Used internally to track which entry in the Pool{T} is associated with this PoolNode{T}.
        /// </summary>
        private readonly int nodeIndex;

        /// <summary>
        /// Reference to the pool that owns the Node.
        /// </summary>
        private readonly Pool<T> pool;

        #endregion
    }
}
