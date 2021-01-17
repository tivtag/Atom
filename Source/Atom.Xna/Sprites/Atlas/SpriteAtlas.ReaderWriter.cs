// <copyright file="SpriteAtlas.ReaderWriter.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Xna.SpriteAtlas.ReaderWriter class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Xna
{
    using System;
    using Atom.Diagnostics.Contracts;
    using Atom.Math;
    using Atom.Storage;
    using Microsoft.Xna.Framework.Graphics;

    /// <content>
    /// Defines the default IObjectReaderWriter for the SpriteAtlas class.
    /// </content>
    public partial class SpriteAtlas
    {
        /// <summary>
        /// Defines the IObjectReaderWriter that implements serialization and deserialization
        /// of <see cref="SpriteAtlas"/> objects. This class can't be inherited.
        /// </summary>
        public sealed class ReaderWriter : BaseObjectReaderWriter<SpriteAtlas>
        {
            /// <summary>
            /// Initializes a new instance of the ReaderWriter class.
            /// </summary>
            /// <param name="textureLoader">
            /// Provides a mechanism that allows loading of Texture2D assets.
            /// </param>
            public ReaderWriter( ITexture2DLoader textureLoader )
            {
                Contract.Requires<ArgumentNullException>( textureLoader != null );

                this.textureLoader = textureLoader;
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
            public override void Serialize( SpriteAtlas @object, ISerializationContext context )
            {
                @object.Verify();

                context.Write( @object.Texture != null ? @object.Texture.Name ?? string.Empty : string.Empty );
                context.Write( @object.sprites.Count );

                var spriteReaderWriter = SpriteReaderWriter.Instance;

                foreach( var sprite in @object.sprites )
                {
                    spriteReaderWriter.Serialize( sprite, context );
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
            public override void Deserialize( SpriteAtlas @object, IDeserializationContext context )
            {
                string textureName = context.ReadString();

                if( textureName.Length > 0 )
                {
                    @object.Texture = this.textureLoader.Load( textureName );
                }

                int spriteCount = context.ReadInt32();
                @object.sprites.Capacity = spriteCount;

                var spriteReaderWriter = SpriteReaderWriter.Instance;
                
                for( int i = 0; i < spriteCount; ++i )
                {
                    Sprite sprite = new Sprite() {
                        Texture = @object.Texture
                    };

                    spriteReaderWriter.Deserialize( sprite, context );
                    @object.sprites.Add( sprite );
                }
            }

            /// <summary>
            /// Provides a mechanism that allows loading of Texture2D assets.
            /// </summary>
            private readonly ITexture2DLoader textureLoader;

            /// <summary>
            /// Defines an IObjectReaderWriter that implements serialization and deserialization
            /// of <see cref="Sprite"/> objects to be stored in a SpriteAtlas.
            /// </summary>
            private sealed class SpriteReaderWriter : BaseObjectReaderWriter<Sprite>
            {
                /// <summary>
                /// Represents an instance of the SpriteReaderWriter class.
                /// </summary>
                public static readonly SpriteReaderWriter Instance = new SpriteReaderWriter();

                /// <summary>
                /// Serializes the given object using the given ISerializationContext.
                /// </summary>
                /// <param name="object">
                /// The object to serialize.
                /// </param>
                /// <param name="context">
                /// The context that provides everything required for the serialization process.
                /// </param>
                public override void Serialize( Sprite @object, ISerializationContext context )
                {
                    context.Write( @object.Name );
                    context.Write( @object.Source );
                    context.Write( @object.Color );
                    context.Write( @object.Scale );
                    context.Write( @object.Origin );
                    context.Write( (int)@object.Effects );
                    context.Write( @object.Rotation );
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
                public override void Deserialize( Sprite @object, IDeserializationContext context )
                {
                    @object.Name = context.ReadString();
                    @object.Source = context.ReadRectangle();
                    @object.Color = context.ReadColor();
                    @object.Scale = context.ReadVector2();
                    @object.Origin = context.ReadVector2();
                    @object.Effects = (SpriteEffects)context.ReadInt32();
                    @object.Rotation = context.ReadSingle();
                }
            }
        }
    }
}
