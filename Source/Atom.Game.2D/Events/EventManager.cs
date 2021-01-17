// <copyright file="EventManager.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Events.EventManager class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Events
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using Atom.Diagnostics.Contracts;
    using System.Globalization;

    /// <summary>
    /// Manages and organises the updating and triggering of <see cref="Event"/>s.
    /// </summary>
    public partial class EventManager
    {
        #region [ Public ]

        #region > Events <

        /// <summary>
        /// Fired when an <see cref="Event"/> has been added to this <see cref="EventManager"/>.
        /// </summary>
        public event RelaxedEventHandler<EventManager, Event> EventAdded;

        /// <summary>
        /// Fired when an <see cref="Event"/> has been removed from this <see cref="EventManager"/>.
        /// </summary>
        public event RelaxedEventHandler<EventManager, Event> EventRemoved;

        /// <summary>
        /// Fired when an <see cref="EventTrigger"/> has been added to this <see cref="EventManager"/>.
        /// </summary>
        public event RelaxedEventHandler<EventManager, EventTrigger> TriggerAdded;

        /// <summary>
        /// Fired when an <see cref="EventTrigger"/> has been removed from this <see cref="EventManager"/>.
        /// </summary>
        public event RelaxedEventHandler<EventManager, EventTrigger> TriggerRemoved;

        #endregion

        #region > Properties <

        /// <summary>
        /// Gets a read-only collection that contains all <see cref="Event"/>s managed 
        /// by the <see cref="EventManager"/>.
        /// </summary>
        /// <value>
        /// A read-only collection that contains all <see cref="Event"/>s managed 
        /// by the <see cref="EventManager"/>.
        /// </value>
        public ICollection<Event> Events => events.Values;

        /// <summary>
        /// Gets a read-only collection that contains all <see cref="EventTrigger"/>s managed 
        /// by the <see cref="EventManager"/>.
        /// </summary>
        /// <value>
        /// A read-only collection that contains all <see cref="EventTrigger"/>s managed 
        /// by the <see cref="EventManager"/>.
        /// </value>
        public ICollection<EventTrigger> Triggers => triggers.Values;

        /// <summary>
        /// Gets or sets the delegate that determines whether it's needed for an <see cref="Event"/> to get saved.
        /// </summary>
        /// <exception cref="ArgumentNullException">Set: Given value is null.</exception>
        /// <value>
        /// The SaveDecider for <see cref="Event"/>s.
        /// </value>
        public Predicate<Event> EventSaveDecider
        {
            get
            {
                return eventSaveNeededDecider;
            }

            set
            {
                if( value == null )
                {
                    throw new ArgumentNullException( nameof( value ) );
                }

                this.eventSaveNeededDecider = value;
            }
        }

        /// <summary>
        /// Gets or sets the delegate that determines whether it's needed for an <see cref="EventTrigger"/> to get saved.
        /// </summary>
        /// <exception cref="ArgumentNullException">Set: Given value is null.</exception>
        /// <value>
        /// The SaveDecider for <see cref="EventTrigger"/>s.
        /// </value>
        public Predicate<EventTrigger> TriggerSaveDecider
        {
            get
            {
                return triggerSaveNeededDecider;
            }

            set
            {
                if( value == null )
                {
                    throw new ArgumentNullException( nameof( value ) );
                }

                this.triggerSaveNeededDecider = value;
            }
        }

        #endregion

        #region > Constructors <

        /// <summary>
        /// Initializes a new instance of the <see cref="EventManager"/> class.
        /// </summary>
        /// <param name="initialEventCapacity">
        /// The initial number of events the <see cref="EventManager"/> can store
        /// with allocating more memory.
        /// </param>
        /// <param name="initialTriggerCount">
        /// The initial number of event triggers the <see cref="EventManager"/> can store
        /// with allocating more memory.
        /// </param>
        /// <param name="eventSaveDecider">
        /// The delegate that determines whether it's needed for an event to get saved.
        /// </param>
        /// <param name="triggerSaveDecider">
        /// The delegate that determines whether it's needed for an trigger to get saved.
        /// </param>
        public EventManager(
            int initialEventCapacity,
            int initialTriggerCount,
            Predicate<Event> eventSaveDecider,
            Predicate<EventTrigger> triggerSaveDecider )
        {
            Contract.Requires<ArgumentNullException>( eventSaveDecider != null );
            Contract.Requires<ArgumentNullException>( triggerSaveDecider != null );

            this.events = new Dictionary<string, Event>( initialEventCapacity );
            this.triggers = new Dictionary<string, EventTrigger>( initialTriggerCount );

            this.eventSaveNeededDecider = eventSaveDecider;
            this.triggerSaveNeededDecider = triggerSaveDecider;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventManager"/> class.
        /// </summary>
        /// <remarks>
        /// Uses the default EventNeedsToBeSavedDelegate.
        /// </remarks>
        /// <param name="initialEventCapacity">
        /// The initial number of events the <see cref="EventManager"/> can store
        /// with allocating more memory.
        /// </param>
        /// <param name="initialTriggerCapacity">
        /// The initial number of triggers the <see cref="EventManager"/> can store
        /// with allocating more memory.
        /// </param>
        public EventManager( int initialEventCapacity, int initialTriggerCapacity )
            : this(
                initialEventCapacity,
                initialTriggerCapacity,
                DefaultEventSaveNeededDecider,
                DefaultTriggerSaveNeededDecider
            )
        {
        }

        #endregion

        #region > Methods <

        #region - Add -

        /// <summary>
        /// Adds the specified <see cref="Event"/> to the <see cref="EventManager"/>; without ever throwing an exception.
        /// </summary>
        /// <param name="e">
        /// The <see cref="Event"/> to add. Events can only be added to one <see cref="EventManager"/> at a time.
        /// </param>
        /// <returns>
        /// True if the event was added succesfully; -or- otherwise false if an exception has occurred.
        /// </returns>
        public bool TryAdd( Event e )
        {
            try
            {
                Add( e );
                return true;
            }
            catch( Exception ex )
            {
                Debug.WriteLine( ex );
                return false;
            }
        }

        /// <summary>
        /// Adds the specified <see cref="Event"/> to the <see cref="EventManager"/>.
        /// </summary>
        /// <param name="e">
        /// The <see cref="Event"/> to add. Events can only be added to one <see cref="EventManager"/> at a time.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="e"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If the given <see cref="Event"/> already has been added to an <see cref="EventManager"/>,
        /// or if the name of the given <see cref="Event"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If there already exists an Event within the <see cref="EventManager"/>
        /// that has the name of the given <see cref="Event"/>.
        /// </exception>
        public void Add( Event e )
        {
            // Validate
            ValidateEventForAddInternal( e );

            // Add
            this.events.Add( e.Name, e );
            e.EventManager = this;

            var permanentEvent = e as PermanentEvent;
            if( permanentEvent != null )
            {
                this.activePermanentEvents.Add( permanentEvent );
            }

            // Notify
            OnEventAdded( e );
            EventAdded?.Invoke( this, e );
        }

        /// <summary>
        /// Adds the specified <see cref="EventTrigger"/> to the <see cref="EventManager"/>.
        /// </summary>
        /// <param name="trigger">
        /// The <see cref="EventTrigger"/> to add. Events can only be added to one <see cref="EventManager"/> at a time.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="trigger"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If the given <see cref="EventTrigger"/> already has been added to an <see cref="EventManager"/>,
        /// or if the name of the given <see cref="EventTrigger"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If there already exists an EventTrigger within the <see cref="EventManager"/>
        /// that has the name of the given <see cref="EventTrigger"/>.
        /// </exception>
        public void Add( EventTrigger trigger )
        {
            // Validate
            ValidateTriggerForAddInternal( trigger );

            // Add
            this.triggers.Add( trigger.Name, trigger );
            trigger.EventManager = this;

            // Notify
            OnTriggerAdded( trigger );
            TriggerAdded?.Invoke( this, trigger );
        }

        #endregion

        #region - Remove -

        /// <summary>
        /// Tries to remove the <see cref="Event"/> with the given <paramref name="name"/>
        /// from this <see cref="EventManager"/>.
        /// </summary>
        /// <param name="name">
        /// The name of the Event to remove.
        /// </param>
        /// <returns>
        /// The Event which has been removed;
        /// or null if there exists no Event of the given <paramref name="name"/> in this <see cref="EventManager"/>.
        /// </returns>
        public Event RemoveEvent( string name )
        {
            Event e;
            if( events.TryGetValue( name, out e ) )
            {
                // Remove
                events.Remove( name );

                var permanentEvent = e as PermanentEvent;
                if( permanentEvent != null )
                {
                    this.activePermanentEvents.Remove( permanentEvent );
                }

                // Notify
                OnEventRemoved( e );
                EventRemoved?.Invoke( this, e );
                return e;
            }

            return null;
        }

        /// <summary>
        /// Tries to remove the <see cref="EventTrigger"/> with the given <paramref name="name"/>
        /// from this <see cref="EventManager"/>.
        /// </summary>
        /// <param name="name">
        /// The name of the EventTrigger to remove.
        /// </param>
        /// <returns>
        /// The EventTrigger which has been removed;
        /// or null if there exists no EventTrigger of the given <paramref name="name"/> in this <see cref="EventManager"/>.
        /// </returns>
        public EventTrigger RemoveTrigger( string name )
        {
            EventTrigger trigger;
            if( triggers.TryGetValue( name, out trigger ) )
            {
                // Remove
                triggers.Remove( name );

                // Notify
                OnTriggerRemoved( trigger );
                TriggerRemoved?.Invoke( this, trigger );

                return trigger;
            }

            return null;
        }

        #endregion

        #region - Contains -

        /// <summary>
        /// Gets whether the <see cref="EventManager"/> contains an Event
        /// that has the given <paramref name="name"/>.
        /// </summary>
        /// <param name="name">
        /// The name that uniquely identifies the Event to check for.
        /// </param>
        /// <returns>
        /// Returns true if the EventManager contains an event that has the given name; 
        /// otherwise false.
        /// </returns>
        public bool ContainsEvent( string name )
        {
            if( name == null )
            {
                return false;
            }

            return events.ContainsKey( name );
        }

        /// <summary>
        /// Gets whether the <see cref="EventManager"/> contains an EventTrigger
        /// that has the given <paramref name="name"/>.
        /// </summary>
        /// <param name="name">
        /// The name that uniquely identifies the EventTrigger to check for.
        /// </param>
        /// <returns>
        /// Returns true if the EventManager contains an event that has the given name;
        /// otherwise false.
        /// </returns>
        public bool ContainsTrigger( string name )
        {
            if( name == null )
            {
                return false;
            }

            return triggers.ContainsKey( name );
        }

        #endregion

        #region - Get -

        /// <summary>
        /// Tries to get the <see cref="Event"/> with the given unique <paramref name="name"/>.
        /// </summary>
        /// <param name="name">The name of the event to get.</param>
        /// <returns>
        /// The event; or null if no matching event has been found.
        /// </returns>
        public Event GetEvent( string name )
        {
            Event e;
            events.TryGetValue( name, out e );
            return e;
        }

        /// <summary>
        /// Tries to get the <see cref="EventTrigger"/> with the given unique <paramref name="name"/>.
        /// </summary>
        /// <param name="name">The name of the trigger to get.</param>
        /// <returns>
        /// The trigger; or null if no matching trigger has been found.
        /// </returns>
        public EventTrigger GetTrigger( string name )
        {
            EventTrigger trigger;
            triggers.TryGetValue( name, out trigger );
            return trigger;
        }

        /// <summary>
        /// Returns a new list that contains the Events that need to be saved.
        /// </summary>
        /// <returns>The list of events which need to be saved.</returns>
        public IList<Event> GetEventsToSave()
        {
            Dictionary<string, Event>.ValueCollection events = this.events.Values;
            var list = new List<Event>( events.Count );

            foreach( Event e in events )
            {
                if( this.eventSaveNeededDecider( e ) )
                {
                    list.Add( e );
                }
            }

            return list;
        }

        /// <summary>
        /// Returns a new list that contains the EventTriggers that need to be saved.
        /// </summary>
        /// <returns>The list of triggers which need to be saved.</returns>
        public IList<EventTrigger> GetTriggersToSave()
        {
            Dictionary<string, EventTrigger>.ValueCollection triggers = this.triggers.Values;
            var list = new List<EventTrigger>( triggers.Count );

            foreach( EventTrigger trigger in triggers )
            {
                if( this.triggerSaveNeededDecider( trigger ) )
                {
                    list.Add( trigger );
                }
            }

            return list;
        }

        #endregion

        #region - Update -

        /// <summary>
        /// Updates the <see cref="EventManager"/>, including all its active <see cref="LongTermEvent"/>s.
        /// </summary>
        /// <remarks>
        /// Includes updating of all active <see cref="LongTermEvent"/>s.
        /// </remarks>
        /// <param name="updateContext">
        /// The current <see cref="IUpdateContext"/>.
        /// </param>
        public virtual void Update( IUpdateContext updateContext )
        {
            for( int i = 0; i < activeLongTermEvents.Count; ++i )
            {
                activeLongTermEvents[i].Update( updateContext );
            }

            for( int i = 0; i < activePermanentEvents.Count; ++i )
            {
                activePermanentEvents[i].Update( updateContext );
            }
        }

        #endregion

        #region - Clear -

        /// <summary>
        /// Removes all Event Data from the <see cref="EventManager"/>.
        /// </summary>
        public virtual void Clear()
        {
            ClearEvents();
            ClearTriggers();
        }

        /// <summary>
        /// Removes all <see cref="Event"/>s from the <see cref="EventManager"/>.
        /// Also clears all active <see cref="LongTermEvent"/>s.
        /// </summary>
        public void ClearEvents()
        {
            ClearActiveLongTermEvents();

            foreach( Event e in events.Values )
            {
                e.EventManager = null;

                OnEventRemoved( e );
                EventRemoved?.Invoke( this, e );
            }

            this.events.Clear();
        }

        /// <summary>
        /// Removes all <see cref="EventTrigger"/>s from the <see cref="EventManager"/>.
        /// </summary>
        public void ClearTriggers()
        {
            foreach( EventTrigger trigger in triggers.Values )
            {
                trigger.EventManager = null;

                OnTriggerRemoved( trigger );
                TriggerRemoved?.Invoke( this, trigger );
            }

            this.triggers.Clear();
        }

        /// <summary>
        /// Clears the list of active <see cref="LongTermEvent"/> forcing
        /// all of them to stop.
        /// </summary>
        public void ClearActiveLongTermEvents()
        {
            foreach( LongTermEvent e in activeLongTermEvents )
            {
                e.InternalStopForce();
            }

            activeLongTermEvents.Clear();
        }

        #endregion

        #endregion

        #endregion

        #region [ Protected ]

        /// <summary>
        /// Called before an <see cref="Event"/> is about to be added.
        /// Provide custom code here to validate the Event. 
        /// </summary>
        /// <param name="e">
        /// The event to validate.
        /// </param>
        protected virtual void ValidateEventForAdd( Event e )
        {
        }

        /// <summary>
        /// Called before an <see cref="EventTrigger"/> is about to be added.
        /// Provide custom code here to validate the Trigger. 
        /// </summary>
        /// <param name="trigger">
        /// The trigger to validate.
        /// </param>
        protected virtual void ValidateTriggerForAdd( EventTrigger trigger )
        {
        }

        #region - Events -

        /// <summary>
        /// Called when an <see cref="Event"/> has been added to the <see cref="EventManager"/>.
        /// </summary>
        /// <param name="e">The event which has been added.</param>
        protected virtual void OnEventAdded( Event e )
        {
        }

        /// <summary>
        /// Called when an <see cref="Event"/> has been removed from the <see cref="EventManager"/>.
        /// </summary>
        /// <param name="e">The event which has been removed.</param>
        protected virtual void OnEventRemoved( Event e )
        {
        }

        /// <summary>
        /// Called when an <see cref="EventTrigger"/> has been added to the <see cref="EventManager"/>.
        /// </summary>
        /// <param name="trigger">The trigger which has been added.</param>
        protected virtual void OnTriggerAdded( EventTrigger trigger )
        {
        }

        /// <summary>
        /// Called when an <see cref="EventTrigger"/> has been removed from the <see cref="EventManager"/>.
        /// </summary>
        /// <param name="trigger">The trigger which has been removed.</param>
        protected virtual void OnTriggerRemoved( EventTrigger trigger )
        {
        }

        #endregion

        #endregion

        #region [ Internal ]

        /// <summary>
        /// Internal helper method that is used by Event.set_Name.
        /// </summary>
        /// <param name="e">The event whos name has changed.</param>
        /// <param name="oldName">The old name of the specified event.</param>
        internal void InternalInformEventNameHasChanged( Event e, string oldName )
        {
            bool removed = events.Remove( oldName );
            Debug.Assert( removed == true, "The Event has not been removed. Strange." );

            events.Add( e.Name, e );
        }

        /// <summary>
        /// Internal helper method that is used by EventTrigger.set_Name.
        /// </summary>
        /// <param name="trigger">The trigger whos name has changed.</param>
        /// <param name="oldName">The old name of the specified trigger.</param>
        internal void InternalInformTriggerNameHasChanged( EventTrigger trigger, string oldName )
        {
            bool removed = triggers.Remove( oldName );
            Debug.Assert( removed == true, "The EventTrigger has not been removed. Strange." );

            triggers.Add( trigger.Name, trigger );
        }

        /// <summary>
        /// Registers the given <see cref="LongTermEvent"/> at the EventManager.
        /// </summary>
        /// <param name="longTermEvent">The event that is about to be registered.</param>
        internal void RegisterLongTermEvent( LongTermEvent longTermEvent )
        {
            Debug.Assert( longTermEvent != null, "Given Event is null." );
            Debug.Assert( longTermEvent.EventManager == this, "The EventManager of the given Event is not equal to this." );
            Debug.Assert( activeLongTermEvents.Contains( longTermEvent ) == false, "This EventManager already contains the given Event." );

            activeLongTermEvents.Add( longTermEvent );
        }

        /// <summary>
        /// Unregisters the given <see cref="LongTermEvent"/> at the EventManager.
        /// </summary>
        /// <param name="longTermEvent">The event that is about to be unregistered.</param>
        internal void UnregisterLongTermEvent( LongTermEvent longTermEvent )
        {
            Debug.Assert( longTermEvent != null, "Given Event is null." );
            Debug.Assert( longTermEvent.EventManager == this, "The EventManager of the given Event is not equal to this." );
            Debug.Assert( activeLongTermEvents.Contains( longTermEvent ) == false, "This EventManager already contains the given Event." );

            activeLongTermEvents.Remove( longTermEvent );
        }

        #endregion

        #region [ Private ]

        #region > Methods <

        /// <summary>
        /// Called before an <see cref="Event"/> is about to be added.
        /// </summary>
        /// <param name="e">
        /// The event to validate.
        /// </param>
        private void ValidateEventForAddInternal( Event e )
        {
            if( e == null )
            {
                throw new ArgumentNullException( nameof( e ) );
            }

            if( e.EventManager != null )
            {
                throw new ArgumentException(
                    string.Format(
                        CultureInfo.CurrentUICulture,
                        EventStrings.Error_EventXAlreadyAddedToAnEventManager,
                        e.Name
                    ),
                    nameof( e )
                 );
            }

            if( e.Name == null )
            {
                throw new ArgumentException( EventStrings.Error_EventNameIsNull, nameof( e ) );
            }

            if( events.ContainsKey( e.Name ) )
            {
                throw new ArgumentException(
                    string.Format(
                        CultureInfo.CurrentUICulture,
                        EventStrings.Error_ThereAlreadyExistsAnEventNamedX,
                        e.Name
                    ),
                    nameof( e )
                );
            }

            ValidateEventForAdd( e );
        }

        /// <summary>
        /// Called before an <see cref="EventTrigger"/> is about to be added.
        /// </summary>
        /// <param name="trigger">
        /// The trigger to validate.
        /// </param>
        private void ValidateTriggerForAddInternal( EventTrigger trigger )
        {
            if( trigger == null )
            {
                throw new ArgumentNullException( nameof( trigger ) );
            }

            if( trigger.EventManager != null )
            {
                throw new ArgumentException(
                    string.Format(
                        CultureInfo.CurrentUICulture,
                        EventStrings.Error_EventTriggerXAlreadyAddedToAnEventManager,
                        trigger.Name
                    ),
                    nameof( trigger )
                );
            }

            if( trigger.Name == null )
            {
                throw new ArgumentException( EventStrings.Error_EventTriggerNameIsNull, "trigger" );
            }

            if( triggers.ContainsKey( trigger.Name ) )
            {
                throw new ArgumentException(
                    string.Format(
                        CultureInfo.CurrentUICulture,
                        EventStrings.Error_ThereAlreadyExistsAnEventTriggerNamedX,
                        trigger.Name
                    ),
                    nameof( trigger )
                );
            }

            ValidateTriggerForAdd( trigger );
        }

        /// <summary>
        /// Implements the DefaultEventSaveNeededDecider.
        /// </summary>
        /// <param name="e">The related Event.</param>
        /// <returns>
        /// Returns true if the given Event should be saved;
        /// otherwise false.
        /// </returns>
        private static bool DefaultEventSaveDecider_Method( Event e )
        {
            if( e == null )
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Implements the DefaultTriggerSaveNeededDecider.
        /// </summary>
        /// <param name="trigger">The related EventTrigger.</param>
        /// <returns>
        /// Returns true if the given EventTrigger should be saved;
        /// otherwise false.
        /// </returns>
        private static bool DefaultTriggerSaveDecider_Method( EventTrigger trigger )
        {
            if( trigger == null )
            {
                return false;
            }

            return true;
        }

        #endregion

        #region > Fields <

        /// <summary>
        /// The events that are managed by the <see cref="EventManager"/>,
        /// sorted by their (unique)-name.
        /// </summary>
        private readonly Dictionary<string, Event> events;

        /// <summary>
        /// The triggers that are managed by the <see cref="EventManager"/>,
        /// sorted by their (unique)-name.
        /// </summary>
        private readonly Dictionary<string, EventTrigger> triggers;

        /// <summary>
        /// The list of active <see cref="LongTermEvent"/>s.
        /// </summary>
        private readonly List<LongTermEvent> activeLongTermEvents = new List<LongTermEvent>();

        /// <summary>
        /// The list of active <see cref="PermanentEvent"/>s.
        /// </summary>
        private readonly List<PermanentEvent> activePermanentEvents = new List<PermanentEvent>();

        #region - Saving -

        /// <summary>
        /// The delegate that determines whether it's needed for an event to get saved.
        /// </summary>
        private Predicate<Event> eventSaveNeededDecider;

        /// <summary>
        /// The delegate that determines whether it's needed for an event trigger to get saved.
        /// </summary>
        private Predicate<EventTrigger> triggerSaveNeededDecider;

        /// <summary>
        /// Stores the EventNeedsToBeSaved delegate that is used by default. This is a read-only field.
        /// </summary>
        private static readonly Predicate<Event> DefaultEventSaveNeededDecider
            = new Predicate<Event>( DefaultEventSaveDecider_Method );

        /// <summary>
        /// Stores the EventTriggerNeedsToBeSaved delegate that is used by default. This is a read-only field.
        /// </summary>
        private static readonly Predicate<EventTrigger> DefaultTriggerSaveNeededDecider
            = new Predicate<EventTrigger>( DefaultTriggerSaveDecider_Method );

        #endregion

        #endregion

        #endregion
    }
}
