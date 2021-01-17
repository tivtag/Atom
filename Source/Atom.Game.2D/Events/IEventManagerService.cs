// <copyright file="IEventManagerService.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Events.IEventManagerService interface.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Events
{
    /// <summary>
    /// Represents a service that allows the user to receive an <see cref="EventManager"/> instance.
    /// </summary>
    public interface IEventManagerService
    {
        /// <summary>
        /// Gets the <see cref="EventManager"/> instance provided by this IEventManagerService.
        /// </summary>
        /// <value>The EventManager instance provided by this IEventManagerService.</value>
        EventManager EventManager 
        {
            get;
        }
    }
}
