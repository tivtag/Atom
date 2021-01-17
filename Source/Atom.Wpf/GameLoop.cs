// <copyright file="GameLoop.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Wpf.GameLoop class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Wpf
{
    using System;
    using System.Windows.Threading;

    /// <summary>
    /// The GameLoop class allows the usage of a standard
    /// game-loop in a Windows Presentation Foundation application.
    /// This class can't be inherited.
    /// </summary>
    public sealed class GameLoop
    {
        #region [ Events ]

        /// <summary>
        /// Raised when this GameLoop is updating.
        /// </summary>
        public event GameLoopUpdateEventHandler Updated;

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Initializes a new instance of the <see cref="GameLoop"/> class.
        /// </summary>
        public GameLoop()
            : this( new TimeSpan( 0, 0, 0, 0, 1 ), DispatcherPriority.Normal, Dispatcher.CurrentDispatcher )
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GameLoop"/> class.
        /// </summary>
        /// <param name="interval">
        /// The period of time between ticks; can be used limit the frame rate.
        /// </param>
        /// <param name="priority">
        /// The priority at which to invoke the GameLoop.
        /// </param>
        /// <param name="dispatcher">
        /// The dispatcher the timer is associated with.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// If the <paramref name="dispatcher"/> is null.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// If the <paramref name="interval"/> is less than 0 or greater than <see cref="System.Int32.MaxValue"/>.
        /// </exception>
        public GameLoop( TimeSpan interval, DispatcherPriority priority, Dispatcher dispatcher )
        {
            this.frameTimer = new DispatcherTimer( interval, priority, this.OnTick, dispatcher ); 
            this.Start();
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Starts this GameLoop.
        /// </summary>
        public void Start()
        {
            this.isActive = true;
            this.frameTimer.Start();
        }

        /// <summary>
        /// Stops this GameLoop.
        /// </summary>
        public void Stop()
        {
            this.isActive = false;
            this.frameTimer.Stop();
        }
        
        /// <summary>
        /// Called once per frame by the DispatcherTimer.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The EventArgs that contain the event data.</param>
        private void OnTick( object sender, EventArgs e )
        {
            if( !this.isActive )
                return;

            DateTime now = DateTime.Now;
            this.elapsedTime = now - this.lastUpdateTime;
            this.lastUpdateTime = now;

            if( this.Updated != null )
            {
                this.Updated( elapsedTime );
            }
        }

        #endregion

        #region [ Fields ]

        /// <summary>
        /// States whether this GameLoop is currently active.
        /// </summary>
        private bool isActive;

        /// <summary>
        /// Stores the TimeSpan the last frame took to execute.
        /// </summary>
        private TimeSpan elapsedTime;

        /// <summary>
        /// Stores the time at which the last frame has ended.
        /// </summary>
        private DateTime lastUpdateTime = DateTime.MinValue;

        /// <summary>
        /// The timer responsible for calling OnTick.
        /// </summary>
        private readonly System.Windows.Threading.DispatcherTimer frameTimer;

        #endregion
    }
}
