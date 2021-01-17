// <copyright file="EdgeDetectionEffect.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Xna.Effects.EdgeDetectionEffect class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Xna.Effects
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// Defines a <see cref="RectangularEffect"/> that tries to detect the edges
    /// in an image Texture.
    /// </summary>
    public sealed class EdgeDetectionEffect : RectangularEffect, Atom.IDrawable
    {
        #region [ Initialization ]

        /// <summary>
        /// Initializes a new instance of the EdgeDetectionEffect class.
        /// </summary>
        /// <param name="effectLoader">
        /// Provides a mechanism that allows loading of effect assets.
        /// </param>
        /// <param name="deviceService">
        /// Provides access to the <see cref="GraphicsDevice"/>.
        /// </param>
        public EdgeDetectionEffect( IEffectLoader effectLoader, IGraphicsDeviceService deviceService )
            : base( effectLoader, deviceService )
        {
        }

        /// <summary>
        /// Loads the effect used by this EdgeDetectionEffect.
        /// </summary>
        /// <param name="effectLoader">
        /// Provides a mechanism that allows loading of effect assers.
        /// </param>
        protected override void LoadEffect( IEffectLoader effectLoader )
        {
            this.effect = effectLoader.Load( "EdgeDetection" );
            this.effectPass = this.effect.CurrentTechnique.Passes[0];

            this.parameterTexture = effect.Parameters["Texture"];
            this.parameterTextureSize = effect.Parameters["TextureSize"];
        }

        #endregion

        #region [ Properties ]

        /// <summary>
        /// Gets or sets a value that controls when a pixel is considered to be an edge.
        /// </summary>
        public float Threshold
        {
            get
            {
                return this.effect.Parameters["Threshold"].GetValueSingle();
            }

            set
            {
                this.effect.Parameters["Threshold"].SetValue( value );
            }
        }

        /// <summary>
        /// Gets or sets the Thickness of the edges created by this EdgeDetectionEffect.
        /// </summary>
        public float Thickness
        {
            get
            {
                return this.effect.Parameters["Thickness"].GetValueSingle();
            }

            set
            {
                this.effect.Parameters["Thickness"].SetValue( value );
            }
        }

        /// <summary>
        /// Gets or sets the Texture2D whose edges are detected.
        /// </summary>
        public Texture2D Texture
        {
            get
            {
                return this.texture;
            }

            set
            {
                this.texture = value;
                this.parameterTexture.SetValue( value );

                if( value != null )
                {
                    this.parameterTextureSize.SetValue( new Vector2( value.Width, value.Height ) );
                }
                else
                {
                    this.parameterTextureSize.SetValue( Vector2.Zero );
                }
            }
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Draws this EdgeDetectionEffect.
        /// </summary>
        /// <param name="drawContext">
        /// The current IDrawContext.
        /// </param>
        public void Draw( IDrawContext drawContext )
        {
            if( this.texture == null )
                return;

            this.Draw( this.effect, this.effectPass );
        }

        /// <summary>
        /// Disposes the managed resources used by this EdgeDetectionEffect.
        /// </summary>
        protected override void DisposeManagedResources()
        {
            this.effect = null;
            this.texture = null;
            this.effectPass = null;
            this.parameterTexture = null;
            this.parameterTextureSize = null;
        }

        /// <summary>
        /// Disposes the unmanaged resources used by this EdgeDetectionEffect.
        /// </summary>
        protected override void DisposeUnmanagedResources()
        {
            if( this.effect != null )
            {
                this.effect.Dispose();
            }

            if( this.texture != null )
            {
                this.texture.Dispose();
            }
        }
         
        #endregion

        #region [ Fields ]

        /// <summary>
        /// The image whose edges are detected with this EdgeDetectionEffect.
        /// </summary>
        private Texture2D texture;

        /// <summary>
        /// The effect used by this EdgeDetectionEffect.
        /// </summary>
        private Effect effect;

        /// <summary>
        /// The pass used by the effect.
        /// </summary>
        private EffectPass effectPass;

        /// <summary>
        /// Identifies the Texture parameter of the EdgeDetectionEffect.
        /// </summary>
        private EffectParameter parameterTexture;

        /// <summary>
        /// Identifies the TextureSize parameter of the EdgeDetectionEffect.
        /// </summary>
        private EffectParameter parameterTextureSize;

        #endregion
    }
}
