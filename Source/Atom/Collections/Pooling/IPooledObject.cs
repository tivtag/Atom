// <copyright file="IPooledObject.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>Defines the Atom.Collections.Pooling.IPooledObject{T} interface.</summary>
// <author>Paul Ennemoser</author>

namespace Atom.Collections.Pooling
{
    /// <summary>
    /// Associates an object with a <see cref="Atom.Collections.Pooling.PoolNode{T}"/>
    /// </summary>
    /// <seealso cref="Atom.Collections.Pooling.Pool{T}"/>
    /// <typeparam name="T">
    /// The type of the object beeing pooled.
    /// </typeparam>
    public interface IPooledObject<T>
    {
        /// <summary>
        /// Gets or sets the <see cref="Atom.Collections.Pooling.PoolNode{T}"/> this IPooledObject{T} is associated with.
        /// </summary>
        PoolNode<T> PoolNode
        {
            get;
            set;
        }
    }
}
