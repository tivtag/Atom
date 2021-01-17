// <copyright file="IEventStorageContext.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Events.IEventStorageContext interface.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Events
{
    /// <summary>
    /// Defines the common interface both the <see cref="IEventSerializationContext"/> and <see cref="IEventDeserializationContext"/>
    /// share.
    /// </summary>
    public interface IEventStorageContext
    {
        /// <summary>
        /// Gets the EventManager that is currently de-/serialized.
        /// </summary>
        EventManager EventManager
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Event"/> with the given <paramref name="name"/>.
        /// </summary>
        /// <param name="name">
        /// The name that uniquely identifies the Event to reveive.
        /// </param>
        /// <returns>
        /// The requested Event;
        /// or null.
        /// </returns>
        Event GetEvent( string name );

        /// <summary>
        /// Gets the <see cref="EventTrigger"/> with the given <paramref name="name"/>.
        /// </summary>
        /// <param name="name">
        /// The name that uniquely identifies the EventTrigger to reveive.
        /// </param>
        /// <returns>
        /// The requested EventTrigger;
        /// or null.
        /// </returns>
        EventTrigger GetTrigger( string name );
    }
}
