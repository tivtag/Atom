// <copyright file="TileMapFloor.ReaderWriter.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Scene.Tiles.TileMapFloor.ReaderWriter class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Scene.Tiles
{
    using System;
    using Atom.Diagnostics.Contracts;
    using Atom.Storage;

    /// <content>
    /// Defines the <see cref="IObjectReaderWriter"/> for the TileMapFloor class.
    /// </content>
    public partial class TileMapFloor
    {
        /// <summary>
        /// Implements a mechanism that serializes / deserializes <see cref="TileMapFloor"/> objects.
        /// </summary>
        public class ReaderWriter : BaseObjectReaderWriter<TileMapFloor>
        {
            /// <summary>
            /// Initializes a new instance of the ReaderWriter class.
            /// </summary>
            /// <param name="layerTypeActivator">
            /// Responsible for creating the <see cref="TileMapDataLayer"/> objects serialized in a TileMapFloor.
            /// </param>
            public ReaderWriter( ITypeActivator layerTypeActivator )
            {
                Contract.Requires<ArgumentNullException>( layerTypeActivator != null );

                this.layerTypeActivator = layerTypeActivator;
            }

            /// <summary>
            /// Serializes the specified TileMapFloor object.
            /// </summary>
            /// <param name="object">
            /// The object to serialize.
            /// </param>
            /// <param name="context">
            /// The context that provides everything required for the serialization process.
            /// </param>
            public override void Serialize( TileMapFloor @object, Atom.Storage.ISerializationContext context )
            {
                const int CurrnetVersion = 0;
                context.Write( CurrnetVersion );

                context.Write( @object.layers.Count );
                foreach( var layer in @object.layers )
                {
                    layer.SerializeCore( context );
                }

                if( @object.actionLayer != null )
                {
                    context.Write( true );
                    @object.actionLayer.SerializeCore( context );
                }
                else
                {
                    context.Write( false );
                }
            }

            /// <summary>
            /// Deserializes the specified TileMapFloor object.
            /// </summary>
            /// <param name="object">
            /// The object to deserialize.
            /// </param>
            /// <param name="context">
            /// The context that provides everything required for the deserialization process.
            /// </param>
            public override void Deserialize( TileMapFloor @object, Atom.Storage.IDeserializationContext context )
            {
                const int CurrnetVersion = 0;
                int version = context.ReadInt32();
                System.Diagnostics.Debug.Assert( version == CurrnetVersion, "Version mismatch." );

                // Load Layers
                int layerCount = context.ReadInt32();
                @object.layers.Clear();
                @object.layers.Capacity = layerCount;

                for( int i = 0; i < layerCount; ++i )
                {
                    string typeName = context.ReadString();
                    var layer = (TileMapDataLayer)this.layerTypeActivator.CreateInstance( typeName );

                    layer.Floor = @object;
                    layer.DeserializeCore( context );
                    @object.layers.Add( layer );
                }

                // Load ActionLayer
                bool hasActionLayer = context.ReadBoolean();
                if( hasActionLayer )
                {
                    string typeName = context.ReadString();
                    @object.actionLayer = (TileMapDataLayer)this.layerTypeActivator.CreateInstance( typeName );

                    @object.actionLayer.Floor = @object;
                    @object.actionLayer.DeserializeCore( context );
                }
                else
                {
                    @object.actionLayer = null;
                }
            }

            /// <summary>
            /// Responsible for creating the <see cref="TileMapDataLayer"/> objects serialized in a TileMapFloor.
            /// </summary>
            private readonly ITypeActivator layerTypeActivator;
        }
    }
}
