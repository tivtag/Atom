// <copyright file="IXnaDrawContext.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Xna.IXnaDrawContext interface.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Xna
{
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// Represents an <see cref="IDrawContext"/> that is supposed to be used in
    /// a Xna application.
    /// </summary>
    public interface IXnaDrawContext : IDrawContext, IXnaContext
    {
        /// <summary>
        /// Gets the Xna GraphicsDevice object.
        /// </summary>
        GraphicsDevice Device
        {
            get;
        }
    }
}
