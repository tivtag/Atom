// <copyright file="SpriteDatabase.ReaderWriter.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Xna.SpriteDatabase.ReaderWriter class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Xna
{
    using System;
    using System.Diagnostics.Contracts;
    using Atom.Storage;

    /*
     * Sprite Database File Format
     * 
     * Header
     *      Name
     * 
     * Sprite Atlas 
     *      atlas 
     * 
     * Animated Sprites
     *      animated_sprite_x
     *      animated_sprite_y
     */

    /// <content>
    /// Defines the default IObjectReaderWriter for the SpriteDatabase class.
    /// </content>
    public partial class SpriteDatabase
    {
        /// <summary>
        /// Defines the IObjectReaderWriter that implements serialization and deserialization
        /// of <see cref="SpriteDatabase"/> objects. This class can't be inherited.
        /// </summary>
        public sealed class ReaderWriter : BaseObjectReaderWriter<SpriteDatabase>
        {
            /// <summary>
            /// The extension of the SpriteDatabase files: '.sdb'
            /// </summary>
            public const string Extension = ".sdb";

            /// <summary>
            /// Initializes a new instance of the ReaderWriter class.
            /// </summary>
            /// <param name="textureLoader">
            /// Provides a mechanism that allows loading of Texture2D assets.
            /// </param>
            public ReaderWriter( ITexture2DLoader textureLoader )
                : this( new SpriteAtlas.ReaderWriter( textureLoader ) )
            {
            }

            /// <summary>
            /// Initializes a new instance of the ReaderWriter class.
            /// </summary>
            /// <param name="spriteAtlasReaderWriter">
            /// The IObjectReaderWriter that should be used to de-/serialize SpriteAtlas objects.
            /// </param>
            public ReaderWriter( IObjectReaderWriter<SpriteAtlas> spriteAtlasReaderWriter )
            {
                Contract.Requires<ArgumentNullException>( spriteAtlasReaderWriter != null );

                this.spriteAtlasReaderWriter = spriteAtlasReaderWriter;
            }

            /// <summary>
            /// Serializes the given object using the given ISerializationContext.
            /// </summary>
            /// <param name="object">
            /// The object to serialize.
            /// </param>
            /// <param name="context">
            /// The context that provides everything required for the serialization process.
            /// </param>
            public override void Serialize( SpriteDatabase @object, ISerializationContext context )
            {
                const int CurrentVersion = 1;
                context.Write( CurrentVersion );
                context.Write( @object.Name );

                // Sprites:
                this.spriteAtlasReaderWriter.Serialize( @object.Atlas, context );

                // Animated Sprites:
                context.Write( @object.animatedSprites.Count );

                var animatedSpriteReaderWriter = CreateAnimatedSpriteReaderWriter( @object );
                for( int i = 0; i < @object.animatedSprites.Count; ++i )
                {
                    animatedSpriteReaderWriter.Serialize( @object.animatedSprites[i], context );
                }
            }

            /// <summary>
            /// Deserializes the given object using the given IDeserializationContext.
            /// </summary>
            /// <param name="object">
            /// The object to deserialize.
            /// </param>
            /// <param name="context">
            /// The context that provides everything required for the deserialization process.
            /// </param>
            public override void Deserialize( SpriteDatabase @object, IDeserializationContext context )
            {
                const int CurrentVersion = 1;
                int version = context.ReadInt32();
                ThrowHelper.InvalidVersion( version, CurrentVersion, typeof( SpriteDatabase ) );

                @object.Name = context.ReadString();

                // Sprites:
                @object.Atlas = new SpriteAtlas();
                this.spriteAtlasReaderWriter.Deserialize( @object.Atlas, context );

                // Animated Sprites:
                int animatedSpriteCount = context.ReadInt32();
                var animatedSprites = @object.animatedSprites;

                animatedSprites.Capacity = animatedSpriteCount;

                var animatedSpriteReaderWriter = CreateAnimatedSpriteReaderWriter( @object );
                for( int i = 0; i < animatedSpriteCount; ++i )
                {
                    var animatedSprite = new AnimatedSprite();
                    animatedSpriteReaderWriter.Deserialize( animatedSprite, context );
                    animatedSprites.Add( animatedSprite );
                }
            }

            /// <summary>
            /// Creates the IObjectReaderWriter for AnimatedSprites.
            /// </summary>
            /// <param name="database">
            /// The database from which the individual frame sprites should be loaden.
            /// </param>
            /// <returns>
            /// The newly created IObjectReaderWriter.
            /// </returns>
            private static IObjectReaderWriter<AnimatedSprite> CreateAnimatedSpriteReaderWriter( SpriteDatabase database )
            {
                return new AnimatedSprite.ReaderWriter(
                    new AnimatedSpriteFrame.ReaderWriter(
                        new SpriteDatabase.SpriteLoader( database )
                    )
                );
            }

            /// <summary>
            /// The IObjectReaderWriter that is used to de-/serialize SpriteAtlas objects.
            /// </summary>
            private readonly IObjectReaderWriter<SpriteAtlas> spriteAtlasReaderWriter;
        }
    }
}
