// <copyright file="LightMap.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Xna.LightMap class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Xna
{
    using System;
    using Atom.Diagnostics.Contracts;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// A <see cref="LightMap"/> is an independent 'layer'
    /// that is used to render Lights to.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Rendering to the <see cref="LightMap"/> is done in
    /// LightMap Begin and LightMap End block. (RenderTargets may not be changed in the block)
    /// The <see cref="LightMap"/> combines itself with the scene by calling Draw.
    /// </para><para>
    /// Also, RenderTargetUsage of the BackBuffer should be set to PreserveContent.
    /// using the PreparingDeviceSettings event.
    /// e.GraphicsDeviceInformation.PresentationParameters.RenderTargetUsage = RenderTargetUsage.PreserveContents;
    /// </para>
    /// </remarks>
    public class LightMap : ManagedDisposable, IContentLoadable
    {
        #region [ Properties ]

        /// <summary>
        /// Gets or sets the ambient color.
        /// </summary>
        public Color AmbientColor
        {
            get 
            {
                return this.ambientColor;
            }

            set 
            {
                this.ambientColor = value;
            }
        }

        /// <summary>
        /// Gets the RenderTarget2D to which lights are drawn to.
        /// </summary>
        protected RenderTarget2D LightTarget
        {
            get
            {
                return this.lightTarget;
            }
        }

        /// <summary>
        /// Gets the xna GraphicsDevice that is required for drawing.
        /// </summary>
        protected GraphicsDevice GraphicsDevice
        {
            get
            {
                return this.graphicsDevice;
            }
        }

        #endregion

        #region [ Initialization ]

        /// <summary>
        /// Initializes a new instance of the <see cref="LightMap"/> class.
        /// </summary>
        /// <param name="renderTargetFactory">
        /// The IRenderTarget2DFactory that is responsible for creating the RenderTarget2D used
        /// by the new LightMap.
        /// </param>
        /// <param name="graphicsDeviceService">
        /// Provides access to the xna GraphicsDevice that is required for rendering the new LightMap.
        /// </param>
        public LightMap( IRenderTarget2DFactory renderTargetFactory, IGraphicsDeviceService graphicsDeviceService )
        {
            Contract.Requires<ArgumentNullException>( renderTargetFactory != null );
            Contract.Requires<ArgumentNullException>( graphicsDeviceService != null );

            this.renderTargetFactory = renderTargetFactory;
            this.graphicsDeviceService = graphicsDeviceService;
        }

        /// <summary>
        /// Overriden to load required graphics content.
        /// </summary>
        public void LoadContent()
        {
            this.graphicsDevice = this.graphicsDeviceService.GraphicsDevice;

            this.CreateLightTarget();
            this.CreateSpriteBatch();
        }

        /// <summary>
        /// Creates the RenderTarget the LightMap is drawing to.
        /// </summary>
        private void CreateLightTarget()
        {
            if( this.lightTarget != null )
            {
                this.lightTarget.Dispose();
            }

            this.lightTarget = this.renderTargetFactory.Create();
        }

        /// <summary>
        /// Creates the SpriteBatch used to draw the texture.
        /// </summary>
        private void CreateSpriteBatch()
        {
            if( this.batch != null )
            {
                this.batch.Dispose();
            }

            this.batch = new SpriteBatch( this.graphicsDevice );
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// After this call all drawn sprites are rendered to the <see cref="LightMap"/>;
        /// instead of the previously set RenderTarget.
        /// </summary>
        public void Begin()
        {
            this.Begin( ClearOptions.Target, 0.0f, 0 );
        }

        /// <summary>
        /// After this call all drawn sprites are rendered to this <see cref="LightMap"/>;
        /// instead of the previously set RenderTarget.
        /// </summary>
        /// <param name="clearOptions">
        /// Flags indicating which surfaces to clear.
        /// </param>
        /// <param name="depth">
        /// New depth value that this method stores in the depth buffer. This parameter
        /// can be in the range of 0.0 through 1.0 (for z-based or w-based depth buffers).
        /// A value of 0.0 represents the nearest distance to the viewer; a value of
        /// 1.0 represents the farthest distance.
        /// </param>
        /// <param name="stencil">
        /// Integer value to store in each stencil-buffer entry. This parameter can be
        /// in the range of 0 through 2n−1, where n is the bit depth of the stencil buffer.
        /// </param>
        public void Begin( ClearOptions clearOptions, float depth, int stencil )
        {
            this.oldTarget = this.graphicsDevice.GetRenderTarget2D();

            this.graphicsDevice.SetRenderTarget( this.lightTarget );
            this.graphicsDevice.Clear( clearOptions, this.ambientColor, depth, stencil );
        }

        /// <summary>
        /// Ends the drawing of light sprites to this <see cref="LightMap"/>.
        /// </summary>
        public void End()
        {
            this.graphicsDevice.SetRenderTarget( this.oldTarget );
        }
        
        /// <summary>
        /// Draws this LightMap.
        /// </summary>
        public void Draw()
        {
            var renderState = this.graphicsDevice.BlendState;
                                    
            // Draw:
            batch.Begin( SpriteSortMode.Immediate, blendState );
            
            batch.Draw( this.lightTarget, Vector2.Zero, Color.White );
            batch.End();
        }

        /// <summary>
        /// Disposes the unmanaged resources used by this LightMap.
        /// </summary>
        protected override void DisposeUnmanagedResources()
        {
            if( this.batch != null )
            {
                this.batch.Dispose();
            }
        }

        /// <summary>
        /// Disposes the managed resources used by this LightMap.
        /// </summary>
        protected override void DisposeManagedResources()
        {
            this.batch = null;
        }

        #endregion

        #region [ Fields ]

        /// <summary>
        /// The blending state that is used when blending together
        /// the light map and the normal unlit scene.
        /// </summary>
        private readonly BlendState blendState = new BlendState() {
            ColorSourceBlend = Blend.DestinationColor,
            AlphaSourceBlend = Blend.DestinationColor,

            ColorDestinationBlend = Blend.SourceColor,
            AlphaDestinationBlend = Blend.SourceColor
        };
        
        /// <summary>
        /// The xna GraphicsDevice that is required for rendering.
        /// </summary>
        private GraphicsDevice graphicsDevice;

        /// <summary>
        /// The ambient color.
        /// </summary>
        private Color ambientColor = Color.Black;

        /// <summary>
        /// The <see cref="RenderTarget2D"/> which contains the Light scene.
        /// </summary>
        private RenderTarget2D lightTarget;

        /// <summary>
        /// The <see cref="RenderTarget2D"/> which was set before drawing to the LightMap has started.
        /// </summary>
        private RenderTarget2D oldTarget;

        /// <summary>
        /// The batch we use for rendering.
        /// </summary>
        private SpriteBatch batch;

        /// <summary>
        /// The IRenderTarget2DFactory that is responsible for creating the RenderTarget2D used
        /// by this LightMap.
        /// </summary>
        private readonly IRenderTarget2DFactory renderTargetFactory;

        /// <summary>
        /// Provides access to the xna GraphicsDevice that is required for rendering.
        /// </summary>
        private readonly IGraphicsDeviceService graphicsDeviceService;

        #endregion
    }
}
