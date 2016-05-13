// <copyright file="EventSerializationContext.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Events.EventSerializationContext class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Events
{
    using System;
    using System.Diagnostics.Contracts;
    using System.IO;
    using Atom.Storage;

    /// <summary>
    /// Implements a mechanism that allows serialization of <see cref="Event"/>s, <see cref="EventTrigger"/>s,
    /// and <see cref="EventManager"/>s.
    /// </summary>
    [CLSCompliant( false )]
    public class EventSerializationContext : BinarySerializationContext, IEventSerializationContext
    { 
        /// <summary>
        /// Gets the <see cref="EventManager"/> that is currently serialized.
        /// </summary>
        /// <value>
        /// Is null when serializing an event that is not attached to an EventManager.
        /// </value>
        public EventManager EventManager
        {
            get
            {
                return this.eventManager;
            }
        }        

        /// <summary>
        /// Initializes a new instance of the EventSerializationContext class.
        /// </summary>
        /// <param name="eventManager">
        /// The EventManager that is going to be serialized.
        /// </param>
        /// <param name="writer">
        /// The BinaryWriter that is going to be written with.
        /// </param>
        public EventSerializationContext( EventManager eventManager, BinaryWriter writer )
            : base( writer )
        {
            this.eventManager = eventManager;
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
        public Event GetEvent( string name )
        {
            return this.eventManager.GetEvent( name );
        }

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
        public EventTrigger GetTrigger( string name )
        {
            return this.eventManager.GetTrigger( name );
        }

        /// <summary>
        /// The <see cref="EventManager"/> that is currently serialized.
        /// </summary>
        private readonly EventManager eventManager;
    }
}