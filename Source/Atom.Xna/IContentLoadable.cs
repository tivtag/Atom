// <copyright file="IContentLoadable.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Xna.IContentLoadable interface.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Xna
{
    /// <summary>
    /// Provides a mechanism for loading required content and assets.
    /// </summary>
    public interface IContentLoadable
    {
        /// <summary>
        /// Loads the required content and assets.
        /// </summary>
        void LoadContent();
    }
}
