// <copyright file="ParticleEffect.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>Defines the Atom.Xna.Particles.ParticleEffect class.</summary>
// <author>Paul Ennemoser</author>

namespace Atom.Xna.Particles
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Represents an effect that is composed of <see cref="Emitter"/>s and <see cref="Controller"/>s.
    /// </summary>
    public class ParticleEffect
    {
        /// <summary>
        /// Gets the list of <see cref="Emitter"/>s that are associated with this ParticleEffect.
        /// </summary>
        public IList<Emitter> Emitters
        {
            get { return this.emitters; }
        }

        /// <summary>
        /// Gets the list of <see cref="Controller"/>s that are associated with this ParticleEffect.
        /// </summary>
        public IList<Controller> Controllers
        {
            get { return this.controllers; }
        }

        /// <summary>
        /// Initializes a new instance of the ParticleEffect class.
        /// </summary>
        /// <param name="renderer">
        /// The <see cref="ParticleRenderer"/> that should be used to render the new ParticleEffect.
        /// </param>
        public ParticleEffect( ParticleRenderer renderer )
        {
            if( renderer == null )
                throw new ArgumentNullException( "renderer" );

            this.renderer = renderer;
        }

        /// <summary>
        /// Renders this ParticleEffect.
        /// </summary>
        public void Render()
        {
            for( int i = 0; i < emitters.Count; ++i )
            {
                renderer.RenderEmitter( emitters[i] );
            }
        }

        /// <summary>
        /// Updates all <see cref="Controller"/>s and <see cref="Emitter"/>s of this ParticleEffect.
        /// </summary>
        /// <param name="updateContext">
        /// The current IXnaUpdateContext.
        /// </param>
        public void Update( IXnaUpdateContext updateContext )
        {
            for( int i = 0; i < this.controllers.Count; ++i )
            {
                this.controllers[i].Update( updateContext );
            }

            var gameTime = updateContext.GameTime;
            var totalSeconds   = (float)gameTime.TotalGameTime.TotalSeconds;
            var elapsedSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;

            for( int i = 0; i < this.emitters.Count; ++i )
            {
                this.emitters[i].Update( totalSeconds, elapsedSeconds );
            }
        }

        /// <summary>
        /// Triggers all <see cref="Emitter"/>s of this ParticleEffect.
        /// </summary>
        /// <param name="position">
        /// The trigger position.
        /// </param>
        public void Trigger( Microsoft.Xna.Framework.Vector2 position )
        {
            for( int i = 0; i < this.emitters.Count; ++i )
            {
                this.emitters[i].Trigger( position );
            }
        }

        /// <summary>
        /// Moves all active particles of this RainSnowParticleEffect by the given scrollValue.
        /// </summary>
        /// <param name="scrollValue">
        /// The value each Particle should be moved for.
        /// </param>
        public void MoveActiveParticles( Microsoft.Xna.Framework.Vector2 scrollValue )
        {
            for( int i = 0; i < emitters.Count; ++i )
            {
                emitters[i].MoveActiveParticles( scrollValue );
            }
        }

        /// <summary>
        /// The list of <see cref="Controller"/>s that control this ParticleEffect.
        /// </summary>
        private readonly List<Controller> controllers = new List<Controller>();

        /// <summary>
        /// The list of <see cref="Emitter"/>s this ParticleEffect consists of.
        /// </summary>
        private readonly List<Emitter> emitters = new List<Emitter>();

        /// <summary>
        /// The ParticleRenderer that is used to render the <see cref="Emitter"/>s of this ParticleEffect.
        /// </summary>
        private readonly ParticleRenderer renderer;
    }
}
