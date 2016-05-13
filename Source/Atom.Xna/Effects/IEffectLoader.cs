// <copyright file="IEffectLoader.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>Defines the Atom.Xna.Effects.IEffectLoader interface.</summary>
// <author>Paul Ennemoser (Tick)</author>

namespace Atom.Xna.Effects
{
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// Provides a mechanism that allows loading of Effect assets.
    /// </summary>
    public interface IEffectLoader : IAssetLoader
    {
        /// <summary>
        /// Tries to receive the <see cref="Effect"/> with the given <paramref name="assetName"/>.
        /// </summary>
        /// <param name="assetName">
        /// The name of the texture asset to load.
        /// </param>
        /// <returns>
        /// The loaded Effect.
        /// </returns>
        Effect Load( string assetName );
    }
}
