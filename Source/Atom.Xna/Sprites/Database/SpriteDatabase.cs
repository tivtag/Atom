// <copyright file="SameTextureSpriteDatabase.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Xna.SpriteDatabase class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Xna
{
    using System;
    using System.Collections.Generic;
    using Atom.Diagnostics.Contracts;

    /// <summary>
    /// Represents a set of <see cref="Sprite"/>s and AnimatedSprites that are
    /// all based on the same Texture2D.
    /// </summary>
    public sealed partial class SpriteDatabase : INormalSpriteSource
    {
        /// <summary>
        /// Gets or sets the name that uniquely identifies this SameTextureSpriteDatabase.
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the SpriteAtlas that is part of this SpriteDatabase.
        /// </summary>
        public SpriteAtlas Atlas
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the list of AnimatedSprites that are stored in this SpriteDatabase.
        /// </summary>
        public IList<AnimatedSprite> AnimatedSprites
        {
            get
            {
                return this.animatedSprites;
            }
        }

        /// <summary>
        /// Gets the <see cref="Sprite"/> this SpriteDatabase contains.
        /// </summary>
        public IEnumerable<Sprite> Sprites
        {
            get
            {
                return this.Atlas.Sprites;
            }
        }

        /// <summary>
        /// Searches this SpriteDatabase for the Sprite with the given <paramref name="name"/>.
        /// </summary>
        /// <param name="name">
        /// The name of the Sprite to find.
        /// </param>
        /// <returns>
        /// The requested Sprite;
        /// or null if there is no Sprite that has the given name.
        /// </returns>
        public Sprite FindSprite( string name )
        {
            Contract.Requires<ArgumentNullException>( name != null );

            if( this.Atlas == null )
                return null;

            foreach( Sprite sprite in this.Atlas.Sprites )
            {
                if( name.Equals( sprite.Name, StringComparison.Ordinal ) )
                {
                    return sprite;
                }
            }

            return null;
        }

        /// <summary>
        /// The AnimatedSprites that are stored in this SpriteDatabase.
        /// </summary>
        private readonly List<AnimatedSprite> animatedSprites = new List<AnimatedSprite>();
    }
}
