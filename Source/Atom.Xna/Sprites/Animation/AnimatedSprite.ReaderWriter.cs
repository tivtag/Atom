// <copyright file="AnimatedSprite.ReaderWriter.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Xna.AnimatedSprite.ReaderWriter class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Xna
{
    using System;
    using Atom.Diagnostics.Contracts;
    using Atom.Storage;

    /// <content>
    /// Defines the default IObjectReaderWriter for the AnimatedSprite class.
    /// </content>
    public partial class AnimatedSprite
    {
        /// <summary>
        /// Defines the IObjectReaderWriter that implements serialization and deserialization
        /// of <see cref="AnimatedSprite"/> objects. This class can't be inherited.
        /// </summary>
        public sealed class ReaderWriter : BaseObjectReaderWriter<AnimatedSprite>
        {
            /// <summary>
            /// Initializes a new instance of the ReaderWriter class.
            /// </summary>
            /// <param name="frameReaderWriter">
            /// The IObjectReaderWriter that should be used to de-/serialize the individual AnimatedSpriteFrames.
            /// </param>
            public ReaderWriter( IObjectReaderWriter<AnimatedSpriteFrame> frameReaderWriter )
            {
                Contract.Requires<ArgumentNullException>( frameReaderWriter != null );

                this.frameReaderWriter = frameReaderWriter;
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
            public override void Serialize( AnimatedSprite @object, ISerializationContext context )
            {
                context.Write( @object.Name ?? string.Empty );
                context.Write( @object.IsLoopingByDefault );
                context.Write( @object.DefaultAnimationSpeed );
                context.Write( @object.frames.Length );

                for( int i = 0; i < @object.frames.Length; ++i )
                {
                    this.frameReaderWriter.Serialize( @object.frames[i], context );
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
            public override void Deserialize( AnimatedSprite @object, IDeserializationContext context )
            {
                @object.Name = context.ReadString();
                @object.IsLoopingByDefault = context.ReadBoolean();
                @object.DefaultAnimationSpeed = context.ReadSingle();

                int frameCount = context.ReadInt32();
                var frames = new AnimatedSpriteFrame[frameCount];

                for( int i = 0; i < frameCount; ++i )
                {
                    var frame = new AnimatedSpriteFrame();
                    this.frameReaderWriter.Deserialize( frame, context );

                    frames[i] = frame;
                }

                @object.SetFrames( frames );
            }

            /// <summary>
            /// The IObjectReaderWriter that is used to de-/serialize the individual AnimatedSpriteFrames.
            /// </summary>
            private readonly IObjectReaderWriter<AnimatedSpriteFrame> frameReaderWriter;
        }
    }
}
