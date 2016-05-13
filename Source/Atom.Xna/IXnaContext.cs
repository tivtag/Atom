// <copyright file="IXnaContext.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Xna.IXnaContext interface.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Xna
{
    /// <summary>
    /// Represents the common base interface that both the <see cref="IXnaDrawContext"/> and
    /// the <see cref="IXnaUpdateContext"/> share.
    /// </summary>
    public interface IXnaContext
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
        Microsoft.Xna.Framework.GameTime GameTime
        {
            get;
            set;
        }
    }
}
