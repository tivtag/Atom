// <copyright file="GraphicsDeviceExtensions.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Xna.GraphicsDeviceExtensions class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Xna
{
    using System;
    using Atom.Diagnostics.Contracts;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// Defines extension methods for the GraphicsDevice class.
    /// </summary>
    public static class GraphicsDeviceExtensions
    {
        /// <summary>
        /// Attempts to get the first RenderTarget2D.
        /// </summary>
        /// <param name="device">
        /// The device to query.
        /// </param>
        /// <returns>
        /// The requested render target or null.
        /// </returns>
        public static RenderTarget2D GetRenderTarget2D( this GraphicsDevice device )
        { 
            var rts = device.GetRenderTargets();
            return rts.Length > 0 ? (RenderTarget2D)rts[0].RenderTarget : null;
        }
    }
}
