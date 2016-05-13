// <copyright file="PooledObjectCreator.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>Defines the  Atom.Collections.Pooling.PooledObjectCreator{T} delegate.</summary>
// <author>Paul Ennemoser (Tick)</author>

namespace Atom.Collections.Pooling
{
    /// <summary>
    /// Provides a mechanism to create an Object used within a <see cref="Atom.Collections.Pooling.Pool{T}"/>.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the object created.
    /// Is not required to implement the <see cref="Atom.Collections.Pooling.IPooledObject{T}"/> interface.
    /// </typeparam>
    /// <returns>
    /// The object which has been created. Should not be null.
    /// </returns>
    public delegate T PooledObjectCreator<T>();
}
