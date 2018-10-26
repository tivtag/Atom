// <copyright file="LongTermEvent.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Events.LongTermEvent class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Events
{
    using System;
    using System.ComponentModel;
    using Atom.Diagnostics.Contracts;

    /// <summary>
    /// Defines the base-class of all <see cref="Event"/>s that
    /// are active for a longer period of time.
    /// </summary>
    public abstract class LongTermEvent : Event
    {
        #region [ Events ]
        
        /// <summary>
        /// The events which are fired when the <see cref="LongTermEvent"/> has been triggered.
        /// </summary>
        public event SimpleEventHandler<LongTermEvent> Triggered;

        /// <summary> 
        /// The events which are fired when the <see cref="LongTermEvent"/> has stopped.
        /// </summary>
        public event SimpleEventHandler<LongTermEvent> Stopped;

        #endregion

        #region [ Properties ]

        /// <summary>
        /// Gets a value indicating whether this <see cref="LongTermEvent"/> is in a triggered state.
        /// </summary>
        /// <value>
        /// Returns true if this LongTermEvent is currently in a triggered state;
        /// otherwise false.
        /// </value>
        [Browsable( false )]
        public bool IsTriggered 
        { 
            get
            { 
                return this.isTriggered;
            }
        }

        /// <summary>
        /// Gets the object which has triggered this <see cref="LongTermEvent"/>.
        /// </summary>
        /// <value>
        /// The object which has triggered this <see cref="LongTermEvent"/>.
        /// </value>
        [Browsable(false)]
        public object Object
        {
            get 
            { 
                return this.obj;
            }
        }

        #endregion

        #region [ Methods ]

        /// <summary> 
        /// Triggers this <see cref="LongTermEvent"/>, 
        /// and as such starting it.
        /// </summary>
        /// <param name="obj">
        /// The object which has triggered the event.
        /// </param>
        /// <exception cref="InvalidOperationException">
        /// If the <see cref="LongTermEvent"/> has not yet been added to an <see cref="EventManager"/>.
        /// </exception>
        public sealed override void Trigger( object obj )
        {
            if( this.isTriggered )
                return;

#if DEBUG
            if( this.EventManager != null )
                throw new InvalidOperationException( Atom.Events.EventStrings.Error_LongTermEventRequiresEventManager );
#endif

            if( !this.Triggering( obj ) )
                return;

            this.isTriggered = true;
            this.obj = obj;
            
            this.Triggered.Raise( this );
            this.EventManager.RegisterLongTermEvent( this );
        }

        /// <summary>
        /// Stops this <see cref="LongTermEvent"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// If the <see cref="LongTermEvent"/> has not yet been added to an <see cref="EventManager"/>.
        /// </exception>
        public void Stop()
        {
            if( !this.isTriggered )
                return;

#if DEBUG
            if( this.EventManager != null )
                throw new InvalidOperationException( Atom.Events.EventStrings.Error_LongTermEventRequiresEventManager );
#endif

            if( !this.Stopping() )
                return;

            this.isTriggered = false;

            this.Stopped.Raise( this );
            this.obj = null;

            this.EventManager.UnregisterLongTermEvent( this );
        }

        /// <summary>
        /// Internal method that forces the <see cref="LongTermEvent"/> to stop.
        /// </summary>
        internal void InternalStopForce()
        {
            this.isTriggered = false;
            this.obj         = false;

            this.Stopping();
            
            if( this.Stopped != null )
            {
                this.Stopped( this );
            }
        }

        /// <summary>
        /// Updates this <see cref="LongTermEvent"/>.
        /// This method is called once per frame
        /// on triggered <see cref="LongTermEvent"/>s.
        /// </summary>
        /// <param name="updateContext">
        /// The current IUpdateContext.
        /// </param>
        public abstract void Update( IUpdateContext updateContext );

        /// <summary>
        /// Called when the event is stopping. 
        /// Can be overriten to include custom operations.
        /// </summary>
        /// <returns>
        /// True if the LongTermEvent should be stopped; otherwise false.
        /// </returns>
        protected virtual bool Stopping() 
        { 
            return true; 
        }

        /// <summary>
        /// Called when the event is triggered. 
        /// Can be overriten to include custom operations (for example for initialization).
        /// </summary>
        /// <param name="obj">The object that is about to trigger the event.</param>
        /// <returns>
        /// True if the LongTermEvent should be triggered; otherwise false.
        /// </returns>
        protected virtual bool Triggering( object obj ) 
        {
            return true;
        }

        #endregion

        #region [ Fields ]

        /// <summary>
        /// The object which has triggered this <see cref="LongTermEvent"/>. If any.
        /// </summary>
        private object obj;

        /// <summary>
        /// States whether this <see cref="LongTermEvent"/> is in a triggered state.
        /// </summary>
        private bool isTriggered;

        #endregion
    }
}
