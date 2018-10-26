// <copyright file="EventTypeRegister.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Events.EventTypeRegister class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Events
{
    using System;
    using System.Collections.Generic;
    using Atom.Diagnostics.Contracts;

    /// <summary>
    /// Static class that registers supported <see cref="Event"/>s and <see cref="EventTrigger"/>s.
    /// </summary>
    public static class EventTypeRegister
    {
        /// <summary>
        /// Registers the given Type at the <see cref="EventTypeRegister"/>.
        /// </summary>
        /// <param name="type">The type to register.</param>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="type"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If <paramref name="type"/> is not or doesn't sub-class the type <see cref="Event"/>.
        /// </exception>
        public static void RegisterEvent( Type type )
        {
            Contract.Requires<ArgumentNullException>( type != null );
            Contract.Requires<ArgumentException>( typeof( Event ).IsAssignableFrom( type ) );

            if( eventRegister.Contains( type ) )
                return;
            eventRegister.Add( type );
        }

        /// <summary>
        /// Registers the given Type at the <see cref="EventTypeRegister"/>.
        /// </summary>
        /// <param name="type">The type to register.</param>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="type"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If <paramref name="type"/> is not or doesn't sub-class the type <see cref="EventTrigger"/>.
        /// </exception>
        public static void RegisterTrigger( Type type )
        {
            Contract.Requires<ArgumentNullException>( type != null );
            Contract.Requires<ArgumentException>( typeof( EventTrigger ).IsAssignableFrom( type ) );

            if( triggerRegister.Contains( type ) )
                return;
            triggerRegister.Add( type );
        }

        /// <summary>
        /// Registers the common Event Data types at the <see cref="EventTypeRegister"/>.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The following event types are registered:
        /// <see cref="NullEvent"/> and <see cref="DualSwitchEvent"/>
        /// </para>
        /// <para>
        /// The following trigger types are registered:
        /// <see cref="EventTrigger"/>, <see cref="AreaEventTrigger"/> and <see cref="TileAreaEventTrigger"/>
        /// </para>
        /// </remarks>
        public static void RegisterCommon()
        {
            eventRegister.Add( typeof( NullEvent ) );
            eventRegister.Add( typeof( DualSwitchEvent ) );
            eventRegister.Add( typeof( TimedEvent ) );
            eventRegister.Add( typeof( MultiEvent ) );            

            triggerRegister.Add( typeof( EventTrigger ) );
            triggerRegister.Add( typeof( AreaEventTrigger ) );
            triggerRegister.Add( typeof( TileAreaEventTrigger ) );
        }

        /// <summary>
        /// Gets a new array that contains all <see cref="Event"/> types which have been registered.
        /// </summary>
        /// <returns>
        /// A new array that contains all <see cref="Event"/> types which have been registered.
        /// </returns>
        public static Type[] GetEventTypes()
        {
            return eventRegister.ToArray();
        }

        /// <summary>
        /// Gets a new array that contains all <see cref="EventTrigger"/> types which have been registered.
        /// </summary>
        /// <returns>
        /// A new array that contains all <see cref="EventTrigger"/> types which have been registered.
        /// </returns>
        public static Type[] GetTriggerTypes()
        {
            return triggerRegister.ToArray();
        }

        /// <summary>
        /// Gets an enumerator over the <see cref="Event"/> types which 
        /// have been registered.
        /// </summary>
        /// <returns>
        /// The <see cref="IEnumerator{Type}"/> over <see cref="Event"/> types 
        /// which have been registered.
        /// </returns>
        public static IEnumerator<Type> GetEventEnumerator()
        {
            return eventRegister.GetEnumerator();
        }

        /// <summary>
        /// Gets an enumerator over the <see cref="EventTrigger"/> types which 
        /// have been registered.
        /// </summary>
        /// <returns>
        /// The <see cref="IEnumerator{Type}"/> over <see cref="EventTrigger"/> types 
        /// which have been registered.
        /// </returns>
        public static IEnumerator<Type> GetTriggerEnumerator()
        {
            return triggerRegister.GetEnumerator();
        }

        /// <summary>
        /// The list of types that have been registered as <see cref="Event"/>s at the EventTypeRegister.
        /// </summary>
        private static readonly List<Type> eventRegister   = new List<Type>( 20 );

        /// <summary>
        /// The list of types that have been registered as <see cref="EventTrigger"/>s at the EventTypeRegister.
        /// </summary>
        private static readonly List<Type> triggerRegister = new List<Type>( 10 );
    }
}
