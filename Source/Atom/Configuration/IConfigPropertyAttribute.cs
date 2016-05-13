// <copyright file="IConfigPropertyAttribute.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Configuration.IConfigPropertyAttribute{T} interface.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Configuration
{
    using System;

    /// <summary>
    /// Represents the data that is attached to a property
    /// to make it configurable.
    /// </summary>
    public interface IConfigPropertyAttribute
    {
        /// <summary>
        /// Gets or sets the default value of the property.
        /// </summary>
        object DefaultValue 
        {
            get; 
            set; 
        }

        /// <summary>
        /// Gets or sets the (optional) name under which the property is stored.
        /// </summary>
        string StorageName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the (optional) comment that is associated with the property.
        /// </summary>
        string Comment
        {
            get;
            set;
        }
    }
}
