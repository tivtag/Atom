// <copyright file="GameLoopUpdateEventHandler.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Wpf.GameLoopUpdateEventHandler class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Wpf
{
    using System;

    /// <summary>
    /// Defines the delegate that contains the logic that gets executed by a <see cref="GameLoop"/> each frame.
    /// </summary>
    /// <param name="elapsedTime">
    /// The time the last frame took.
    /// </param>
    public delegate void GameLoopUpdateEventHandler( TimeSpan elapsedTime );
}
