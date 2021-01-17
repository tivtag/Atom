// <copyright file="BaseTriggerController.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>Defines the Atom.Xna.Particles.Controllers.BaseTriggerController class.</summary>
// <author>Paul Ennemoser</author>

namespace Atom.Xna.Particles.Controllers
{
    /// <summary>
    /// Represents a <see cref="Controller"/> that is intended to trigger
    /// a <see cref="ParticleEffect"/> after a fixed interval.
    /// </summary>
    public abstract class BaseTriggerController : Controller
    {
        /// <summary>
        /// Gets or sets the time that has to pass until the ParticleEffect is triggered again.
        /// </summary>
        public float TriggerTime
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes a new instance of the BaseTriggerController class.
        /// </summary>
        /// <param name="effect">
        /// The <see cref="ParticleEffect"/> this new <see cref="Controller"/> controls.
        /// </param>
        protected BaseTriggerController( ParticleEffect effect )
            : base( effect )
        {
        }

        /// <summary>
        /// Updates this TriggerController.
        /// </summary>
        /// <param name="updateContext">
        /// The current IXnaUpdateContext.
        /// </param>
        public override void Update( IXnaUpdateContext updateContext )
        {
            this.timeLeft -= updateContext.FrameTime;

            if( this.timeLeft <= 0.0f )
            {
                this.Trigger();
                this.timeLeft = this.TriggerTime;
            }
        }

        /// <summary>
        /// Triggers the <see cref="ParticleEffect"/>.
        /// </summary>
        protected virtual void Trigger()
        {
            this.Effect.Trigger( this.GetTriggerPosition() );
        }

        /// <summary>
        /// Gets the position at which the ParticleEffect should be triggered at.
        /// </summary>
        /// <returns>
        /// The trigger position.
        /// </returns>
        protected abstract Microsoft.Xna.Framework.Vector2 GetTriggerPosition();

        /// <summary>
        /// Stores the time left until the ParticleEffect gets triggered again.
        /// </summary>
        private float timeLeft;
    }
}
