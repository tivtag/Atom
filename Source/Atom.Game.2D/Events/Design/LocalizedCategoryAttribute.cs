// <copyright file="LocalizedCategoryAttribute.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Events.Design.LocalizedCategoryAttribute class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Events.Design
{
    using System.ComponentModel;

    /// <summary>
    /// Defines a localized CategoryAttribute that uses the <see cref="Atom.Events.EventStrings"/>
    /// resource manager to lock-up resource information.
    /// This class can't be inherited.
    /// </summary>
    internal sealed class LocalizedCategoryAttribute : CategoryAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LocalizedCategoryAttribute"/> class.
        /// </summary>
        /// <param name="resourceName">
        /// The name of the string resource that is locked-up.
        /// </param>
        public LocalizedCategoryAttribute( string resourceName )
            : base( EventStrings.ResourceManager.GetString( resourceName ) )
        {
        }
    }
}
