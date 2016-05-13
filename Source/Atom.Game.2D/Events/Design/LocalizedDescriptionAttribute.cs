// <copyright file="LocalizedDescriptionAttribute.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Events.Design.LocalizedDescriptionAttribute class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Events.Design
{
    using System.ComponentModel;

    /// <summary>
    /// Defines a localized DescriptionAttribute that uses the <see cref="Atom.Events.EventStrings"/>
    /// resource manager to lock-up resource information.
    /// This class can't be inherited.
    /// </summary>
    internal sealed class LocalizedDescriptionAttribute : DescriptionAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LocalizedDescriptionAttribute"/> class.
        /// </summary>
        /// <param name="resourceName">
        /// The name of the string resource that is locked-up.
        /// </param>
        public LocalizedDescriptionAttribute( string resourceName )
            : base( EventStrings.ResourceManager.GetString( resourceName ) )
        {
        }
    }
}
