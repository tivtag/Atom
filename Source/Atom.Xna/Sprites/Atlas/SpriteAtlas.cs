// <copyright file="SpriteAtlas.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Xna.SpriteAtlas class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Xna
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// Represents an atlas of <see cref="Sprite"/>s that share the same <see cref="Texture2D"/>.
    /// This class can't be inherited.
    /// </summary>
    public sealed partial class SpriteAtlas
    {
        /// <summary>
        /// Gets or sets the Texture all <see cref="Sprites"/> of this SpriteAtlas use.
        /// </summary>
        public Texture2D Texture
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the list of Sprites this SpriteAtlas contains.
        /// </summary>
        public IList<Sprite> Sprites
        {
            get
            {
                return this.sprites;
            }
        }

        /// <summary>
        /// Verifies the integrity of this SpriteAtlas.
        /// </summary>
        public void Verify()
        {
            if( this.Texture == null )
            {
                throw new InvalidOperationException( "The texture of this SpriteAtlas is null. This is not valid." );  
            }

            foreach( var sprite in this.sprites )
            {
                this.VerifySprite( sprite );
            }
        }

        /// <summary>
        /// Verifies the integrity of the specified Sprite.
        /// </summary>
        /// <param name="sprite">
        /// The Sprite to verify.
        /// </param>
        private void VerifySprite( Sprite sprite )
        {
            if( sprite.Texture != this.Texture )
            {
                throw new InvalidOperationException(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        "The texture '{0}' of the Sprite '{1}' doesn't match the texture '{2}' of this SpriteAtlas.",
                        sprite.Texture != null ? (sprite.Texture.Name ?? string.Empty) : string.Empty,
                        sprite.Name ?? string.Empty,
                        this.Texture.Name ?? string.Empty
                    )
                );
            }
        }
        
        /// <summary>
        /// The Sprites that are part of this SpriteAtlas.
        /// </summary>
        private readonly List<Sprite> sprites = new List<Sprite>();
    }
}
