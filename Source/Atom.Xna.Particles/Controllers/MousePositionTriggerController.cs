// <copyright file="MousePositionTriggerController.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>Defines the Atom.Xna.Particles.Controllers.MousePositionTriggerController class.</summary>
// <author>Paul Ennemoser</author>

namespace Atom.Xna.Particles.Controllers
{
    using Microsoft.Xna.Framework.Input;

    /// <summary>
    /// Implements a <see cref="BaseTriggerController"/> that triggers at the current
    /// position of the mouse.
    /// </summary>
    public class MousePositionTriggerController : BaseTriggerController
    {
        /// <summary>
        /// Initializes a new instance of the MousePositionTriggerController class.
        /// </summary>
        /// <param name="effect">
        /// The <see cref="ParticleEffect"/> this new <see cref="Controller"/> controls.
        /// </param>
        public MousePositionTriggerController( ParticleEffect effect )
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
            MouseState mouseState = Mouse.GetState();
            return new Microsoft.Xna.Framework.Vector2( mouseState.X, mouseState.Y );
        }
    }
}
