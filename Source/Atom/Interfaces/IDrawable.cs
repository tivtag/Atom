// <copyright file="IDrawable.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>Defines the Atom.IDrawable interface.</summary>
// <author>Paul Ennemoser</author>

namespace Atom
{
    /// <summary>
    /// Provides the mechanism of drawing an object.
    /// </summary>
    public interface IDrawable
    {
        /// <summary>
        /// Draws this IDrawable.
        /// </summary>
        /// <param name="drawContext">
        /// The current <see cref="IDrawContext"/>.
        /// </param>
        void Draw( IDrawContext drawContext );
    }
}
