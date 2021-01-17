// <copyright file="IFontLoader.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Xna.Fonts.IFontLoader interface.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Xna.Fonts
{
    /// <summary>
    /// Provides a mechanism for loading <see cref="IFont"/> assets.
    /// </summary>
    public interface IFontLoader : ICustomAssetLoader<IFont>
    {
        /// <summary>
        /// Reloads the underlying data of all cached <see cref="IFont"/> assets.
        /// </summary>
        void Reload();
    }
}
