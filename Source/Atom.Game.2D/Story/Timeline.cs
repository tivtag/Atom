// <copyright file="Timeline.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Story.Timeline structure.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Story
{
    using System;
    using System.Collections.Generic;
    using Atom.Diagnostics.Contracts;
    using System.Linq;

    /// <summary>
    /// Represents an ordered series of <see cref="Incident"/>, 
    /// which are placed on a quantizied time scale.
    /// </summary>
    /// <seealso cref="TimeTick"/>
    public class Timeline
    {
        /// <summary>
        /// Gets or sets the name that (uniquely) identifies this Timeline.
        /// </summary>
        /// <remarks>
        /// The names of Timelines "must" only be unique within the same Storyboard.
        /// </remarks>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this Timeline is active.
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
        /// Gets or sets the offset this Timeline starts at,
        /// relative to the master <see cref="Storyboard"/>.
        /// </summary>
        public TimeTick StartOffset
        {
            get
            {
                return this.startOffset;
            }

            set
            {
                this.startOffset = value;

                foreach( var frame in this.frames )
                {
                    frame.SetStartOffset( value );                 
                }
            }
        }

        /// <summary>
        /// Gets the total number of incidents this Timeline contains.
        /// </summary>
        public int IncidentCount
        {
            get
            {
                return this.Incidents.Count();
            }
        }

        /// <summary>
        /// Gets the number of <see cref="Frame"/>s this Timeline is split in.
        /// </summary>
        public int FrameCount
        {
            get 
            {
                return this.frames.Count;
            }
        }
        
        /// <summary>
        /// Gets the Frames of this Timeline.
        /// </summary>
        public IEnumerable<Frame> Frames
        {
            get
            {
                return this.frames;
            }
        }

        /// <summary>
        /// Gets the incidents this Timeline contains.
        /// </summary>
        public IEnumerable<Incident> Incidents
        {
            get
            {
                return this.frames.SelectMany( frame => frame.Incidents );
            }
        }

        /// <summary>
        /// Gets the TimeTick at which this Timeline ends.
        /// </summary>
        public TimeTick LastActualTick
        {
            get
            {
                if( this.frames.Count == 0 )
                {
                    return TimeTick.Zero;
                }

                Frame frame = this.frames[this.frames.Count - 1];
                return frame.ActualTick;
            }
        }
        
        /// <summary>
        /// Rebuilds the internal structure of this Timeline;
        /// recreating all Frames.
        /// </summary>
        public void Rebuild()
        {
            List<Incident> incidents = this.Incidents.ToList();
            incidents.Sort( (x, y) => x.RelativeTick.CompareTo( y.RelativeTick ) );

            this.frames.Clear();

            Frame frame = null;
            TimeTick tick = new TimeTick();

            foreach( Incident incident in incidents )
            {
                TimeTick incidentTick = incident.RelativeTick;

                if( tick != incidentTick )
                {
                    tick = incidentTick;
                    frame = null;
                }

                if( frame == null )
                {
                    frame = new Frame( tick, this.startOffset );
                    this.frames.Add( frame );
                }

                frame.Incidents.Add( incident );
            }
        }

        /// <summary>
        /// Inserts the specified Incident into this Timeline.
        /// </summary>
        /// <param name="incident">
        /// The incident to insert.
        /// </param>
        public void Insert( Incident incident )
        {
            Contract.Requires<ArgumentNullException>( incident != null );

            Frame frame = this.FindRelativeFrame( incident.RelativeTick );

            if( frame == null )
            {
                frame = new Frame( incident.RelativeTick, this.startOffset );
                this.Insert( frame );
            }

            frame.Insert( incident );
        }

        /// <summary>
        /// Inserts the specified Frame into this Timeline.
        /// </summary>
        /// <param name="frame">
        /// The Frame to insert.
        /// </param>
        private void Insert( Frame frame )
        {
            TimeTick tick = frame.RelativeTick;

            for( int i = 0; i < frames.Count; ++i )
            {
                Frame otherFrame = this.frames[i];

                if( otherFrame.RelativeTick > tick )
                {                    
                    this.frames.Insert( i, frame );
                    return;
                }
            }

            this.frames.Add( frame );
        }

        /// <summary>
        /// Gets the Frame at the specified index.
        /// </summary>
        /// <param name="index">
        /// The zero-based index of the Frame to get.
        /// </param>
        /// <returns>
        /// The requested Frame.
        /// </returns>
        public Frame GetFrameAt( int index )
        {
            return this.frames[index];
        }

        /// <summary>
        /// Gets the Incident in the Frame at the specified frameIndex;
        /// at the specified incidentIndex.
        /// </summary>
        /// <param name="frameIndex">
        /// The zero-based index of the Frame that contains the Incident.
        /// </param>
        /// <param name="incidentIndex">
        /// The zero-based index into the Frame of the Incident to get.
        /// </param>
        /// <returns>
        /// The requested Incident.
        /// </returns>
        public Incident GetIncident( int frameIndex, int incidentIndex )
        {
            return this.frames[frameIndex].Incidents[incidentIndex];
        }

        /// <summary>
        /// Attempts to find the Frame at the specified relative TimeTick.
        /// </summary>
        /// <param name="tick">
        /// The relative time tick of the Frame to get.
        /// </param>
        /// <returns>
        /// The requested Frame; -or- null.
        /// </returns>
        private Frame FindRelativeFrame( TimeTick tick )
        {
            return this.frames.FirstOrDefault( frame => frame.RelativeTick == tick );
        }

        /// <summary>
        /// Stores the offset at which this timeline begins, relative
        /// to its master timeline.
        /// </summary>
        private TimeTick startOffset;

        /// <summary>
        /// The frames into which this Timeline has split.
        /// </summary>
        private readonly List<Frame> frames = new List<Frame>();

        /// <summary>
        /// Represents the storage field of the IsActive property.
        /// </summary>
        private bool isActive = true;
    }
}
