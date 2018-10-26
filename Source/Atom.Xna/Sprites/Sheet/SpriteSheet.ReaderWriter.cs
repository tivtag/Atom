// <copyright file="SpriteSheet.ReaderWriter.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Xna.SpriteSheet.ReaderWriter class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>
 
namespace Atom.Xna
{
    using System;
    using Atom.Diagnostics.Contracts;
    using Atom.Storage;
    
    /// <content>
    /// Defines the default IObjectReaderWriter for the Sprite class.
    /// </content>
    public partial class SpriteSheet
    {
        /// <summary>
        /// Defines the default IObjectReaderWriter that implements serialization and deserialization
        /// of <see cref="SpriteSheet"/> objects. This class can't be inherited.
        /// </summary>
        public sealed class ReaderWriter : BaseObjectReaderWriter<SpriteSheet>
        {
            /// <summary>
            /// The extension of the SpriteSheet files: '.sprsh'
            /// </summary>
            public const string Extension = ".sprsh";

            /// <summary>
            /// Initializes a new instance of the ReaderWriter class.
            /// </summary>
            /// <param name="spriteLoader">
            /// Provides a mechanism that allows loading of <see cref="ISprite"/> assets.
            /// </param>
            public ReaderWriter( ISpriteLoader spriteLoader )
            {
                Contract.Requires<ArgumentNullException>( spriteLoader != null );

                this.spriteLoader = spriteLoader;
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
            public override void Serialize( SpriteSheet @object, ISerializationContext context )
            {
                context.Write( @object.Name ?? string.Empty );
                context.Write( @object.Count );

                for( int index = 0; index < @object.Count; ++index )
                {
                    ISprite sprite = @object[index];

                    if( sprite != null )
                    {
                        context.Write( @object[index].Name );
                    }
                    else
                    {
                        context.Write( string.Empty );
                    }
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
            public override void Deserialize( SpriteSheet @object, IDeserializationContext context )
            {
                @object.Name = context.ReadString();

                int count = context.ReadInt32();
                @object.sprites.Capacity = count;

                for( int index = 0; index < count; ++index )
                {
                    string spriteName = context.ReadString();
                    
                    if( spriteName.Length > 0 )
                    {
                        ISprite sprite = this.spriteLoader.Load( spriteName );
                        @object.Add( sprite );
                    }
                    else
                    {
                        @object.Add( null );
                    }
                }
            }

            /// <summary>
            /// Provides a mechanism that allows loading of <see cref="ISprite"/> assets.
            /// </summary>
            private readonly ISpriteLoader spriteLoader;
        }
    }
}
