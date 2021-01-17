// <copyright file="ISwitchable.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.ISwitchable interface.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom
{
    using System;

    /// <summary>
    /// Defines the interface of an object that can be switched on or off.
    /// </summary>
    public interface ISwitchable
    {
        /// <summary>
        /// Gets or sets a value indicating whether this ISwitchable
        /// is currently switched on or off.
        /// </summary>
        bool IsSwitched
        { 
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this ISwitchable
        /// can currently be switched on or off.
        /// </summary>
        bool IsSwitchable
        { 
            get; 
            set; 
        }

        /// <summary>
        /// Fired when the <see cref="IsSwitched"/> state of this ISwitchable has changed.
        /// </summary>
        event EventHandler IsSwitchedChanged;
    }
}
