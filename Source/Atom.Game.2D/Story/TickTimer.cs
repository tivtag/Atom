// <copyright file="TickTimer.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Story.TickTimer class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Story
{
    using System;
    using Atom.Diagnostics.Contracts;

    /// <summary>
    /// Measures the flow of concrete tick time. 
    /// </summary>
    public class TickTimer : IUpdateable
    {
        /// <summary>
        /// Raised when the <see cref="Current"/> TimeTick of this TickTimer has changed.
        /// </summary>
        public event RelaxedEventHandler<TimeTick> Ticked = delegate { };

        /// <summary>
        /// Gets or sets the current <see cref="TimeTick"/> that has been recorded by this TickTimer.
        /// </summary>
        public TimeTick Current
        {
            get
            {
                return this.current;
            }

            set
            {
                this.current = value;
                this.timeUntilNextTick = TimeTick.TimeQuantity;
            }
        }
        
        /// <summary>
        /// Gets or sets a value indicating how real time flow influences the progress of tick time.
        /// </summary>
        public float Factor
        {
            get
            {
                return this.factor;
            }

            set
            {
                Contract.Requires<ArgumentException>( value >= 0.0f );
                this.factor = value;
            }
        }

        /// <summary>
        /// Updates this TickTimer.
        /// </summary>
        /// <param name="updateContext">
        /// The current IUpdateContext.
        /// </param>
        public void Update( IUpdateContext updateContext )
        {
            this.timeUntilNextTick -= updateContext.FrameTime * this.factor;

            if( this.timeUntilNextTick <= 0.0f )
            {
                this.UpdateOnTick();
            }
        }

        /// <summary>
        /// Called when enough real time has flown for atleast one time tick.
        /// </summary>
        private void UpdateOnTick()
        {
            do
            {
                this.OnTick();
                this.timeUntilNextTick += TimeTick.TimeQuantity;
            }
            while( this.timeUntilNextTick <= 0.0f );
        }

        /// <summary>
        /// Called when a TimeTick has occurred.
        /// </summary>
        private void OnTick()
        {
            this.current += 1;
            this.Ticked( this, this.current );
        }

        /// <summary>
        /// The time (in seconds) until the next TimeTick occurs.
        /// </summary>
        private float timeUntilNextTick = TimeTick.TimeQuantity;

        /// <summary>
        /// Represents the storage field of the Factor property.
        /// </summary>
        private float factor = 1.0f;

        /// <summary>
        /// Stores the current TimeTick.
        /// </summary>
        private TimeTick current;
    }
}
