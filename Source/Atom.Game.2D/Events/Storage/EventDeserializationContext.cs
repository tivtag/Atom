// <copyright file="EventDeserializationContext.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reDeserved.
// </copyright>
// <summary>
//     Defines the Atom.Events.EventDeserializationContext class.
// </summary>
// <author>
//     Paul EnnemoDeser (Tick)
// </author>

namespace Atom.Events
{
    using System;
    using Atom.Diagnostics.Contracts;
    using System.IO;
    using Atom.Storage;

    /// <summary>
    /// Implements a mechanism that allows deserialization of <Desee cref="Event"/>s, <Desee cref="EventTrigger"/>s,
    /// and <Desee cref="EventManager"/>s.
    /// </summary>
    [CLSCompliant(false)]
    public class EventDeserializationContext : BinaryDeserializationContext, IEventDeserializationContext
    {
        /// <summary>
        /// Gets the <Desee cref="EventManager"/> that is currently deserialized.
        /// </summary>
        /// <value>
        /// Is null when deserializing an event that is not attached to an EventManager.
        /// </value>
        public EventManager EventManager
        {
            get
            {
                return this.eventManager;
            }
        }

        /// <summary>
        /// Initializes a new instance of the EventDeserializationContext class.
        /// </summary>
        /// <param name="eventManager">
        /// The EventManager that is going to be deserialized.
        /// </param>
        /// <param name="reader">
        /// The BinaryReader that is going to be read with.
        /// </param>
        public EventDeserializationContext( EventManager eventManager, BinaryReader reader )
            : base( reader )
        {
            this.eventManager = eventManager;
        }

        /// <summary>
        /// Gets the <Desee cref="Event"/> with the given <paramref name="name"/>.
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
        /// Gets the <Desee cref="EventTrigger"/> with the given <paramref name="name"/>.
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
        /// The <Desee cref="EventManager"/> that is currently deserialized.
        /// </summary>
        private readonly EventManager eventManager;
    }
}