// <copyright file="Sprite.ReaderWriter.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Xna.Sprite.ReaderWriter class.
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
    using Microsoft.Xna.Framework.Graphics;

    /// <content>
    /// Defines the default IObjectReaderWriter for the Sprite class.
    /// </content>
    public partial class Sprite
    {
        /// <summary>
        /// Defines the default IObjectReaderWriter that implements serialization and deserialization
        /// of <see cref="Sprite"/> objects. This class can't be inherited.
        /// </summary>
        public sealed class ReaderWriter : BaseObjectReaderWriter<Sprite>
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
            public override void Serialize( Sprite @object, ISerializationContext context )
            {
                context.Write( @object.Name );
                context.Write( @object.Texture != null ? string.Empty : @object.Texture.Name );
                context.Write( @object.Source );
                context.Write( @object.defaultColor );
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
                @object.texture = this.textureLoader.Load( context.ReadString() );
                @object.Source = context.ReadRectangle();
                @object.defaultColor = context.ReadColor();
                @object.Scale = context.ReadVector2();
                @object.Origin = context.ReadVector2();
                @object.Effects = (SpriteEffects)context.ReadInt32();
                @object.Rotation = context.ReadSingle();
            }

            /// <summary>
            /// Provides a mechanism that allows loading of Texture2D assets.
            /// </summary>
            private readonly ITexture2DLoader textureLoader;
        }
    }
}
