// <copyright file="Incident.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Story.Incident class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Story
{
    using System;

    /// <summary>
    /// Represents a single 'event' that is placed on a Timeline.
    /// </summary>
    public class Incident
    {        
        /// <summary>
        /// Gets or sets the <see cref="TimeTick"/> this Incident occurs on.
        /// </summary>
        public TimeTick RelativeTick
        {
            get;
            set;
        }
        
        /// <summary>
        /// Gets or sets the action that is executed when this Incident occurs.
        /// </summary>
        public IAction Action 
        {
            get;
            set;
        }

        /// <summary>
        /// Executes this Incident.
        /// </summary>
        public void Do()
        {
            if( !this.state && this.Action != null )
            {
                this.Action.Execute();
            }

            this.state = true;
        }

        /// <summary>
        /// Reverses the actions of this Incident.
        /// </summary>
        public void Undo()
        {
            if( this.state && this.Action != null )
            {
                this.Action.Dexecute();
            }

            this.state = false;
        }

        /// <summary>
        /// Gets a value indicating whether this Incident lies before the given TimeTick.
        /// </summary>
        /// <param name="tick">
        /// The TimeTick to compare to.
        /// </param>
        /// <returns>
        /// True if it lies before the given TimeTick; -or- otherwise false.
        /// </returns>
        public bool IsBefore( TimeTick tick )
        {
            return this.RelativeTick < tick;
        }

        /// <summary>
        /// Gets a value indicating whether this Incident lies within the given TimeTick range.
        /// </summary>
        /// <param name="tick">
        /// The TimeTick to compare to.
        /// </param>
        /// <param name="tolerance">
        /// The allowed tolerance.
        /// </param>
        /// <returns>
        /// True if it lies within the given TimeTick range; -or- otherwise false.
        /// </returns>
        public bool IsWithin( TimeTick tick, TimeTick tolerance )
        {
            TimeTick start = this.RelativeTick - tolerance;
            TimeTick end = this.RelativeTick + tolerance;

            return start <= tick && tick <= end;
        }

        /// <summary>
        /// Gets a value indicating whether this Incident lies after the given TimeTick.
        /// </summary>
        /// <param name="tick">
        /// The TimeTick to compare to.
        /// </param>
        /// <returns>
        /// True if it lies after the given TimeTick; -or- otherwise false.
        /// </returns>
        public bool IsAfter( TimeTick tick )
        {
            return this.RelativeTick > tick;
        }

        /// <summary>
        /// States whether this Incident was executed yet.
        /// </summary>
        private bool state;
    }
}
