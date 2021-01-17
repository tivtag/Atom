// <copyright file="Controller.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>Defines the Atom.Xna.Particles.Controller class.</summary>
// <author>Paul Ennemoser</author>

namespace Atom.Xna.Particles
{
    using System;

    /// <summary>
    /// Represents an object that controls some behaviour of a <see cref="ParticleEffect"/>.
    /// </summary>
    public abstract class Controller
    {
        /// <summary>
        /// Gets the <see cref="ParticleEffect"/> this <see cref="Controller"/> controls.
        /// </summary>
        public ParticleEffect Effect
        {
            get
            {
                return this.effect;
            }
        }

        /// <summary>
        /// Initializes a new instance of the Controller class.
        /// </summary>
        /// <param name="effect">
        /// The <see cref="ParticleEffect"/> this new <see cref="Controller"/> controls.
        /// </param>
        protected Controller( ParticleEffect effect )
        {
            if( effect == null )
                throw new ArgumentNullException( "effect" );

            this.effect = effect;
        }

        /// <summary>
        /// Updates this Controller.
        /// </summary>
        /// <param name="updateContext">
        /// The current IXnaUpdateContext.
        /// </param>
        public abstract void Update( IXnaUpdateContext updateContext );

        /// <summary>
        /// The <see cref="ParticleEffect"/> this <see cref="Controller"/> controls.
        /// </summary>
        private readonly ParticleEffect effect;
    }
}
