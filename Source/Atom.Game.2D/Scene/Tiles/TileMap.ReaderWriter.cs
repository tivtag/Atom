// <copyright file="TileMap.ReaderWriter.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Scene.Tiles.TileMap.ReaderWriter class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Scene.Tiles
{
    using System;
    using System.Diagnostics.Contracts;
    using Atom.Storage;

    /// <content>
    /// Defines the <see cref="IObjectReaderWriter"/> for the TileMap class.
    /// </content>
    public partial class TileMap
    {
        /// <summary>
        /// Implements a mechanism that serializes / deserializes <see cref="TileMap"/> objects.
        /// </summary>
        public class ReaderWriter : BaseObjectReaderWriter<TileMap>
        {
            /// <summary>
            /// Initializes a new instance of the ReaderWriter class.
            /// </summary>
            /// <param name="floorReaderWriter">
            /// Responsible for serializing and deserializing the TileMapFloors that are part of the TileMap.
            /// </param>
            public ReaderWriter( IObjectReaderWriter<TileMapFloor> floorReaderWriter )
            {
                Contract.Requires<ArgumentNullException>( floorReaderWriter != null );

                this.floorReaderWriter = floorReaderWriter;
            }

            /// <summary>
            /// Serializes the given TileMap using the given ISerializationContext.
            /// </summary>
            /// <param name="object">
            /// The object to serialize.
            /// </param>
            /// <param name="context">
            /// The context that provides everything required for the serialization process.
            /// </param>
            public override void Serialize( TileMap @object, ISerializationContext context )
            {
                const int CurrentVersion = 0;
                context.Write( CurrentVersion );

                context.Write( @object.Name );
                context.Write( @object.width );
                context.Write( @object.height );
                context.Write( @object.tileSize );
                context.Write( @object.floors.Count );

                foreach( var floor in @object.floors )
                {
                    this.floorReaderWriter.Serialize( floor, context );
                }
            }

            /// <summary>
            /// Deserializes the given TileMap using the given IDeserializationContext.
            /// </summary>
            /// <param name="object">
            /// The object to deserialize.
            /// </param>
            /// <param name="context">
            /// The context that provides everything required for the deserialization process.
            /// </param>
            public override void Deserialize( TileMap @object, IDeserializationContext context )
            {
                const int CurrentVersion = 0;
                int version = context.ReadInt32();
                ThrowHelper.InvalidVersion( version, CurrentVersion, this.GetType() );

                @object.Name = context.ReadString();
                @object.width = context.ReadInt32();
                @object.height = context.ReadInt32();
                @object.tileSize = context.ReadInt32();

                // Load Floors:
                int floorCount = context.ReadInt32();
                @object.floors.Clear();
                @object.floors.Capacity = floorCount;

                for( int i = 0; i < floorCount; ++i )
                {
                    var floor = @object.AddFloor( 0 );
                    this.floorReaderWriter.Deserialize( floor, context );
                }
            }

            /// <summary>
            /// Responsible for serializing and deserializing the TileMapFloors that are part of the TileMap.
            /// </summary>
            private readonly IObjectReaderWriter<TileMapFloor> floorReaderWriter;
        }
    }
}