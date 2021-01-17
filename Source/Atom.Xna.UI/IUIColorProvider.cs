// <copyright file="IUIColorProvider.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Xna.UI.IUIColorProvider interface.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Xna.UI
{
    using Microsoft.Xna.Framework;

    /// <summary>
    /// Provides a mechanis to receive a <see cref="Color"/> for a <see cref="UIElement"/>.
    /// </summary>
    public interface IUIColorProvider
    {
        /// <summary>
        /// Gets a color for the given UIElement.
        /// </summary>
        /// <param name="element">
        /// The input UIElement this color operation is relevant to.
        /// </param>
        /// <returns>
        /// The color.
        /// </returns>
        Color GetColorFor( UIElement element );
    }
}
