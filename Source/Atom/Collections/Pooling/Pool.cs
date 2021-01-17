// <copyright file="Pool.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <additional_copyright>
// Additional Copyright (c) 2007 Thomas H. Aylesworth
//
// This is a heavily modified version of Thomas H. Aylesworth's Pool class,
// modified by Paul Ennemoser aka. Tick/tanek/Teka.
//
// Thanks for the original code, from 
// http://swampthingtom.blogspot.com/2007/06/generic-pool-collection-class.html
//
// Permission is hereby granted, free of charge, to any person obtaining a
// copy of this software and associated documentation files (the "Software"), 
// to deal in the Software without restriction, including without limitation 
// the rights to use, copy, modify, merge, publish, distribute, sublicense, 
// and/or sell copies of the Software, and to permit persons to whom the 
// Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in 
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR 
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE 
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER 
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING 
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.
// </additional_copyright>
// <summary>
//     Defines the Atom.Collections.Pooling.Pool{T} class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Collections.Pooling
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Atom.Diagnostics.Contracts;

    /// <summary>
    /// Represents a pool of available items that can be removed as needed
    /// and returned when finished.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the items the Pool stores.
    /// The type must have a public paramterless constructor.
    /// </typeparam>
    public class Pool<T> : IEnumerable<T>
    {
        /// <summary>
        /// Gets or sets a value indicating whether this Pool{T}
        /// is of fixed size; and as such doesn't resize itself when required.
        /// </summary>
        /// <value>
        /// The default value is false.
        /// </value>
        public bool IsFixedSize
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the number of available items in the Pool{T}.
        /// </summary>
        /// <remarks>
        /// Retrieving this property is an O(1) operation.
        /// </remarks>
        /// <value>
        /// The number of items that are currently available in the Pool{T}.
        /// </value>
        public int AvailableCount
        {
            get 
            {
                // Contract.Ensures( Contract.Result<int>() >= 0 );

                return this.available.Count;
            }
        }

        /// <summary>
        /// Gets the number of active items in the Pool{T}.
        /// </summary>
        /// <remarks>
        /// Retrieving this property is an O(1) operation.
        /// </remarks>
        /// <value>
        /// The number of items that are currently used by the Pool.
        /// </value>
        public int ActiveCount
        {
            get
            {
                // Contract.Ensures( Contract.Result<int>() >= 0 );
                return this.pool.Count - this.available.Count;
            }
        }

        /// <summary>
        /// Gets the total number of items in the Pool{T}.
        /// </summary>
        /// <remarks>
        /// Retrieving this property is an O(1) operation.
        /// </remarks>
        /// <value>
        /// The number of items that the Pool can handle.
        /// </value>
        public int Capacity
        {
            get
            {
                // Contract.Ensures( Contract.Result<int>() >= 0 );

                return this.pool.Count;
            }
        }

        /// <summary>
        /// Gets an enumerator that iterates through the active nodes 
        /// in the Pool{T}.
        /// </summary>
        /// <remarks>
        /// This method is an O(n) operation, 
        /// where n is Capacity divided by ActiveCount. 
        /// </remarks>
        /// <value>
        /// A new enumerator over the active items in the Pool.
        /// </value>
        public IEnumerable<PoolNode<T>> ActiveNodes
        {
            get
            {
                for( int i = 0; i < this.pool.Count; ++i )
                {
                    PoolNode<T> node = this.pool[i];

                    if( node.IsActive )
                    {
                        yield return node;
                    }
                }
            }
        }

        /// <summary>
        /// Gets an enumerator that iterates through all of the nodes 
        /// in the Pool.
        /// </summary>
        /// <remarks>
        /// This method is an O(1) operation. 
        /// </remarks>
        /// <value>
        /// A new enumerator over all items in the Pool{T}.
        /// </value>
        public IEnumerable<PoolNode<T>> AllNodes => pool;

        /// <summary>
        /// Initializes a new instance of the <see cref="Pool{T}"/> class.
        /// </summary>
        /// <remarks>
        /// No actual nodes are added by the Pool{T}. Consider using <see cref="AddNodes"/> after creating the Pool.
        /// </remarks>
        /// <param name="initialCapacity">
        /// The total number of items the new Pool{T} can contain without having to re-allocate memory.
        /// </param>
        /// <param name="creator">
        /// The method that creates and initializes new <typeparamref name="T"/>-Items for the new Pool{T}.
        /// </param>
        public Pool( int initialCapacity, PooledObjectCreator<T> creator )
        {
            Contract.Requires<ArgumentException>( initialCapacity >= 0 );
            Contract.NotNull( creator, nameof( creator ) );

            this.creator = creator;
            this.pool = new List<PoolNode<T>>( initialCapacity );
            this.available = new Queue<int>( initialCapacity );
        }

        /// <summary>
        /// Creates a new Pool{T} that contains <paramref name="initialSize"/> pre-allocated <see cref="PoolNode{T}"/>s.
        /// </summary>
        /// <param name="initialSize">
        /// The initial number of  <see cref="PoolNode{T}"/>s to pre-allocate.
        /// </param>
        /// <param name="creator">
        /// The method that creates and initializes new <typeparamref name="T"/>-Items for the new Pool{T}.
        /// </param>
        /// <returns>
        /// The newly created Pool{T}.
        /// </returns>
        public static Pool<T> Create( int initialSize, PooledObjectCreator<T> creator )
        {
            var pool = new Pool<T>( initialSize, creator );
            pool.AddNodes( initialSize );

            return pool;
        }

        /// <summary>
        /// Defines the conditions that should hold true on each instance of a class
        /// whenever that object is visible to a client.
        /// </summary>
        // [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant( 0 <= this.available.Count );
            Contract.Invariant( this.available.Count <= this.pool.Count );
        }

        /// <summary>
        /// Makes all items in the Pool available.
        /// </summary>
        /// <remarks>
        /// This method is an O(n) operation, where n is Capacity.
        /// </remarks>
        public void Clear()
        {
            this.available.Clear();

            for( int i = 0; i < this.pool.Count; ++i )
            {
                this.pool[i].IsActive = false;
                this.available.Enqueue( i );
            }
        }

        /// <summary>
        /// Removes an available item from the Pool and makes it active.
        /// </summary>
        /// <remarks>
        /// This method is an O(1) operation, unless the Pool must be resized.
        /// </remarks>
        /// <returns>
        /// The node that is removed from the available Pool; 
        /// or null if there are no available nodes and Pool is of fixed size (<seealso cref="IsFixedSize"/>).
        /// </returns>
        public PoolNode<T> Get()
        {
            if( this.available.Count == 0 )
            {
                if( this.IsFixedSize )
                {
                    return null;
                }
                else
                {
                    this.AddNewNode();
                }
            }

            int nodeIndex = this.available.Dequeue();
            // Contract.Assume( 0 <= nodeIndex && nodeIndex < this.pool.Count );

            PoolNode<T> node = this.pool[nodeIndex];
            node.IsActive = true;
            return node;
        }

        /// <summary>
        /// Returns the given active <see cref="PoolNode{T}"/> to 
        /// the list of available nodes of this Pool{T}
        /// </summary>
        /// <remarks>
        /// This method is an O(1) operation.
        /// </remarks>
        /// <param name="node">
        /// The node to return to this Pool{T}.
        /// </param>
        /// <exception cref="ArgumentException">
        /// The node being returned is not owned by this Pool{T}.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// The node being returned was not active.
        /// This probably means the node was previously returned.
        /// </exception>
        public void Return( PoolNode<T> node )
        {
            Contract.Requires<ArgumentNullException>( node != null );
            Contract.Requires<ArgumentException>( node.Pool == this, ErrorStrings.InvalidNodePoolMismatch );
            Contract.Requires<InvalidOperationException>( node.IsActive, ErrorStrings.AttemptToReturnInActiveNode );

            node.IsActive = false;
            this.available.Enqueue( node.NodeIndex );
        }

        /// <summary>
        /// Gets an enumerator that iterates through the active items 
        /// of this Pool.
        /// </summary>
        /// <returns>
        /// A new enumerator that iterates through the active items 
        /// of this Pool.
        /// </returns>
        /// <remarks>
        /// This method is an O(n) operation, 
        /// where n is Capacity divided by ActiveCount. 
        /// </remarks>
        public IEnumerator<T> GetEnumerator()
        {
            for( int i = 0; i < this.pool.Count; ++i )
            {
                PoolNode<T> node = this.pool[i];

                if( node.IsActive )
                {
                    yield return node.Item;
                }
            }
        }

        /// <summary>
        /// Gets an enumerator that iterates through the active items 
        /// of this Pool.
        /// </summary>
        /// <returns>
        /// A new enumerator that iterates through the active items 
        /// of this Pool.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <summary>
        /// Adds the specified number of <see cref="PoolNode{T}"/>s to this Pool{T}.
        /// </summary>
        /// <param name="count">
        /// The number of nodes to add.
        /// </param>
        public void AddNodes( int count )
        {
            Contract.Requires<ArgumentException>( count >= 0 );

            for( int i = 0; i < count; ++i )
            {
                this.AddNewNode();
            } 
        }

        /// <summary>
        /// Adds a new <see cref="PoolNode{T}"/> to this Pool{T},
        /// and as such resizing the Pool.
        /// </summary>
        private void AddNewNode()
        {
            var node = new PoolNode<T>( this.creator(), this.pool.Count, this );

            this.pool.Add( node );
            this.available.Enqueue( node.NodeIndex );

            this.OnCreated( node );
        }

        /// <summary>
        /// Called when the given <see cref="PoolNode{T}"/> has been created
        /// for this Pool{T}.
        /// </summary>
        /// <param name="node">
        /// The PoolNode{T} that has just been created.
        /// </param>
        protected virtual void OnCreated( PoolNode<T> node )
        {
            Contract.NotNull( node, nameof( node ) );
        }

        /// <summary>
        /// The method that is used to create new Objects for this Pool{T}.
        /// </summary>
        private readonly PooledObjectCreator<T> creator;

        /// <summary>
        /// The pool of item nodes.
        /// </summary>
        private readonly List<PoolNode<T>> pool;

        /// <summary>
        /// The queue of available item node indices.
        /// </summary>
        private readonly Queue<int> available;
    }
}