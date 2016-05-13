// <copyright file="ITypeActivator.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.ITypeActivator interface.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom
{
    /// <summary>
    /// Provides a mechanism that creates new objects given a type name.
    /// </summary>
    public interface ITypeActivator
    {
        /// <summary>
        /// Creates an instance of the type with the given typeName.
        /// </summary>
        /// <param name="typeName">
        /// The name that uniquely identifies the type to initiate.
        /// </param>
        /// <returns>
        /// The object that has been created.
        /// </returns>
        object CreateInstance( string typeName );
    }
}
