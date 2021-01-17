// <copyright file="RectangularEffect.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Xna.Effects.RectangularEffect class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Xna.Effects
{
    using System;
    using Atom.Diagnostics.Contracts;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// Represents a single-pass effect that covers a rectangular area.
    /// </summary>
    /// <remarks>
    /// This is usually used to create fullscreen effects.
    /// </remarks>
    public abstract class RectangularEffect : ManagedDisposable, IDisposable
    {
        #region [ Properties ]

        /// <summary>
        /// Gets the screen-space vertices that are used to draw the RectangularEffect.
        /// </summary>
        protected VertexPositionTexture[] Vertices
        {
            get 
            { 
                return RectangularEffect.fullScreenVertices; 
            }
        }

        /// <summary>
        /// Gets the <see cref="GraphicsDevice"/> required for rendering.
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
        /// Initializes a new instance of the RectangularEffect class.
        /// </summary>
        /// <param name="effectLoader">
        /// Provides a mechanism that allows loading of effect assets.
        /// </param>
        /// <param name="deviceService">
        /// Provides access to the <see cref="GraphicsDevice"/>.
        /// </param>
        protected RectangularEffect( IEffectLoader effectLoader, IGraphicsDeviceService deviceService )
        {
            Contract.Requires<ArgumentNullException>( effectLoader != null );
            Contract.Requires<ArgumentNullException>( deviceService != null );

            this.effectLoader = effectLoader;
            this.deviceService = deviceService;
        }

        /// <summary>
        /// Loads the content used by this RectangularEffect.
        /// </summary>
        public void LoadContent()
        {
            this.graphicsDevice = this.deviceService.GraphicsDevice;

            this.LoadEffect( this.effectLoader );
            this.LoadCustomContent();
        }

        /// <summary>
        /// Loads the effect used by this RectangularEffect.
        /// </summary>
        /// <param name="effectLoader">
        /// Provides a mechanism that allows loading of effect assets.
        /// </param>
        protected abstract void LoadEffect( IEffectLoader effectLoader );

        /// <summary>
        /// Provides a hook that can be overwritten by subclasses to load additional content.
        /// </summary>
        protected virtual void LoadCustomContent()
        {
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Draws this RectangularEffect using the given Effect and a single EffectPass.
        /// </summary>
        /// <param name="effect">
        /// The effect to draw.
        /// </param>
        /// <param name="effectPass">
        /// The effect pass to use while drawing.
        /// </param>
        protected void Draw( Effect effect, EffectPass effectPass )
        {
            effectPass.Apply();

            this.graphicsDevice.DrawUserPrimitives( 
                PrimitiveType.TriangleStrip,
                this.Vertices,
                0,
                2
            );
        }

        #endregion

        #region [ Fields ]

        /// <summary>
        /// The xna GraphicsDevice.
        /// </summary>
        private GraphicsDevice graphicsDevice;

        /// <summary>
        /// Provides a mechanism that allows loading of effect assets.
        /// </summary>
        private readonly IEffectLoader effectLoader;

        /// <summary>
        /// Provides access to the <see cref="GraphicsDevice"/>.
        /// </summary>
        private readonly IGraphicsDeviceService deviceService;
        
        /// <summary>
        /// The screen-space vertices that are used to draw the Noise.
        /// </summary>
        private static VertexPositionTexture[] fullScreenVertices = new VertexPositionTexture[4] { 
            new VertexPositionTexture( new Vector3(-1,  1, 0), new Vector2( 0, 0 ) ),
            new VertexPositionTexture( new Vector3( 1,  1, 0), new Vector2( 1, 0 ) ),
            new VertexPositionTexture( new Vector3(-1, -1, 0), new Vector2( 0, 1 ) ),
            new VertexPositionTexture( new Vector3( 1, -1, 0), new Vector2( 1, 1 ) )
        };

        #endregion
    }
}
