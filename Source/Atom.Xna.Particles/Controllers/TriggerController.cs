// <copyright file="TriggerController.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>Defines the Atom.Xna.Particles.Controllers.TriggerController class.</summary>
// <author>Paul Ennemoser</author>

namespace Atom.Xna.Particles.Controllers
{
    /// <summary>
    /// Implements a <see cref="BaseTriggerController"/> that triggers at 
    /// a specific position.
    /// </summary>
    public class TriggerController : BaseTriggerController
    {
        /// <summary>
        /// Gets or sets the position at which this TriggerController triggers the <see cref="ParticleEffect"/>.
        /// </summary>
        public Microsoft.Xna.Framework.Vector2 Position
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes a new instance of the TriggerController class.
        /// </summary>
        /// <param name="effect">
        /// The <see cref="ParticleEffect"/> this new <see cref="Controller"/> controls.
        /// </param>
        public TriggerController( ParticleEffect effect )
            : base( effect )
        {
        }

        /// <summary>
        /// Gets the position at which the ParticleEffect should be triggered at.
        /// </summary>
        /// <returns>
        /// The trigger position.
        /// </returns>
        protected override Microsoft.Xna.Framework.Vector2 GetTriggerPosition()
        {
            return this.Position;
        }
    }
}
