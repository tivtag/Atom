// <copyright file="XnaUpdateContext.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>Defines the Atom.Xna.XnaUpdateContext class.</summary>
// <author>Paul Ennemoser</author>

namespace Atom.Xna
{
    /// <summary>
    /// Defines a default implementation of the <see cref="IXnaUpdateContext"/> interface.
    /// </summary>
    public class XnaUpdateContext : IXnaUpdateContext
    {
        /// <summary>
        /// Gets or sets the current <see cref="Microsoft.Xna.Framework.GameTime"/>.
        /// </summary>
        /// <remarks>
        /// Must be manually updated each frame.
        /// </remarks>
        /// <value>
        /// Stores the timing information about the last frame.
        /// </value>
        public Microsoft.Xna.Framework.GameTime GameTime
        {
            get
            {
                return this.currentGameTime;
            }

            set
            {
                if( value != null )
                {
                    this.frameTime = (float)value.ElapsedGameTime.TotalSeconds;
                }
                else
                {
                    this.frameTime = 0.0f;
                }

                this.currentGameTime = value;
            }
        }

        /// <summary>
        /// Gets the time the last frame took in seconds.
        /// </summary>
        /// <value>
        /// The time (in seconds) the last frame took to execute.
        /// </value>
        public float FrameTime
        {
            get 
            {
                return this.frameTime; 
            }
        }

        /// <summary>
        /// Stores the Xna GameTime object of the current frame.
        /// </summary>
        private Microsoft.Xna.Framework.GameTime currentGameTime;

        /// <summary>
        /// Cached frame time value.
        /// </summary>
        private float frameTime;
    }
}
