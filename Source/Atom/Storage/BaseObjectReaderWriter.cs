// <copyright file="BaseObjectReaderWriter.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Storage.BaseObjectReaderWriter class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Storage
{
    using System;

    /// <summary>
    /// Represents an <see cref="IObjectReaderWriter"/> that serializes / deserializes objects
    /// of type <typeparamref name="TObject"/>.
    /// </summary>
    /// <typeparam name="TObject">
    /// The type of the object this BaseObjectReaderWriter{TObject} can serialize / deserialize.
    /// </typeparam>
    public abstract class BaseObjectReaderWriter<TObject> : IObjectReaderWriter<TObject>
    {
        /// <summary>
        /// Gets the <see cref="Type"/> this IObjectReaderWriter serializes and deserializes.
        /// </summary>
        public Type Type
        {
            get 
            {
                return typeof( TObject );
            }
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
        public void Serialize( object @object, ISerializationContext context )
        {
            this.Serialize( (TObject)@object, context );
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
        public void Deserialize( object @object, IDeserializationContext context )
        {
            this.Deserialize( (TObject)@object, context );
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
        public abstract void Serialize( TObject @object, ISerializationContext context );

        /// <summary>
        /// Deserializes the given object using the given IDeserializationContext.
        /// </summary>
        /// <param name="object">
        /// The object to deserialize.
        /// </param>
        /// <param name="context">
        /// The context that provides everything required for the deserialization process.
        /// </param>
        public abstract void Deserialize( TObject @object, IDeserializationContext context );
    }
}
