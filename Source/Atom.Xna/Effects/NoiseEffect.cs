// <copyright file="NoiseEffect.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Xna.Effects.NoiseEffect class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Xna.Effects
{
    using System;
    using Atom.Math;
    using Microsoft.Xna.Framework.Graphics;
    using Xna = Microsoft.Xna.Framework;

    /// <summary>
    /// Implements an improved perlin noise fullscreen effect.
    /// This class can't be inherited.
    /// </summary>
    public sealed class NoiseEffect : RectangularEffect, IUpdateable, IDrawable
    {
        #region [ Initialization ]

        /// <summary>
        /// Initializes a new instance of the NoiseEffect class.
        /// </summary>
        /// <param name="effectLoader">
        /// Provides a mechanism that allows loading of effect assets.
        /// </param>
        /// <param name="deviceService">
        /// Provides access to the <see cref="GraphicsDevice"/>.
        /// </param>
        public NoiseEffect( IEffectLoader effectLoader, IGraphicsDeviceService deviceService )
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
            this.effect = effectLoader.Load( "Noise" );
            this.effectPass = this.effect.CurrentTechnique.Passes[0];

            this.parameterTexture = this.effect.Parameters["Texture"];
            this.parameterMoveOffset = this.effect.Parameters["MoveOffset"];
            this.parameterColorFactor = this.effect.Parameters["ColorFactor"];
        }

        /// <summary>
        /// Loads all additional content that is used by this NoiseEffect.
        /// </summary>
        protected override void LoadCustomContent()
        {
            this.textureSeed = CreateSeedTexture( seedResolution, this.GraphicsDevice );
        }

        /// <summary>
        /// Creates the texture that is used as an input into the PerlinNoise shader.
        /// </summary>
        /// <param name="resolution">
        /// The resolution of the seed to create. A higher value results reduces the size,
        /// but increases the quantity of the details in the NoiseEffect.
        /// </param>
        /// <param name="device">
        /// The GraphicsDevice used to display the texture.
        /// </param>
        /// <returns>The newly created seed texture.</returns>
        private static Texture2D CreateSeedTexture( int resolution, GraphicsDevice device )
        {
            var noiseImage = new Texture2D(
                device,
                resolution,
                resolution,
                false,
                SurfaceFormat.Color
            );

            Random rand = new Random();
            Xna.Color[] noisyColors = new Xna.Color[resolution * resolution];

            for( int x = 0; x < resolution; ++x )
            {
                for( int y = 0; y < resolution; ++y )
                {
                    noisyColors[x + (y * resolution)] = 
                        new Xna.Color( new Microsoft.Xna.Framework.Vector3( (float)rand.NextDouble(), 0, 0 ) );
                }
            }

            noiseImage.SetData( noisyColors );
            return noiseImage;
        }

        #endregion

        #region [ Properties ]

        /// <summary>
        /// Gets or sets the movement speed of the NoiseEffect.
        /// </summary>
        /// <value>The speed the noise moves when updating the NoiseEffect.</value>
        public Vector2 MoveSpeed
        {
            get
            {
                return this.moveSpeed;
            }

            set
            {
                this.moveSpeed = Vector2.Max( value, Vector2.Zero );
            }
        }

        /// <summary>
        /// Gets or sets the movement direction of the NoiseEffect.
        /// </summary>
        /// <value>The direction the noise moves when updating the NoiseEffect.</value>
        public Vector2 MoveDirection
        {
            get
            {
                return this.moveDirection;
            }

            set
            {
                this.moveDirection = Vector2.Normalize( value );
            }
        }

        /// <summary>
        /// Gets or sets a value that controls how strongly the Noise effect is amplified.
        /// </summary>
        public float Overcast
        {
            get
            {
                return this.effect.Parameters["Overcast"].GetValueSingle();
            }

            set
            {
                this.effect.Parameters["Overcast"].SetValue( value );
            }
        }

        /// <summary>
        /// Gets or sets the base color of the NoiseEffect.
        /// </summary>
        public Xna.Color BaseColor
        {
            get
            {
                return new Xna.Color( this.effect.Parameters["BaseColor"].GetValueVector4() );
            }

            set
            {
                this.effect.Parameters["BaseColor"].SetValue( value.ToVector4() );
            }
        }

        /// <summary>
        /// Gets or sets the color factor of the NoiseEffect.
        /// </summary>
        public Vector4 ColorFactor
        {
            get
            {
                return this.parameterColorFactor.GetValueVector4().ToAtom();
            }

            set
            {
                this.parameterColorFactor.SetValue( value.ToXna() );
            }
        }

        /// <summary>
        /// Gets or sets the alpha value of the <see cref="ColorFactor"/> of the NoiseEffect.
        /// </summary>
        public float Alpha
        {
            get
            {
                return this.parameterColorFactor.GetValueVector4().W;
            }

            set
            {
                var color = this.parameterColorFactor.GetValueVector4();
                color.W = value;

                this.parameterColorFactor.SetValue( color );
            }
        }

        /// <summary>
        /// Gets or sets the resolution of the NoiseEffect's seed.
        /// </summary>
        /// <value>
        /// A higher value results reduces the size,
        /// but increases the quantity of the details in the NoiseEffect.
        /// </value>
        public int SeedResolution
        {
            get
            {
                return this.seedResolution;
            }

            set
            {
                if( value == this.seedResolution )
                    return;

                this.textureSeed = CreateSeedTexture( seedResolution, this.GraphicsDevice );
                this.seedResolution = value;
            }
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Manually moves this Noise Effect by the specified <paramref name="offset"/>.
        /// </summary>
        /// <param name="offset">
        /// The offset the move manually.
        /// </param>
        public void Move( Vector2 offset )
        {
            this.moveOffset += offset;
        }

        /// <summary>
        /// Draws this NoiseEffect.
        /// </summary>
        /// <param name="drawContext">
        /// The current IDrawContext.
        /// </param>
        public void Draw( IDrawContext drawContext )
        {
            this.parameterTexture.SetValue( this.textureSeed );
            this.parameterMoveOffset.SetValue( this.moveOffset.ToXna() );

            this.Draw( this.effect, this.effectPass );
        }

        /// <summary>
        /// Updates this NoiseEffect.
        /// </summary>
        /// <param name="updateContext">
        /// The current IUpdateContext.
        /// </param>
        public void Update( IUpdateContext updateContext )
        {
            this.moveOffset += this.moveSpeed * this.moveDirection * updateContext.FrameTime;
        }

        /// <summary>
        /// Disposes the managed resources used by this NoiseEffect.
        /// </summary>
        protected override void DisposeManagedResources()
        {
            this.effect = null;
            this.effectPass = null;
            this.textureSeed = null;
            this.parameterTexture = null;
            this.parameterMoveOffset = null;
            this.parameterColorFactor = null;
        }

        /// <summary>
        /// Disposes the unmanaged resources used by this NoiseEffect.
        /// </summary>
        protected override void DisposeUnmanagedResources()
        {
            if( this.textureSeed != null )
            {
                this.textureSeed.Dispose();
            }
        }

        #endregion

        #region [ Fields ]

        /// <summary>
        /// The current movement offset of the NoiseEffect.
        /// </summary>
        private Vector2 moveOffset;

        /// <summary>
        /// The movement direction of the NoiseEffect.
        /// </summary>
        private Vector2 moveDirection = Vector2.One;

        /// <summary>
        /// The movement speed of the NoiseEffect.
        /// </summary>
        private Vector2 moveSpeed = new Vector2( 0.1f, 0.1f );

        /// <summary>
        /// The resolution of the seed texture.
        /// </summary>
        private int seedResolution = 32;

        /// <summary>
        /// The texture that is passed into the Perlin Noise shader as the seed.
        /// </summary>
        private Texture2D textureSeed;
        
        /// <summary>
        /// The effect used by this NoiseEffect.
        /// </summary>
        private Effect effect;

        /// <summary>
        /// The pass used by the effect.
        /// </summary>
        private EffectPass effectPass;

        /// <summary>
        /// Identifies the MoveOffset parameter of the NoiseEffect.
        /// </summary>
        private EffectParameter parameterMoveOffset;

        /// <summary>
        /// Identifies the ColorFactor parameter of the NoiseEffect.
        /// </summary>
        private EffectParameter parameterColorFactor;

        /// <summary>
        /// Identifies the Texture parameter of the NoiseEffect.
        /// </summary>
        private EffectParameter parameterTexture;

        #endregion
    }
}
