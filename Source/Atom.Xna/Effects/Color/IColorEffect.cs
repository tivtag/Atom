// <copyright file="IColorEffect.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Xna.Effects.IColorEffect interface.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Xna.Effects
{
    using System;
    using Microsoft.Xna.Framework;

    /// <summary>
    /// Represents an effect that modifies <see cref="Color"/>s.
    /// </summary>
    public interface IColorEffect
    {
        /// <summary>
        /// Applies the current state of this IColorEffect to the specified <paramref name="color"/>.
        /// </summary>
        /// <param name="color">
        /// The color to apply this IColorEffect on.
        /// </param>
        /// <returns>
        /// The output color that has this IColorEffect applied.
        /// </returns>
        Color Apply( Color color );
    }
}