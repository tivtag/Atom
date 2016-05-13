// <copyright file="RenderTarget2DFactory.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Xna.RenderTarget2DFactory class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Xna
{
    using System;
    using System.Diagnostics.Contracts;
    using Atom.Math;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// Implements an <see cref="IRenderTarget2DFactory"/> that creates RenderTarget2D instances
    /// with a specific size and SurfaceFormat.
    /// </summary>
    public class FullscreenRenderTarget2DFactory : IRenderTarget2DFactory
    {
        /// <summary>
        /// Gets or sets the SurfaceFormat of the RenderTarget2Ds created by this RenderTarget2DFactory.
        /// </summary>
        public SurfaceFormat Format
        {
            get
            {
                return this.format;
            }

            set
            {
                this.format = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the FullscreenRenderTarget2DFactory class.
        /// </summary>
        /// <param name="graphicsDeviceService">
        /// Provides access to the xna GraphiceDevice that is required by the new RenderTarget2DFactory
        /// to create RenderTarget2D instances.
        /// </param>
        public FullscreenRenderTarget2DFactory( IGraphicsDeviceService graphicsDeviceService )
        {
            Contract.Requires<ArgumentNullException>( graphicsDeviceService != null );

            this.graphicsDeviceService = graphicsDeviceService;
        }

        /// <summary>
        /// Creates a new RenderTarget2D instance.
        /// </summary>
        /// <returns>
        /// A new RenderTarget2D instance.
        /// </returns>
        public RenderTarget2D Create()
        {
            var device = this.graphicsDeviceService.GraphicsDevice;
            var viewport = device.Viewport;

            return new RenderTarget2D(
                device,
                viewport.Width,
                viewport.Height,
                false,
                this.format,
                DepthFormat.None,
                0,
                RenderTargetUsage.PreserveContents
            );
        }

        /// <summary>
        /// Creates a new RenderTarget2D instance.
        /// </summary>
        /// <param name="sizeDivider">
        /// The divider that is applied on the size of the render target.
        /// </param>
        /// <returns>
        /// A new RenderTarget2D instance.
        /// </returns>
        public RenderTarget2D Create( Point2 sizeDivider )
        {
            var device = this.graphicsDeviceService.GraphicsDevice;
            var viewport = device.Viewport;

            return new RenderTarget2D(
                this.graphicsDeviceService.GraphicsDevice,
                viewport.Width / sizeDivider.X,
                viewport.Height / sizeDivider.Y,
                false,
                this.format, 
                DepthFormat.None,     
                0,
                RenderTargetUsage.PreserveContents
            );
        }
        
        /// <summary>
        /// The SurfaceFormat of the RenderTarget2Ds created by this RenderTarget2DFactory.
        /// </summary>
        private SurfaceFormat format = SurfaceFormat.Color;

        /// <summary>
        /// Provides access to the xna GraphiceDevice that is required by this RenderTarget2DFactory
        /// to create RenderTarget2D instances.
        /// </summary>
        private readonly IGraphicsDeviceService graphicsDeviceService;
    }
}
