// <copyright file="TimedEvent.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Events.TimedEvent class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Events
{
    using System.ComponentModel;
    using Atom.Events.Design;

    /// <summary>
    /// Defines an <see cref="Event"/> that when triggered
    /// triggers another <see cref="Event"/> after N seconds.
    /// </summary>
    public class TimedEvent : LongTermEvent
    {
        /// <summary>
        /// Gets or sets the time (in seconds) until the TimedEvent
        /// delegates the trigger call to the <see cref="TimedEvent.Event"/>.
        /// </summary>
        [LocalizedDisplayName( "PropDisp_TimedEvent_Time" )]
        [LocalizedCategory( "PropCate_Settings" )]
        [LocalizedDescription( "PropDesc_TimedEvent_Time" )]
        public float Time
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the <see cref="Event"/> that gets called
        /// <see cref="Time"/> seconds after triggering this TimedEvent.
        /// </summary>
        [LocalizedDisplayName( "PropDisp_TimedEvent_Event" )]
        [LocalizedCategory( "PropCate_Settings" )]
        [LocalizedDescription( "PropDesc_TimedEvent_Event" )]
        [Editor( "Atom.Events.Design.EventCreationEditor, Atom.Game.Design", "System.Drawing.Design.UITypeEditor, System.Windows.Forms" )]
        public Event Event
        {
            get;
            set;
        }

        /// <summary>
        /// Updates this TimedEvent.
        /// </summary>
        /// <param name="updateContext">
        /// The current <see cref="IUpdateContext"/>.
        /// </param>
        public override void Update( IUpdateContext updateContext )
        {
            this.timeLeft -= updateContext.FrameTime;

            if( this.timeLeft <= 0.0f )
            {
                if( this.Event != null )
                {
                    this.Event.Trigger( this.Object );
                }

                this.Stop();
            }
        }

        /// <summary>
        /// Called when this TimedEvent was triggered.
        /// </summary>
        /// <param name="obj">
        /// The object that triggered this TimedEvent.
        /// </param>
        /// <returns>
        /// Always returns true.
        /// </returns>
        protected override bool Triggering( object obj )
        {
            this.timeLeft = this.Time;
            return true;
        }

        /// <summary>
        /// Serializes this TimedEvent using the specified IEventSerializationContext.
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
            context.Write( this.Time );
            context.Write( this.Event != null ? this.Event.Name : string.Empty );
        }

        /// <summary>
        /// Deserializes this TimedEvent using the specified IEventDeserializationContext.
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
            ThrowHelper.InvalidVersion( version, CurrentVersion, "Atom.Events.TimedEvent" );

            this.Time  = context.ReadSingle();
            this.Event = context.GetEvent( context.ReadString() );
        }

        /// <summary>
        /// The time left until the TimedEvent delegates the trigger call.
        /// </summary>
        private float timeLeft;
    }
}
