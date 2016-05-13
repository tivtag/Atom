// <copyright file="IContentManagerProvider.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Xna.IContentManagerProvider interface.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Xna
{
    using Microsoft.Xna.Framework.Content;

    /// <summary>
    /// Provides a mechanism that allows receiving of a <see cref="ContentManager"/>.
    /// </summary>
    public interface IContentManagerProvider
    {
        /// <summary>
        /// Gets the <see cref="ContentManager"/> this IContentManagerProvider provides.
        /// </summary>
        ContentManager Content 
        {
            get; 
        }
    }
}
