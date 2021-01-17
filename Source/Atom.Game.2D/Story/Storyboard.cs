// <copyright file="Storyboard.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Story.Storyboard class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Story
{
    using System;
    using System.Collections.Generic;
    using Atom.Diagnostics.Contracts;
    using System.Linq;

    /// <summary>
    /// Combines multiple independent Timelines to lie on a single master timeline.
    /// </summary>
    /// <remarks>
    /// 
    ///                  +--------------->
    ///         +--------|----->
    ///     +---|--->    |   +--->
    /// ____|___|________|___|___________
    /// 
    /// 
    /// where ______ is the time Storyboard master timeline.
    /// and +------> individual timelines, that are placed on the master timeline
    /// using their starting time.
    /// </remarks>
    public class Storyboard : IUpdateable
    {
        /// <summary>
        /// Gets or sets a value indicating whether this Storyboard is currently active.
        /// </summary>
        /// <value>
        /// The default value is true.
        /// </value>
        public bool IsActive 
        {
            get
            {
                return this.isActive;
            }

            set
            {
                this.isActive = value;
            }
        }

        /// <summary>
        /// Gets or sets the current TimeTick that is displayed on the Storyboard.
        /// </summary>
        public TimeTick CurrentTick
        {
            get
            {
                return this.timer.Current;
            }

            set
            {
                this.timer.Current = value;

                foreach( var timeline in this.timelines )
                {
                    timeline.MoveTo( value );
                }
            }
        }

        /// <summary>
        /// Gets the number of Timelines this Storyboard contains.
        /// </summary>
        public int TimelineCount
        {
            get
            {
                return this.timelines.Count;
            }
        }

        /// <summary>
        /// Gets the Timelines that this Storyboard contains.
        /// </summary>
        public IEnumerable<Timeline> Timelines
        {
            get
            {
                return this.timelines.Select( walker => walker.Timeline );
            }
        }
        
        /// <summary>
        /// Adds the specified Timeline to this Storyboard.
        /// </summary>
        /// <remarks>
        /// One timeline should be added to only one Storyboard
        /// at a time or unexpected results might occour.
        /// </remarks>
        /// <param name="timeline">
        /// The Timeline to add to this Storyboard.
        /// </param>
        public void AddTimeline( Timeline timeline )
        {
            Contract.Requires<ArgumentNullException>( timeline != null );

            var timelineWalker = new TimelineWalker( timeline, this.timer );

            this.timelines.Add( timelineWalker );
        }

        /// <summary>
        /// Updates this Storyboard.
        /// </summary>
        /// <param name="updateContext">
        /// The current IUpdateContext.
        /// </param>
        public void Update( IUpdateContext updateContext )
        {
            if( this.isActive )
            {
                this.timer.Update( updateContext );
            }
        }

        /// <summary>
        /// Gets the timeline at the specified zero-based index.
        /// </summary>
        /// <param name="index">
        /// The zero-based index of the Timeline to get.
        /// </param>
        /// <returns>
        /// The requested Timeline.
        /// </returns>
        public Timeline GetTimelineAt( int index )
        {
            return this.timelines[index].Timeline;
        }
        
        /// <summary>
        /// The timer that controls the position of time within this Storyboard.
        /// </summary>
        private readonly TickTimer timer = new TickTimer();

        /// <summary>
        /// Stores the timelines (and their associated timeline walkers) pf this Storyboard.
        /// </summary>
        private readonly List<TimelineWalker> timelines = new List<TimelineWalker>();

        /// <summary>
        /// Represents the storage field of the IsActive property.
        /// </summary>
        private bool isActive = true;
    }
}
