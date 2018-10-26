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
    /// Represents a LongTermEvent that is controlled using Lambda functions.
    /// </summary>
    public class LongTermLambdaEvent : LongTermEvent
    {
        /// <summary>
        /// Gets or sets the action that is executed when this LongTermLambdaEvent is updated.
        /// </summary>
        public Action<LongTermLambdaEvent, IUpdateContext> Updated
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the function that decides whether this LongTermLambdaEvent can be
        /// triggered.
        /// </summary>
        public Func<LongTermLambdaEvent, object, bool> CanBeTriggered
        {
            get;
            set;
        }

        /// <summary>
        /// Updates this LongTermLambdaEvent.
        /// </summary>
        /// <param name="updateContext">
        /// The current IUpdateContext.
        /// </param>
        public override void Update( IUpdateContext updateContext )
        {
            if( this.Updated == null )
            {
                this.Updated( this, updateContext );
            }
        }

        /// <summary>
        /// Gets a value that indicates whether the 
        /// specified <see cref="Object"/> can trigger this <see cref="Event"/>.
        /// </summary>
        /// <param name="obj">
        /// The object to test.
        /// </param>
        /// <returns>
        /// Returns true if the object can trigger it; otherwise false.
        /// </returns>
        public override bool CanBeTriggeredBy( object obj )
        {
            if( this.CanBeTriggered == null )
            {
                return base.CanBeTriggeredBy(obj);
            }

            return this.CanBeTriggered( this, obj );
        }
    }
}
