// <copyright file="MouseInputEventHandler.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Xna.UI.MouseInputEventHandler delegate.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Xna.UI
{
    using Microsoft.Xna.Framework.Input;

    /// <summary>
    /// Defines the delegate that handles MouseInput.
    /// </summary>
    /// <param name="sender">
    /// The object that is the 'owner' of event.
    /// </param>
    /// <param name="mouseState">
    /// The state of the <see cref="Mouse"/>.
    /// Do NOT modify this value, unless you know exactly what you do.
    /// </param>
    /// <param name="oldMouseState">
    /// The state of the <see cref="Mouse"/> one frame ago.
    /// Do NOT modify this value, unless you know exactly what you do.
    /// </param>
    public delegate void MouseInputEventHandler( object sender, ref MouseState mouseState, ref MouseState oldMouseState );
}
