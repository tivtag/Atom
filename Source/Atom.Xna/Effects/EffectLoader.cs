// <copyright file="EffectLoader.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Xna.Effects.EffectLoader class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Xna.Effects
{
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// Implements a mechanism that allows loading of Effect assets
    /// from compiled xna resources by using a <see cref="Microsoft.Xna.Framework.Content.ContentManager"/>.
    /// </summary>
    public class EffectLoader : ContentAssetLoader, IEffectLoader
    {
        /// <summary>
        /// Initializes a new instance of the EffectLoader class.
        /// </summary>
        /// <param name="contentManager">
        /// The Xna ContentManager responsible for loading the Effect assets.
        /// </param>
        public EffectLoader( ContentManager contentManager )
            : base( contentManager )
        {
        }

        /// <summary>
        /// Creates a new EffectLoader that uses a custom ContentManager with a
        /// rootDirectory of "Content/Effects/".
        /// </summary>
        /// <param name="serviceProvider">
        /// The service provider the content manager should use to locate services.
        /// </param>
        /// <returns>
        /// The newly created EffectLoader.
        /// </returns>
        public static EffectLoader Create( System.IServiceProvider serviceProvider )
        {
            return new EffectLoader( CreateContentManager( serviceProvider ) );
        }

        /// <summary>
        /// Creates a new ContentManager with a rootDirectory of "Content/Effects/".
        /// </summary>
        /// <param name="serviceProvider">
        /// The service provider the content manager should use to locate services.
        /// </param>
        /// <returns>
        /// The newly created ContentManager.
        /// </returns>
        private static ContentManager CreateContentManager( System.IServiceProvider serviceProvider )
        {
            return new ContentManager( serviceProvider, "Content/Effects/" );
        }

        /// <summary>
        /// Tries to receive the <see cref="Effect"/> with the given <paramref name="assetName"/>.
        /// </summary>
        /// <param name="assetName">
        /// The name of the Effect asset to load.
        /// </param>
        /// <returns>
        /// The loaded Effect.
        /// </returns>
        public Effect Load( string assetName )
        {
            return this.ContentManager.Load<Effect>( assetName );
        }
    }
}
