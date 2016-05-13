// <copyright file="KeyboardInputEventHandler.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Xna.UI.KeyboardInputEventHandler delegate.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Xna.UI
{
    using Microsoft.Xna.Framework.Input;

    /// <summary>
    /// Defines the delegate that handles Keyboard Input.
    /// </summary>
    /// <param name="sender">
    /// The object that is the 'owner' of event.
    /// </param>
    /// <param name="keyState">
    /// The state of the <see cref="Keyboard"/>.
    /// Do NOT modify this value, unless you know exactly what you do.
    /// </param>
    /// <param name="oldKeyState">
    /// The state of the <see cref="Keyboard"/> one frame ago.
    /// Do NOT modify this value, unless you know exactly what you do.
    /// </param>
    public delegate void KeyboardInputEventHandler( object sender, ref KeyboardState keyState, ref KeyboardState oldKeyState );
}
