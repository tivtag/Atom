// <copyright file="AnimatedSpriteFrame.ReaderWriter.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Xna.AnimatedSpriteFrame.ReaderWriter class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Xna
{
    using System;
    using Atom.Diagnostics.Contracts;
    using Atom.Math;
    using Atom.Storage;

    /// <content>
    /// Defines the default <see cref="IObjectReaderWriter"/> for the AnimatedSpriteFrame class.
    /// </content>
    public partial class AnimatedSpriteFrame
    {
        /// <summary>
        /// Defines the IObjectReaderWriter that implements serialization and deserialization
        /// of <see cref="AnimatedSprite"/> objects. This class can't be inherited.
        /// </summary>
        public sealed class ReaderWriter : BaseObjectReaderWriter<AnimatedSpriteFrame>
        {
            /// <summary>
            /// Initializes a new instance of the ReaderWriter class.
            /// </summary>
            /// <param name="spriteLoader">
            /// The <see cref="INormalSpriteLoader"/> that should be used to load the <see cref="Sprite"/> of the frames.
            /// </param>
            public ReaderWriter( INormalSpriteLoader spriteLoader )
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
            public override void Serialize( AnimatedSpriteFrame @object, ISerializationContext context )
            {
                context.Write( @object.time );
                context.Write( @object.offset );
                context.Write( @object.sprite != null ? (@object.sprite.Name ?? string.Empty) : string.Empty );
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
            public override void Deserialize( AnimatedSpriteFrame @object, IDeserializationContext context )
            {
                @object.time = context.ReadSingle();
                @object.offset = context.ReadVector2();
                @object.sprite = this.spriteLoader.LoadSprite( context.ReadString() );
            }

            /// <summary>
            /// The <see cref="INormalSpriteLoader"/> that should be used to load the <see cref="Sprite"/> of the frames.
            /// </summary>
            private readonly INormalSpriteLoader spriteLoader;
        }
    }
}
