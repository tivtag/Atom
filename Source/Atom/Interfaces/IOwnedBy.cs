// <copyright file="IOwnedBy.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.IOwnedBy interface.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom
{
    /// <summary>
    /// Represents an object that is owned by another object.
    /// </summary>
    public interface IOwnedBy
    {
        /// <summary>
        /// Gets or sets the object that owns this object.
        /// </summary>
        object Owner
        {
            get;
            set;
        }
    }
}
