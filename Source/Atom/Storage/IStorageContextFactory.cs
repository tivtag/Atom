// <copyright file="IStorageContextFactory.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the 
//     Atom.Storage.IStorageContextFactory{TObject, TSerializationContext, TDeserializationContext, TSerializationBuildContext, TDeserializationBuildContext}
//     interface.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Storage
{
    /// <summary>
    /// Provides a mechanism that creates <see cref="ISerializationContext"/> and <see cref="IDeserializationContext"/>
    /// objects for a specific object type.
    /// </summary>
    /// <typeparam name="TObject">
    /// The type of the object supposed to be deserialized / serialized.
    /// </typeparam>
    /// <typeparam name="TSerializationContext">
    /// The type of the ISerializationContext created by the IStorageContextFactory.
    /// </typeparam>
    /// <typeparam name="TDeserializationContext">
    /// The type of the IDeserializationContext created by the IStorageContextFactory.
    /// </typeparam>
    /// <typeparam name="TSerializationBuildContext">
    /// The context under which the ISerializationContext is build.
    /// </typeparam>
    /// <typeparam name="TDeserializationBuildContext">
    /// The context under which the IDeserializationContext is build.
    /// </typeparam>
    public interface IStorageContextFactory<TObject, TSerializationContext, TDeserializationContext, TSerializationBuildContext, TDeserializationBuildContext>
        where TSerializationContext : ISerializationContext
        where TDeserializationContext : IDeserializationContext
    {
        /// <summary>
        /// Builds a new <see cref="ISerializationContext"/> for the given object.
        /// </summary>
        /// <param name="object">
        /// The object that is supposed to be serialized.
        /// </param>
        /// <param name="buildContext">
        /// The additional context under which the TSerializationContext is build.
        /// </param>
        /// <returns>
        /// The newly created TSerializationContext.
        /// </returns>
        TSerializationContext BuildSerializationContext( TObject @object, TSerializationBuildContext buildContext );
        
        /// <summary>
        /// Builds a new <see cref="IDeserializationContext"/> for the given object.
        /// </summary>
        /// <param name="object">
        /// The object that is supposed to be deserialized.
        /// </param>
        /// <param name="buildContext">
        /// The additional context under which the TDeserializationContext is build.
        /// </param>
        /// <returns>
        /// The newly created TDeserializationContext.
        /// </returns>
        TDeserializationContext BuildDeserializationContext( TObject @object, TDeserializationBuildContext buildContext );
    }
}
