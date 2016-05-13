// <copyright file="IRenderTarget2DFactory.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Xna.IRenderTarget2DFactory interfaces.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Xna
{
    using Atom.Math;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// Provides a mechanism that creates new instances of
    /// the RenderTarget2D class.
    /// </summary>
    public interface IRenderTarget2DFactory
    {
        /// <summary>
        /// Creates a new RenderTarget2D instance.
        /// </summary>
        /// <returns>
        /// A new RenderTarget2D instance.
        /// </returns>
        RenderTarget2D Create();

        /// <summary>
        /// Creates a new RenderTarget2D instance.
        /// </summary>
        /// <param name="sizeDivider">
        /// The divider that is applied on the size of the render target.
        /// </param>
        /// <returns>
        /// A new RenderTarget2D instance.
        /// </returns>
        RenderTarget2D Create( Point2 sizeDivider );
    }
}
