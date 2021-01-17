// <copyright file="INameable.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.INameable interface.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom
{
    /// <summary>
    /// Provides a merchanism to get or sets the name of the object.
    /// </summary>
    public interface INameable : IReadOnlyNameable
    {
        /// <summary>
        /// Gets or sets the name of the named object.
        /// </summary>
        /// <value>The (usually unique) name of the object.</value>
        new string Name
        {
            get;
            set;
        }
    }
}
