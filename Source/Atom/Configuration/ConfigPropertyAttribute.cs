// <copyright file="ConfigPropertyAttribute.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Configuration.ConfigPropertyAttribute{T} class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Configuration
{
    using System;

    /// <summary>
    /// Marks a property to be part of the configuration.
    /// </summary>
    [AttributeUsage( AttributeTargets.Property, AllowMultiple=false, Inherited=true )]
    public sealed class ConfigPropertyAttribute : Attribute, IConfigPropertyAttribute
    {
        /// <summary>
        /// Gets or sets the default value of the property.
        /// </summary>
        public object DefaultValue
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the (optional) name under which the property is stored.
        /// </summary>
        public string StorageName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the (optional) comment that is associated with the property.
        /// </summary>
        public string Comment 
        { 
            get;
            set;
        }
    }
}
