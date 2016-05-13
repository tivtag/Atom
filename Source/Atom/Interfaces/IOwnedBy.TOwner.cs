// <copyright file="IOwnedBy.TOwner.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.IOwnedBy{TOwner} interface.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom
{
    /// <summary>
    /// Represents an object that is owned by another object.
    /// </summary>
    /// <typeparam name="TOwner">
    /// The type of the owner.
    /// </typeparam>
    public interface IOwnedBy<TOwner>
    { 
        /// <summary>
        /// Gets or sets the object that owns this object.
        /// </summary>
        TOwner Owner 
        {
            get;
            set;
        }
    }
}
