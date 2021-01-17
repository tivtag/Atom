// <copyright file="SpriteDatabase.SpriteLoader.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Xna.SpriteDatabase.SpriteLoader class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Xna
{
    using System;
    using Atom.Diagnostics.Contracts;

    /// <content>
    /// Defines an INormalSpriteLoader that loads <see cref="Sprite"/>s directly from an existing <see cref="SpriteDatabase"/>.
    /// </content>
    public partial class SpriteDatabase
    {
        /// <summary>
        /// Implements an <see cref="INormalSpriteLoader"/> that searches a <see cref="SpriteDatabase"/> object
        /// for Sprites.
        /// </summary>
        public sealed class SpriteLoader : INormalSpriteLoader
        {
            /// <summary>
            /// Initializes a new instance of the SpriteLoader class.
            /// </summary>
            /// <param name="database">
            /// The database from which Sprites should be received.
            /// </param>
            public SpriteLoader( SpriteDatabase database )
            {
                Contract.Requires<ArgumentNullException>( database != null );

                this.database = database;
            }

            /// <summary>
            /// Loads the Sprite with the given <paramref name="assetName"/>.
            /// </summary>
            /// <param name="assetName">
            /// The name that uniquely identifies the Sprite to load.
            /// </param>
            /// <returns>
            /// The Sprite that has been loaden.
            /// </returns>
            public Sprite LoadSprite( string assetName )
            {
                return this.database.FindSprite( assetName );
            }

            /// <summary>
            /// Gets or sets the root directory that is automatically
            /// prefixed to assets loaded by this IAssetLoader.
            /// </summary>
            string IAssetLoader.RootDirectory
            {
                get;
                set;
            }

            /// <summary>
            /// This operation has no effect for this implementation.
            /// </summary>
            void IDisposable.Dispose()
            {
            }

            /// <summary>
            /// Unloads all assets that have been loaden with this IAssetLoader.
            /// </summary>
            void IAssetLoader.UnloadAll()
            {
            }

            /// <summary>
            /// The database from which Sprites are received.
            /// </summary>
            private readonly SpriteDatabase database;
        }
    }
}
