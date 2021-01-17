// <copyright file="IReadOnlyNameable.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.IReadOnlyNameable interface.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom
{
    /// <summary>
    /// Provides a merchanism to get the name of the object.
    /// </summary>
    public interface IReadOnlyNameable
    {
        /// <summary>
        /// Gets the name name of the object.
        /// </summary>
        /// <value>The (usually unique) name of the object.</value>
        string Name 
        { 
            get;
        }
    }
}
