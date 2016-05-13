// <copyright file="IAssetLoader.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Xna.IAssetLoader interface.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Xna
{
    using System;

    /// <summary>
    /// Provides the base contract that all asset loaders share.
    /// </summary>
    public interface IAssetLoader : IDisposable
    {
        /// <summary>
        /// Gets or sets the root directory that is automatically
        /// prefixed to assets loaded by this IAssetLoader.
        /// </summary>
        string RootDirectory
        {
            get;
            set;
        }
               
        /// <summary>
        /// Unloads all assets that have been loaden with this IAssetLoader.
        /// </summary>
        void UnloadAll();
    }
}
