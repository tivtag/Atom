// <copyright file="ParticleRenderer.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Xna.Particles.ParticleRenderer class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Xna.Particles
{
    using System;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// Defines the abstract base class of an object
    /// that is responsible for rendering <see cref="Particle"/>s.
    /// </summary>
    public abstract class ParticleRenderer : IDisposable
    {
        /// <summary>
        /// Gets the IGraphicsDeviceService that provides a mechanism to access the Xna GraphicsDevice object.
        /// </summary>
        public IGraphicsDeviceService GraphicsDeviceService { get; private set; }

        /// <summary>
        /// Initializes a new instance of the ParticleRenderer class.
        /// </summary>
        /// <param name="graphicsDeviceService">
        /// Provides a mechanism to access the Xna GraphicsDevice object.
        /// </param>
        protected ParticleRenderer( IGraphicsDeviceService graphicsDeviceService )
        {
            if( graphicsDeviceService == null )
            {
                throw new ArgumentNullException( nameof( graphicsDeviceService ) );
            }

            this.GraphicsDeviceService = graphicsDeviceService;
        }

        /// <summary>
        /// Renders the specified <see cref="Emitter"/>.
        /// </summary>
        /// <param name="emitter">
        /// The <see cref="Emitter"/> to render.
        /// </param>
        public abstract void RenderEmitter( Emitter emitter );

        /// <summary>
        /// Loads all content that is required by this ParticleRenderer.
        /// </summary>
        /// <param name="content">
        /// The <see cref="ContentManager"/> to use.
        /// </param>
        public abstract void LoadContent( ContentManager content );
        
        /// <summary>
        /// Finalizes an instance of the ParticleRenderer class.
        /// </summary>
        ~ParticleRenderer()
        {
            this.Dispose( false );
        }

        /// <summary>
        /// Releases all resources used by this <see cref="ParticleRenderer"/>.
        /// </summary>
        public void Dispose()
        {
            Dispose( true );
            GC.SuppressFinalize( this );
        }

        /// <summary>
        /// Releases all resources used by this <see cref="ParticleRenderer"/>.
        /// </summary>
        /// <param name="releaseManaged">
        /// Determines whether to release all managed resources of this object.
        /// </param>
        protected virtual void Dispose( bool releaseManaged )
        {
        }
    }
}