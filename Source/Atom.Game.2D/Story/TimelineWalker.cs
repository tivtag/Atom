// <copyright file="TimelineWalker.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Story.TimelineWalker class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Story
{
    using System;
    using Atom.Diagnostics.Contracts;

    /// <summary>
    /// Implements a mechanism that walks on a Timeline based on the current TickTime.
    /// </summary>
    public class TimelineWalker : IDisposable
    {
        /// <summary>
        /// Raised when the TickTimer has progressed by one TimeTick.
        /// </summary>
        public event RelaxedEventHandler<TimeTick> Ticked
        {
            add
            {
                this.timer.Ticked += value;
            }

            remove
            {
                this.timer.Ticked -= value;
            }
        }

        /// <summary>
        /// Raised when a Frame has been executed.
        /// </summary>
        public event RelaxedEventHandler<Frame> FrameExecuted = delegate { };
                
        /// <summary>
        /// Gets the current absolute TickTime.
        /// </summary>
        public TimeTick CurrentTick
        {
            get
            {
                return this.timer.Current;
            }
        }

        /// <summary>
        /// Gets or sets the speed at which the timer progresses this TimelineWalker.
        /// </summary>
        public float WalkSpeed
        {
            get
            {
                return this.timer.Factor;
            }

            set
            {
                this.timer.Factor = value;
            }
        }

        /// <summary>
        /// Gets the Timeline this TimelineWalker walks on.
        /// </summary>
        public Timeline Timeline
        {
            get
            {
                return this.timeline;
            }
        }
        
        /// <summary>
        /// Initializes a new instance of the TimelineWalker class.
        /// </summary>
        /// <param name="timeline">
        /// The Timeline the new TimelineWalker walks on.
        /// </param>
        /// <param name="timer">
        /// The timer that is responsible for progressing TickTime.
        /// </param>
        public TimelineWalker( Timeline timeline, TickTimer timer )
        {
            Contract.Requires<ArgumentNullException>( timeline != null );
            Contract.Requires<ArgumentNullException>( timer != null );

            this.timeline = timeline;
            this.timer = timer;
            this.timer.Ticked += this.OnTicked;

            this.Rebuild();
        }

        internal void MoveTo( TimeTick value )
        {
            if( timeline.FrameCount == 0 )
                return;

            int currentFrameIndex = this.hasEnded ? this.timeline.FrameCount : this.nextFrameIndex - 1;
            Frame currentFrame = null;
            
            if( currentFrameIndex < 0 )
            {
                currentFrame = timeline.GetFrameAt( 0 );
            }
            else
            {
                currentFrame = timeline.GetFrameAt( currentFrameIndex );
            }
            
            if( value > currentFrame.ActualTick )
            {
                do
                {
                    currentFrame.Do();
                    ++currentFrameIndex;

                    if( currentFrameIndex >= timeline.FrameCount )
                        break;

                    currentFrame = timeline.GetFrameAt( currentFrameIndex );
                }
                while( value > currentFrame.ActualTick );
            }
            else if( value < currentFrame.ActualTick )
            {
                do
                {
                    currentFrame.Undo();
                    --currentFrameIndex;

                    if( currentFrameIndex < 0 )
                        break;

                    currentFrame = timeline.GetFrameAt( currentFrameIndex );
                }
                while( value < currentFrame.ActualTick );
            }

            this.Rebuild();
        }

        /// <summary>
        /// Rebuilds the internal structure of this TimelineWalker
        /// based on the currently set Timeline and the state of the TickTimer.
        /// </summary>
        public void Rebuild()
        {
            TimeTick tick = this.CurrentTick;

            for( int frameIndex = 0; frameIndex < this.timeline.FrameCount; ++frameIndex )
            {
                Frame frame = this.timeline.GetFrameAt( frameIndex );

                if( frame.ActualTick > tick )
                {
                    this.nextFrame = frame;
                    this.nextFrameIndex = frameIndex;
                    break;
                }
            }

            if( this.hasEnded )
            {
                if( tick < this.timeline.LastActualTick )
                {
                    this.hasEnded = false;
                }
                else
                {
                    this.hasEnded = true;
                }
            }
        }
        
        /// <summary>
        /// Called when the TickTimer has progressed by one TimeTick.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event.
        /// </param>
        /// <param name="tick">
        /// The current TimeTick.
        /// </param>
        private void OnTicked( object sender, TimeTick tick )
        {
            if( this.hasEnded )
                return;

            if( this.nextFrame == null )
            {
                this.OnEnded();
            }
            else
            {
                if( tick == this.nextFrame.ActualTick )
                {
                    this.MoveFrame();
                }
            }
        }

        /// <summary>
        /// Disposes this TimelineWalker; unregistering it form the TickTimer.
        /// </summary>
        public void Dispose()
        {
            this.timer.Ticked -= this.OnTicked;
        }

        /// <summary>
        /// Moves this TimelineWalker onward by one Frame.
        /// </summary>
        private void MoveFrame()
        {
            this.OnNextFrame();
            
            ++this.nextFrameIndex;

            if( this.nextFrameIndex >= this.timeline.FrameCount )
            {
                this.OnEnded();
            }
            else
            {
                this.nextFrame = this.timeline.GetFrameAt( this.nextFrameIndex );
            }
        }

        /// <summary>
        /// Called when the next Frame has been reached.
        /// </summary>
        private void OnNextFrame()
        {
            this.nextFrame.Do();
            this.FrameExecuted( this, this.nextFrame );
        }

        /// <summary>
        /// Called when this TimelineWalker has reached the end of the Timeline.
        /// </summary>
        private void OnEnded()
        {
            this.nextFrame = null;
            this.nextFrameIndex = -1;
            this.hasEnded = true;
        }

        /// <summary>
        /// The index into the timeline of the next Frame.
        /// </summary>
        private int nextFrameIndex;

        /// <summary>
        /// Captures the Frame that will be executed next.
        /// </summary>
        private Frame nextFrame;

        /// <summary>
        /// States whether the Timeline has finished.
        /// </summary>
        private bool hasEnded;

        /// <summary>
        /// The Timeline that is currently walked by this TimelineWalker.
        /// </summary>
        private readonly Timeline timeline;    

        /// <summary>
        /// Responsible for progressing TickTime.
        /// </summary>
        private readonly TickTimer timer;
    }
}
