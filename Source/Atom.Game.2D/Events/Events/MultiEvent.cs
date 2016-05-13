// <copyright file="MultiEvent.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Events.MultiEvent class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Events
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using Atom.Events.Design;

    /// <summary>
    /// Represents an <see cref="Event"/> that delegates all calls to <see cref="Trigger"/>
    /// to a list of other <see cref="Event"/>s.
    /// </summary>
    /// <remarks>
    /// Cyclic references won't be automatically detected.
    /// </remarks>
    public class MultiEvent : Event
    {
        #region [ Properties ]

        /// <summary>
        /// Gets the list of events this <see cref="MultiEvent"/> delegates all calls to <see cref="Trigger"/> over.
        /// </summary>
        [LocalizedDisplayName( "PropDisp_MultiEvent_Events" )]
        [LocalizedCategory( "PropCate_Settings" )]
        [LocalizedDescription( "PropDesc_MultiEvent_Events" )]
        [Editor( typeof( Atom.Events.Design.ExistingEventCollectionEditor ), typeof( System.Drawing.Design.UITypeEditor ) )]
        public IList<Event> Events
        {
            get 
            {
                return this.events;
            }
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Triggers this MultiEvent.
        /// </summary>
        /// <param name="obj">
        /// The object that has triggered this MultiEvent.
        /// </param>
        public override void Trigger( object obj )
        {
            if( !this.CanBeTriggeredBy( obj ) )
                return;

            for( int i = 0; i < this.events.Count; ++i )
            {
                var e = this.events[i];

                if( e != null )
                {
                    e.Trigger( obj );
                }
            }
        }

        /// <summary>
        /// Serializes this MultiEvent using the specified IEventSerializationContext.
        /// </summary>
        /// <param name="context">
        /// The context under which the serialization process occurs.
        /// </param>
        public override void Serialize( IEventSerializationContext context )
        {
            base.Serialize( context );

            // Header
            const int CurrentVersion = 1;
            context.Write( CurrentVersion );

            // Data
            context.Write( this.events.Count );

            for( int i = 0; i < this.events.Count; ++i )
            {
                var e = this.events[i];

                if( e != null )
                {
                    context.Write( e.Name );
                }
                else
                {
                    context.Write( string.Empty );
                }
            }      
        }

        /// <summary>
        /// Deserializes this MultiEvent using the specified IEventDeserializationContext.
        /// </summary>
        /// <param name="context">
        /// The context under which the deserialization process occurs.
        /// </param>
        public override void Deserialize( IEventDeserializationContext context )
        {
            base.Deserialize( context );

            // Header
            const int CurrentVersion = 1;
            int version = context.ReadInt32();
            ThrowHelper.InvalidVersion( version, CurrentVersion, "Atom.Events.MultiEvent" );

            // Data
            int count = context.ReadInt32();

            this.events.Clear();
            this.events.Capacity = count;

            for( int i = 0; i < count; ++i )
            {
                string eventName = context.ReadString();

                if( eventName.Length > 0 )
                {
                    var e = context.GetEvent( eventName );

                    if( e != null )
                    {
                        this.events.Add( e );
                    }
                    else
                    {
                        string message = string.Format(
                            System.Globalization.CultureInfo.CurrentUICulture,
                            EventStrings.Error_EventXDoesNotExistInEventManager,
                            eventName
                        );

                        GlobalServices.TryLog( message );
                    }
                }
                else
                {
                    this.events.Add( null );
                }
            }
        }

        #endregion

        #region [ Fields ]

        /// <summary>
        /// The list of events.
        /// </summary>
        private readonly List<Event> events = new List<Event>();

        #endregion
    }
}
