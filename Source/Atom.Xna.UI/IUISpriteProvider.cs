// <copyright file="IUISpriteProvider.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Xna.UI.IUISpriteProvider interface.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Xna.UI
{
    /// <summary>
    /// Provides a mechanis to receive an <see cref="ISprite"/> for a <see cref="UIElement"/>.
    /// </summary>
    public interface IUISpriteProvider
    {
        /// <summary>
        /// Gets a ISprite for the given UIElement.
        /// </summary>
        /// <param name="element">
        /// The input UIElement this color operation is relevant to.
        /// </param>
        /// <returns>
        /// The Sprite.
        /// </returns>
        ISprite GetSpriteFor( UIElement element );
    }
}
