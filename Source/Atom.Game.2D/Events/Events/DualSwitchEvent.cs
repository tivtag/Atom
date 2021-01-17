// <copyright file="DualSwitchEvent.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Events.DualSwitchEvent class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Events
{
    using System;
    using System.ComponentModel;
    using Atom.Events.Design;

    /// <summary>
    /// Represents an on/off switch which triggers <see cref="Event"/>s based on its current state.
    /// </summary>
    public class DualSwitchEvent : Event, ISwitchable
    {        
        /// <summary>
        /// Fired when the <see cref="IsSwitched"/> state of this DualSwitchEvent has changed.
        /// </summary>
        public event EventHandler IsSwitchedChanged;

        /// <summary>
        /// Gets or sets a value indicating whether this DualSwitchEvent
        /// is currently switched on or off.
        /// </summary>
        /// <value>The default value is true.</value>
        [DefaultValue( true )]
        [LocalizedDisplayName( "PropDisp_DSEvent_IsSwitched" )]
        [LocalizedCategory( "PropCate_Settings" )]
        [LocalizedDescription( "PropDesc_DSEvent_IsSwitched" )]
        public bool IsSwitched
        {
            get
            {
                return this.isSwitched;
            }

            set
            {
                if( value == this.isSwitched )
                {
                    return;
                }

                this.isSwitched = value;
                
                // Notify.
                this.IsSwitchedChanged.Raise( this );
                this.OnPropertyChanged( "IsSwitched" );
            }
        }
        
        /// <summary>
        /// Gets or sets a value indicating whether this DualSwitchEvent
        /// can currently be switched on or off.
        /// </summary>
        [DefaultValue( true )]
        [LocalizedDisplayName( "PropDisp_DSEvent_IsSwitchable" )]
        [LocalizedCategory( "PropCate_Settings" )]
        [LocalizedDescription( "PropDesc_DSEvent_IsSwitchable" )]
        public bool IsSwitchable
        {
            get 
            {
                return this.isSwitchable;
            }

            set 
            { 
                this.isSwitchable = value;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="Event"/> triggered when the <see cref="DualSwitchEvent"/> is enabled.
        /// </summary>
        /// <value>The default value is null.</value>
        [DefaultValue( null )]
        [LocalizedDisplayName( "PropDisp_DSEvent_EventOn" )]
        [LocalizedCategory( "PropCate_Settings" )]
        [LocalizedDescription( "PropDesc_DSEvent_EventOn" )]
        [Editor( "Atom.Events.Design.EventCreationEditor, Atom.Game.Design", "System.Drawing.Design.UITypeEditor, System.Windows.Forms" )]
        public Event EventWhenEnabled
        {
            get 
            {
                return this.eventOnState; 
            }

            set
            { 
                this.eventOnState = value;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="Event"/> triggered when the <see cref="DualSwitchEvent"/> is disabled.
        /// </summary>
        /// <value>The default value is null.</value>
        [DefaultValue( null )]
        [LocalizedDisplayName( "PropDisp_DSEvent_EventOff" )]
        [LocalizedCategory( "PropCate_Settings" )]
        [LocalizedDescription( "PropDesc_DSEvent_EventOff" )]
        [Editor( "Atom.Events.Design.EventCreationEditor, Atom.Game.Design", "System.Drawing.Design.UITypeEditor, System.Windows.Forms" )]
        public Event EventWhenDisabled
        {
            get 
            {
                return this.eventOffState;
            }

            set 
            { 
                this.eventOffState = value;
            }
        }

        /// <summary>
        /// Toggles the <see cref="DualSwitchEvent"/> from 'on to off' or from 'off to on'.
        /// </summary>
        /// <returns>The new state of the switch.</returns>
        public bool Toggle()
        {
            this.IsSwitched = !this.IsSwitched;
            return this.IsSwitched;
        }

        /// <summary>
        /// Gets a value indicating whether the specified <see cref="Object"/> can trigger 
        /// this DualSwitchEvent's current <see cref="Event"/> (based on the switch-state).
        /// </summary>
        /// <param name="obj">
        /// The object which wants to trigger this Event.
        /// </param>
        /// <returns>
        /// A value that indicates whether this Event can be triggered.
        /// </returns>
        public override bool CanBeTriggeredBy( object obj )
        {
            if( !this.IsSwitchable )
            {
                return false;
            }

            if( this.isSwitched )
            {
                return this.eventOnState == null ? true : this.eventOnState.CanBeTriggeredBy( obj );
            }
            else
            {
                return this.eventOffState == null ? true : this.eventOffState.CanBeTriggeredBy( obj );
            }
        }

        /// <summary>
        /// Triggers this <see cref="DualSwitchEvent"/> by redirecting the call 
        /// to the current <see cref="Event"/> based on the switch's current on/off state.
        /// </summary>
        /// <param name="obj">
        /// The object which has triggered the Event.
        /// </param>
        public override void Trigger( object obj )
        {
            if( this.isSwitched )
            {
                this.eventOnState?.Trigger( obj );
            }
            else
            {
                this.eventOffState?.Trigger( obj );
            }
        }
        
        /// <summary>
        /// Serializes this DualSwitchEvent using the specified IEventSerializationContext.
        /// </summary>
        /// <param name="context">
        /// The context under which the serialization process occurs.
        /// </param>
        public override void Serialize( IEventSerializationContext context )
        {
            base.Serialize( context );

            context.Write( this.isSwitched );
            context.Write( this.IsSwitchable );
            context.Write( this.eventOffState == null ? string.Empty : this.eventOffState.Name );
            context.Write( this.eventOnState == null  ? string.Empty : this.eventOnState.Name );
        }

        /// <summary>
        /// Deserializes this DualSwitchEvent using the specified IEventDeserializationContext.
        /// </summary>
        /// <param name="context">
        /// The context under which the deserialization process occurs.
        /// </param>
        public override void Deserialize( IEventDeserializationContext context )
        {
            base.Deserialize( context );

            this.isSwitched = context.ReadBoolean();
            this.isSwitchable = context.ReadBoolean();

            string eventOnName = context.ReadString();
            string eventOffName = context.ReadString();

            if( eventOnName.Length == 0 )
            {
                this.eventOnState = null;
            }
            else
            {
                this.eventOnState = context.GetEvent( eventOnName );
            }

            if( eventOffName.Length == 0 )
            {
                this.eventOffState = null;
            }
            else
            {
                this.eventOffState = context.GetEvent( eventOffName );
            }
        }

        /// <summary> 
        /// Specifies whether the DualSwitchEvent is currently set to on or off. 
        /// </summary>
        private bool isSwitched;

        /// <summary> 
        /// Specifies whether the DualSwitchEvent can be switched on or off.
        /// </summary>
        private bool isSwitchable = true;

        /// <summary>
        /// Identifies the event that is fired when the DualSwitchEvent is enabled.
        /// </summary>
        private Event eventOnState;

        /// <summary>
        /// Identifies the event that is fired when the DualSwitchEvent is disabled.
        /// </summary>
        private Event eventOffState;
    }
}