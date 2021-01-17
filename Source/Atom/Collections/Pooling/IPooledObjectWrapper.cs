// <copyright file="IPooledObjectWrapper.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>Defines the Atom.Collections.Pooling.IPooledObjectWrapper{T} interface.</summary>
// <author>Paul Ennemoser</author>

namespace Atom.Collections.Pooling
{
    /// <summary>
    /// Represents an <see cref="Atom.Collections.Pooling.IPooledObject{T}"/> that
    /// wraps the concept of pooling around another Object.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the pooled Object.
    /// </typeparam>
    /// <seealso cref="Atom.Collections.Pooling.Pool{T}"/>
    public interface IPooledObjectWrapper<T> : IPooledObject<IPooledObjectWrapper<T>>
    {
        /// <summary>
        /// Gets the Object this PooledObjectWrapper{T} wraps around.
        /// </summary>
        T PooledObject
        {
            get;
        }
    }
}
