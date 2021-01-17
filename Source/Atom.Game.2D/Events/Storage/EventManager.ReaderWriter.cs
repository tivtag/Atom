// <copyright file="EventManager.ReaderWriter.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Events.EventManager.ReaderWriter class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Events
{
    using System;
    using System.Collections.Generic;
    using Atom.Diagnostics.Contracts;

    /// <content>
    /// Contains the ReaderWriter that allows serialization and deserialization of <see cref="EventManager"/>s.
    /// </content>
    public partial class EventManager
    {
        /// <summary>
        /// Implements a mechanism for serializing and deserializing of <see cref="EventManager"/>s.
        /// </summary>
        public class ReaderWriter
        {
            /// <summary>
            /// Gets the <see cref="ITypeActivator"/> that is used to create instances
            /// of de-serialized Events and EventTriggers.
            /// </summary>
            protected ITypeActivator TypeActivator
            {
                get
                {
                    return this.typeActivator;
                }
            }

            /// <summary>
            /// Initializes a new instance of the ReaderWriter class.
            /// </summary>
            /// <param name="typeActivator">
            /// Provides a mechanism that is used to create instances of de-serialized Events and EventTriggers.
            /// </param>
            public ReaderWriter( ITypeActivator typeActivator )
            {
                Contract.Requires<ArgumentNullException>( typeActivator != null );

                this.typeActivator = typeActivator;
            }

            #region [ Serialization ]

            /// <summary>
            /// Fully serializes the <see cref="EventManager"/> provided
            /// with the specified <see cref="IEventSerializationContext"/>.
            /// </summary>
            /// <param name="context">
            /// The context under which the serialization operation occurs.
            /// </param>
            public void Serialize( IEventSerializationContext context )
            {
                Contract.Requires<ArgumentNullException>( context != null );

                var events = context.EventManager.GetEventsToSave();
                var triggers = context.EventManager.GetTriggersToSave();

                this.WriteHeader( events, triggers, context );
                this.WriteBody( events, triggers, context );
            }

            /// <summary>
            /// Writes the document header that descripes the content of the EventManager. 
            /// </summary>
            /// <param name="events">
            /// The <see cref="Event"/>s that should be serialized.
            /// </param>
            /// <param name="triggers">
            /// The <see cref="EventTrigger"/>s that should be serialized.
            /// </param>
            /// <param name="context">
            /// The context under which the serialization operation occurs.
            /// </param>
            public void WriteHeader( IList<Event> events, IList<EventTrigger> triggers, IEventSerializationContext context )
            {
                // 1th write File CurrentVersion
                const int CurrentVersion = 1;
                context.Write( CurrentVersion );

                // write the number of events that need to be saved.
                context.Write( events.Count );

                // write event header data:
                foreach( Event e in events )
                {
                    context.Write( e.TypeName );
                    context.Write( e.Name );
                }

                // write the number of triggers that need to be saved.
                context.Write( triggers.Count );

                // write event header data:
                foreach( EventTrigger trigger in triggers )
                {
                    context.Write( trigger.TypeName );
                    context.Write( trigger.Name );
                }

                this.WriteAdditionalHeader( context );
            }

            /// <summary>
            /// Writes the document body that contains the actual <see cref="Event"/> and <see cref="EventTrigger"/>
            /// data.
            /// </summary>
            /// <param name="events">
            /// The <see cref="Event"/>s that should be serialized.
            /// </param>
            /// <param name="triggers">
            /// The <see cref="EventTrigger"/>s that should be serialized.
            /// </param>
            /// <param name="context">
            /// The context under which the serialization operation occurs.
            /// </param>
            public void WriteBody( IList<Event> events, IList<EventTrigger> triggers, IEventSerializationContext context )
            {
                // Write Events
                {
                    context.Write( events.Count );

                    foreach( Event e in events )
                    {
                        e.Serialize( context );
                    }
                }

                // Write Triggers
                {
                    context.Write( triggers.Count );

                    foreach( EventTrigger trigger in triggers )
                    {
                        trigger.Serialize( context );
                    }
                }

                this.WriteAdditionalBody( context );
            }

            /// <summary>
            /// Method that can be overriden by sub-classes that want to write
            /// additional header data.
            /// </summary>
            /// <param name="context">
            /// The context under which the serialization operation occurs.
            /// </param>
            protected virtual void WriteAdditionalHeader( IEventSerializationContext context )
            {
            }

            /// <summary>
            /// Method that can be overriden by sub-classes that want to write
            /// additional body data.
            /// </summary>
            /// <param name="context">
            /// The context under which the serialization operation occurs.
            /// </param>
            protected virtual void WriteAdditionalBody( IEventSerializationContext context )
            {
            }

            #endregion

            #region [ Deserialization ]

            /// <summary>
            /// Fully deserializes the <see cref="EventManager"/> provided
            /// with the specified <see cref="IEventDeserializationContext"/>.
            /// </summary>
            /// <param name="context">
            /// The context under which the deserialization operation occurs.
            /// </param>
            public void Deserialize( IEventDeserializationContext context )
            {
                Contract.Requires<ArgumentNullException>( context != null );

                this.ReadHeader( context );
                this.ReadBody( context );
            }

            /// <summary>
            /// Reads the document header that descripes the content of the EventManager.
            /// </summary>
            /// <param name="context">
            /// The context under which the deserialization operation occurs.
            /// </param>
            public void ReadHeader( IEventDeserializationContext context )
            {
                // 1th, validate version.
                const int CurrentVersion = 1;
                int version = context.ReadInt32();

                ThrowHelper.InvalidVersion( version, CurrentVersion, this.GetType() );

                // - Read Event Header:
                var eventManager = context.EventManager;
                {
                    // read Event Count
                    int eventCount = context.ReadInt32();

                    // read and setup event header data 
                    for( int i = 0; i < eventCount; ++i )
                    {
                        string eventTypeName = context.ReadString();
                        string eventName     = context.ReadString();

                        // This allows existing events to replace events
                        // within the EventDocument.
                        if( eventManager.ContainsEvent( eventName ) )
                            continue;

                        Event e = (Event)this.typeActivator.CreateInstance( eventTypeName );

                        e.Name = eventName;
                        eventManager.Add( e );
                    }
                }

                // - Read Trigger Header:
                {
                    int triggerCount = context.ReadInt32();

                    // Read and setup trigger header data.
                    for( int i = 0; i < triggerCount; ++i )
                    {
                        string triggerTypeName = context.ReadString();
                        string triggerName     = context.ReadString();

                        // This allows existing triggers to replace triggers
                        // within the EventDocument.
                        if( eventManager.ContainsTrigger( triggerName ) )
                            continue;

                        EventTrigger trigger = (EventTrigger)this.typeActivator.CreateInstance( triggerTypeName );

                        trigger.Name = triggerName;
                        eventManager.Add( trigger );
                    }
                }

                this.ReadAdditionalHeader( context );
            }

            /// <summary>
            /// Reads the document body that contains the actual Event and EventTrigget data.
            /// </summary>
            /// <param name="context">
            /// The context under which the deserialization operation occurs.
            /// </param>
            public void ReadBody( IEventDeserializationContext context )
            {
                var eventManager = context.EventManager;

                // Read Events:
                {
                    int eventCount = context.ReadInt32();

                    for( int i = 0; i < eventCount; ++i )
                    {
                        /* string eventTypeName */ context.ReadString();
                        string eventName = context.ReadString();

                        Event e = eventManager.GetEvent( eventName );
                        e.Deserialize( context );
                    }
                }

                // Read Triggers:
                {
                    int triggerCount = context.ReadInt32();

                    for( int i = 0; i < triggerCount; ++i )
                    {
                        /* string triggerTypeName */ context.ReadString();
                        string triggerName = context.ReadString();

                        EventTrigger trigger = eventManager.GetTrigger( triggerName );
                        trigger.Deserialize( context );
                    }
                }

                this.ReadAdditionalBody( context );
            }

            /// <summary>
            /// Method that can be overriden by sub-classed that want to read
            /// additional header data.
            /// </summary>
            /// <param name="context">
            /// The <see cref="IEventDeserializationContext"/> object that is used to read the data.
            /// </param>
            protected virtual void ReadAdditionalHeader( IEventDeserializationContext context )
            {
            }

            /// <summary>
            /// Method that can be overriden by sub-classed that want to read
            /// additional body data.
            /// </summary>
            /// <param name="context">
            /// The <see cref="IEventDeserializationContext"/> object that is used to read the data.
            /// </param>
            protected virtual void ReadAdditionalBody( IEventDeserializationContext context )
            {
            }

            #endregion

            /// <summary>
            /// Provides a mechanism that is used to create instances of de-serialized Events and EventTriggers.
            /// </summary>
            private readonly ITypeActivator typeActivator;
        }
    }
}
