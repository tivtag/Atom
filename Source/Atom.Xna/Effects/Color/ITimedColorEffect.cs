// <copyright file="ITimedColorEffect.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Xna.Effects.ITimedColorEffect interface.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Xna.Effects
{
    using System;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// Represents an <see cref="IColorEffect"/> that modifies <see cref="Microsoft.Xna.Framework.Color"/>s
    /// differently as time flows.
    /// </summary>
    public interface ITimedColorEffect : IColorEffect, IUpdateable
    {
        /// <summary>
        /// Raised when this IColorEffect has reached its final effect.
        /// </summary>
        event EventHandler Ended;

        /// <summary>
        /// Resets this IColorEffect to its initial state.
        /// </summary>
        void Reset();
    }
}
