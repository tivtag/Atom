// <copyright file="Frame.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Story.Frame class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Story
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents a single quantitized moment on a <see cref="Timeline"/>.
    /// </summary>
    /// <remarks>
    /// It can contain multiple <see cref="Incident"/>s that appear to take place
    /// on the same moment of tick time. The order of the incidents is not defined.
    /// </remarks>
    public sealed class Frame
    {
        /// <summary>
        /// Gets the TimeTick relative to the previous frame.
        /// </summary>
        public TimeTick RelativeTick
        {
            get
            {
                return this.relativeTick;
            }
        }

        /// <summary>
        /// Gets the TimeTick relative to the start of the timeline.
        /// </summary>
        public TimeTick ActualTick
        {
            get
            {
                return this.actualTick;
            }
        }

        /// <summary>
        /// Gets the <see cref="Incident"/> that occur on this Frame.
        /// </summary>
        public IList<Incident> Incidents
        {
            get
            {
                return this.incidents;
            }
        }

        /// <summary>
        /// Initializes a new instance of the Frame class.
        /// </summary>
        /// <param name="relativeTick">
        /// The TimeTick relative to the start of the parent Timeline.
        /// </param>
        /// <param name="startOffset">
        /// The offset this Timeline starts at, relative to the master <see cref="Storyboard"/>.
        /// </param>
        public Frame( TimeTick relativeTick, TimeTick startOffset )
        {
            this.relativeTick = relativeTick;
            this.SetStartOffset( startOffset );
        }

        /// <summary>
        /// Adds the given Incident to this Frame.
        /// </summary>
        /// <param name="incident">
        /// The Incident that should occur during this Frame.
        /// </param>
        public void Insert( Incident incident )
        {
            this.incidents.Add( incident );
        }

        /// <summary>
        /// Executes all incidents that occur during this Frame.
        /// </summary>
        public void Do()
        {
            for( int i = 0; i < this.incidents.Count; ++i )
            {
                this.incidents[i].Do();
            }
        }

        /// <summary>
        /// Reverses all incidents that occured during this Frame.
        /// </summary>
        public void Undo()
        {
            for( int i = 0; i < this.incidents.Count; ++i )
            {
                this.incidents[i].Undo();
            }
        }

        internal void SetStartOffset( TimeTick value )
        {
            this.actualTick = this.relativeTick + value;
        }

        private TimeTick actualTick;
        private readonly IList<Incident> incidents = new List<Incident>();
        private readonly TimeTick relativeTick;
    }
}
