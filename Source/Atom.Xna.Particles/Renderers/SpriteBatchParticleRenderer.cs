// <copyright file="SpriteBatchParticleRenderer.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Xna.Particles.SpriteBatchParticleRenderer class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Xna.Particles
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// Implements a ParticleRenderer that internally uses a SpritePatch to render each individual particle.
    /// </summary>
    public class SpriteBatchParticleRenderer : ParticleRenderer
    {    
        /// <summary>
        /// Gets or sets the offset that is applied when rendering the Particles.
        /// The default value is zero.
        /// </summary>
        public Vector2 Offset
        {
            get
            {
                return this.offset;
            }

            set
            {
                if( value == this.offset )
                    return;

                this.offset = value;
                this.GenerateTransform();
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SpriteBatchParticleRenderer"/> class.
        /// </summary>
        /// <param name="graphicsDeviceService">
        /// Provides a mechanism to access the Xna GraphicsDevice object.
        /// </param>
        public SpriteBatchParticleRenderer( IGraphicsDeviceService graphicsDeviceService )
            : base( graphicsDeviceService )
        {
        }

        private void GenerateTransform()
        {
            this.transform = Matrix.CreateTranslation( this.offset.X, this.offset.Y, 0.0f );
        }

        /// <inheritdoc />
        public override void RenderEmitter( Emitter emitter )
        {
            int particleCount = emitter.ActiveParticleCount;

            if( particleCount > 0 )
            {
                Texture2D texture = emitter.ParticleTexture;

                if( texture != null && !texture.IsDisposed )
                {
                    // Calculate the source rectangle and origin offset of the Particle texture...
                    Rectangle source = new Rectangle( 0, 0, emitter.ParticleTexture.Width, emitter.ParticleTexture.Height );
                    Vector2 origin = new Vector2( source.Width / 2f, source.Height / 2f );
                    int textureWidth = texture.Width;

                    this.batch.Begin( SpriteSortMode.Deferred, this.blendState, null, DepthStencilState.None, null, null, this.transform );

                    for( int i = 0; i < particleCount; ++i )
                    {
                        Particle particle = emitter.Particles[i];

                        float scale = particle.Scale / textureWidth;
                        this.batch.Draw( texture, particle.Position, source, particle.Color, particle.Rotation, origin, scale, SpriteEffects.None, 0f );
                    }

                    this.batch.End();
                }
            }
        }

        /// <inheritdoc />
        public override void LoadContent( Microsoft.Xna.Framework.Content.ContentManager content )
        {
            if( this.batch == null )
                this.batch = new SpriteBatch( this.GraphicsDeviceService.GraphicsDevice );
        }

        /// <inheritdoc />
        protected override void Dispose( bool releaseManaged )
        {
            if( this.batch != null )
            {
                this.batch.Dispose();
                this.batch = null;
            }
        }

        private SpriteBatch batch;
        private BlendState blendState = BlendState.NonPremultiplied;
        private Vector2 offset;
        private Matrix transform = Matrix.Identity;
    }
}
