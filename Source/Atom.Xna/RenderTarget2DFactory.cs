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
    using Atom.Diagnostics.Contracts;
    using Atom.Math;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// Implements an <see cref="IRenderTarget2DFactory"/> that creates RenderTarget2D instances
    /// with a specific size and SurfaceFormat.
    /// </summary>
    public class RenderTarget2DFactory : IRenderTarget2DFactory
    {
        /// <summary>
        /// Gets or sets the size of the RenderTarget2Ds created by this RenderTarget2DFactory.
        /// </summary>
        public Point2 Size
        {
            get 
            {
                return this.size; 
            }

            set
            {
                this.size = value; 
            }
        }

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
        /// Initializes a new instance of the RenderTarget2DFactory class.
        /// </summary>
        /// <param name="size">
        /// The initial size of the RenderTarget2Ds created by the new RenderTarget2DFactory.
        /// </param>
        /// <param name="graphicsDeviceService">
        /// Provides access to the xna GraphiceDevice that is required by the new RenderTarget2DFactory
        /// to create RenderTarget2D instances.
        /// </param>
        /// <param name="multiSampleCount">
        /// The preferred number of multisample locations.
        /// </param>
        public RenderTarget2DFactory( Point2 size, IGraphicsDeviceService graphicsDeviceService, int multiSampleCount = 0 )
        {
            Contract.Requires<ArgumentNullException>( graphicsDeviceService != null );

            this.Size = size;
            this.graphicsDeviceService = graphicsDeviceService;
            this.multiSampleCount = multiSampleCount;
        }

        /// <summary>
        /// Creates a new RenderTarget2D instance.
        /// </summary>
        /// <returns>
        /// A new RenderTarget2D instance.
        /// </returns>
        public RenderTarget2D Create()
        {
            return new RenderTarget2D(
                this.graphicsDeviceService.GraphicsDevice,
                this.size.X,
                this.size.Y,
                false,
                this.format,
                DepthFormat.None,
                this.multiSampleCount,
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
            return new RenderTarget2D(
                this.graphicsDeviceService.GraphicsDevice,
                this.size.X / sizeDivider.X,
                this.size.Y / sizeDivider.Y,
                false,
                this.format,
                DepthFormat.None,
                0,
                RenderTargetUsage.PreserveContents
            );
        }

        /// <summary>
        /// The size of the RenderTarget2Ds created by this RenderTarget2DFactory.
        /// </summary>
        private Point2 size;

        /// <summary>
        /// The SurfaceFormat of the RenderTarget2Ds created by this RenderTarget2DFactory.
        /// </summary>
        private SurfaceFormat format = SurfaceFormat.Color;

        /// <summary>
        /// The preferred number of multisample locations.
        /// </summary>
        private readonly int multiSampleCount;

        /// <summary>
        /// Provides access to the xna GraphiceDevice that is required by this RenderTarget2DFactory
        /// to create RenderTarget2D instances.
        /// </summary>
        private readonly IGraphicsDeviceService graphicsDeviceService;
    }
}
