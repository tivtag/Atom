// <copyright file="ISpriteSheetLoader.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Xna.ISpriteSheetLoader interface.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Xna
{
    /// <summary>
    /// Provides a mechanism that allows loading of <see cref="ISpriteSheet"/> assets.
    /// </summary>
    public interface ISpriteSheetLoader : IAssetLoader
    {
        /// <summary>
        /// Loads the <see cref="ISpriteSheet"/> resource with the given <paramref name="assetName"/>.
        /// </summary>
        /// <param name="assetName">
        /// The name that uniquely identifies the ISpriteSheet resource to load.
        /// </param>
        /// <returns>
        /// The ISpriteSheet resource thas has been loaden.
        /// </returns>
        ISpriteSheet LoadSpriteSheet( string assetName );
    }
}
