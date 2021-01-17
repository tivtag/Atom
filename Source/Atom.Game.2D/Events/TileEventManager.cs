// <copyright file="TileEventManager.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Events.TileEventManager class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Events
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Diagnostics;
    using Atom.Diagnostics.Contracts;
    using System.Globalization;
    using Atom.Scene.Tiles;

    /// <summary>
    /// Defines an <see cref="EventManager"/> that provides additional
    /// operations for tile based games and applications.
    /// </summary>
    [CLSCompliant( false )]
    public class TileEventManager : EventManager
    {
        #region [ Public ]
        
        /// <summary>
        /// Gets the <see cref="TileMap"/> this <see cref="TileEventManager"/> is associated with.
        /// </summary>
        /// <value>The TileMap object.</value>
        public TileMap Map
        {
            get 
            {
                return this.map;
            }
        }

        #region > Methods <

        /// <summary>
        /// Triggers all related TileAreaEventTriggers for the given Object.
        /// </summary>
        /// <param name="obj">
        /// The related object.
        /// </param>
        /// <param name="floorNumber">
        /// The number of the floor the given object is on.
        /// </param>
        /// <param name="area">
        /// The area the given object triggers TileAreaEventTrigger in.
        /// </param>
        public void TriggerEvents( object obj, int floorNumber, Atom.Math.Rectangle area )
        {
            if( floorNumber < 0 || floorNumber >= this.layeredAreaTriggers.Length )
                return;

            var areaTriggers = this.layeredAreaTriggers[floorNumber];
            TriggerContext context = new TriggerContext( this, obj );

            foreach( TileAreaEventTrigger trigger in areaTriggers )
            {
                if( trigger.WouldTriggerBy( context, ref area ) )
                {
                    trigger.Trigger( obj );
                }
            }
        }

        /// <summary>
        /// Gets all related TileAreaEventTriggers of type <typeparamref name="T"/> for the given Object
        /// in the given area.
        /// </summary>
        /// <typeparam name="T">The type of triggers to query.</typeparam>
        /// <param name="context">
        /// The context of execution.
        /// </param>
        /// <param name="floorNumber">
        /// The number of the floor the given object is on.
        /// </param>
        /// <param name="area">
        /// The area the given object triggers TileAreaEventTrigger in.
        /// </param>
        /// <returns>
        /// The triggers that would trigger.
        /// </returns>
        public IEnumerable<T> GetTriggers<T>( TriggerContext context, int floorNumber, Atom.Math.Rectangle area )
            where T : TileAreaEventTrigger
        {
            if( floorNumber < 0 || floorNumber >= this.layeredAreaTriggers.Length )
                yield break;

            var areaTriggers = this.layeredAreaTriggers[floorNumber];

            foreach( T trigger in areaTriggers.Where( t => t is T ).Cast<T>() )
            {
                if( trigger.WouldTriggerBy( context, ref area ) )
                {
                    yield return trigger;
                }
            }
        }

        #endregion

        #region > Constructors <

        /// <summary>
        /// Initializes a new instance of the <see cref="TileEventManager"/> class.
        /// </summary>
        /// <param name="map">The <see cref="TileMap"/> the new TileMapEventManager is associated with.</param>
        /// <param name="initialEventCapacity">
        /// The initial number of events the <see cref="EventManager"/> can store
        /// with allocating more memory.
        /// </param>
        /// <param name="initialTriggerCapacity">
        /// The initial number of event triggers the <see cref="EventManager"/> can store
        /// with allocating more memory.
        /// </param>
        /// <param name="eventSaveNeededDecider">
        /// The delegate that determines whether it's needed for an event to get saved.
        /// </param>
        /// <param name="triggerSaveNeededDecider">
        /// The delegate that determines whether it's needed for an trigger to get saved.
        /// </param>
        public TileEventManager( 
            TileMap map,
            int initialEventCapacity,
            int initialTriggerCapacity,
            Predicate<Event> eventSaveNeededDecider, 
            Predicate<EventTrigger> triggerSaveNeededDecider
        )
            : base( initialEventCapacity, initialTriggerCapacity, eventSaveNeededDecider, triggerSaveNeededDecider )
        {
            Contract.Requires<ArgumentNullException>( map != null );

            this.map = map;
            this.map.FloorsChanged += this.OnMapFloorsChanged;

            this.CreateAreaTriggerArray();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TileEventManager"/> class.
        /// </summary>
        /// <remarks>
        /// Uses the default NeedsToBeSavedDelegates.
        /// </remarks>
        /// <param name="map">The <see cref="TileMap"/> the TileMapEventManager is associated with.</param>
        /// <param name="initialEventCapacity">
        /// The initial number of events the <see cref="EventManager"/> can store
        /// with allocating more memory.
        /// </param>
        /// <param name="initialTriggerCapacity">
        /// The initial number of triggers the <see cref="EventManager"/> can store
        /// with allocating more memory.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// If the given <paramref name="map"/> is null.
        /// </exception>
        public TileEventManager( TileMap map, int initialEventCapacity, int initialTriggerCapacity )
            : base( initialEventCapacity, initialTriggerCapacity )
        {
            Contract.Requires<ArgumentNullException>( map != null );

            this.map = map;
            this.map.FloorsChanged += OnMapFloorsChanged;
            this.CreateAreaTriggerArray();
        }

        #endregion

        #endregion

        #region [ Protected ]

        /// <summary>
        /// Called when an <see cref="EventTrigger"/> has been added to the <see cref="EventManager"/>.
        /// </summary>
        /// <param name="trigger">
        /// The trigger which has been removed.
        /// </param>
        protected override void OnTriggerAdded( EventTrigger trigger )
        {
            var tileAreaTrigger = trigger as TileAreaEventTrigger;
            
            if( tileAreaTrigger != null )
            {
                int floor = tileAreaTrigger.FloorNumber;

                if( this.IsValidFloor( floor ) )
                {
                    layeredAreaTriggers[floor].Add( tileAreaTrigger );
                }
            }

            base.OnTriggerAdded( trigger );
        }

        /// <summary>
        /// Called when an <see cref="EventTrigger"/> has been removed to the <see cref="EventManager"/>.
        /// </summary>
        /// <param name="trigger">
        /// The trigger to remove.
        /// </param>
        protected override void OnTriggerRemoved( EventTrigger trigger )
        {
            var tileAreaTrigger = trigger as TileAreaEventTrigger;

            if( tileAreaTrigger != null )
            {
                int floor = tileAreaTrigger.FloorNumber;

                if( IsValidFloor( floor ) )
                    layeredAreaTriggers[floor].Remove( tileAreaTrigger );
            }

            base.OnTriggerRemoved( trigger );
        }

        /// <summary>
        /// Override to add special case validation for <see cref="TileAreaEventTrigger"/>s.
        /// </summary>
        /// <param name="trigger">
        /// The trigger to validate.
        /// </param>
        protected override void ValidateTriggerForAdd( EventTrigger trigger )
        {
            var tileAreaTrigger = trigger as TileAreaEventTrigger;

            // Do the validation:
            if( tileAreaTrigger != null )
            {
                if( !IsValidFloor( tileAreaTrigger.FloorNumber ) )
                {
                    throw new ArgumentException( 
                        string.Format( 
                            CultureInfo.CurrentCulture,
                            EventStrings.Error_FloorXIsInvalidFloorCountY,
                            tileAreaTrigger.FloorNumber.ToString( CultureInfo.CurrentCulture ),
                            this.FloorCount.ToString( CultureInfo.CurrentCulture )
                        ), 
                        "trigger"
                    );
                }
            }

            base.ValidateTriggerForAdd( trigger );
        }

        #endregion

        #region [ Internal ]

        /// <summary>
        /// Gets the number of floors this TileEventManager manages.
        /// </summary>
        /// <value>
        /// This value is linked to the FloorCount of the <see cref="TileMap"/>.
        /// </value>
        internal int FloorCount
        {
            get 
            {
                return this.layeredAreaTriggers.Length;
            }
        }

        /// <summary>
        /// Returns whether the given <paramref name="floorNumber"/> is valid.
        /// </summary>
        /// <param name="floorNumber">
        /// The number of the floor to validate.
        /// </param>
        /// <returns>
        /// Returns <see langword="true"/> if the given <paramref name="floorNumber"/> is valid;
        /// otherwise <see langword="false"/>.
        /// </returns>
        internal bool IsValidFloor( int floorNumber )
        {
            return floorNumber >= 0 && floorNumber <= layeredAreaTriggers.Length;
        }

        /// <summary>
        /// Internal method that get called when the Depth setting of a
        /// <see cref="TileAreaEventTrigger"/> has changed.
        /// </summary>
        /// <param name="trigger">The trigger that has changed.</param>
        /// <param name="oldFloor">The old floor number of the trigger.</param>
        internal void InternalInformTileAreaTriggerFloorHasChanged(
            TileAreaEventTrigger trigger,
            int oldFloor )
        {
            Debug.Assert( IsValidFloor( oldFloor ), "oldFloor is invalid." );

            var list = layeredAreaTriggers[oldFloor];

            // Remove
            bool result = list.Remove( trigger );
            Debug.Assert( result, "An error occurred while removing the trigger." );

            // Re-Add
            list = layeredAreaTriggers[trigger.FloorNumber];
            list.Add( trigger );
        }

        #endregion

        #region [ Private ]

        #region > Methods <

        /// <summary>
        /// Rebuild the layeredAreaTrigger array.
        /// </summary>
        private void RebuildAreaTriggerArray()
        {
            this.CreateAreaTriggerArray();

            for( int i = 0; i < this.layeredAreaTriggers.Length; ++i )
            {
                this.layeredAreaTriggers[i].Clear();
            }

            foreach( var trigger in this.Triggers )
            {
                var areaTrigger = trigger as TileAreaEventTrigger;

                if( areaTrigger != null )
                    this.layeredAreaTriggers[areaTrigger.FloorNumber].Add( areaTrigger );
            }
        }

        /// <summary>
        /// Creates, but doesn't setup the AreaTrigger Array.
        /// </summary>
        private void CreateAreaTriggerArray()
        {
            if( this.layeredAreaTriggers == null )
                this.layeredAreaTriggers = new List<TileAreaEventTrigger>[map.FloorCount];
            else
                Array.Resize<List<TileAreaEventTrigger>>( ref this.layeredAreaTriggers, map.FloorCount );

            for( int i = 0; i < this.layeredAreaTriggers.Length; ++i )
            {
                if( this.layeredAreaTriggers[i] == null )
                    this.layeredAreaTriggers[i] = new List<TileAreaEventTrigger>();
            }
        }

        /// <summary>
        /// Called when a TileMapFloor has been added to or
        /// removed from the underlying TileMap.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event.
        /// </param>
        private void OnMapFloorsChanged( TileMap sender )
        {
            this.RebuildAreaTriggerArray();
        }

        #endregion

        #region > Fields <

        /// <summary>
        /// The TileMap the TileMapEventManager is associated with.
        /// </summary>
        private readonly TileMap map;

        /// <summary>
        /// Stores the TileAreaEventTrigger sorted by the floor they correspond to.
        /// </summary>
        private List<TileAreaEventTrigger>[] layeredAreaTriggers;

        #endregion

        #endregion
    }
}
