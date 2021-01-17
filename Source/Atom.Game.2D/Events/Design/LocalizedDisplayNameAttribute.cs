// <copyright file="LocalizedDisplayNameAttribute.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Events.Design.LocalizedDisplayNameAttribute class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Events.Design
{
    using System.ComponentModel;

    /// <summary>
    /// Defines a localized DisplayNameAttribute that uses the <see cref="Atom.Events.EventStrings"/>
    /// resource manager to lock-up resource information.
    /// This class can't be inherited.
    /// </summary>
    internal sealed class LocalizedDisplayNameAttribute : DisplayNameAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LocalizedDisplayNameAttribute"/> class.
        /// </summary>
        /// <param name="resourceName">
        /// The name of the string resource that is locked-up.
        /// </param>
        public LocalizedDisplayNameAttribute( string resourceName )
            : base( EventStrings.ResourceManager.GetString( resourceName ) )
        {
        }
    }
}
