// <copyright file="IObjectReaderWriter.TObject.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Storage.IObjectReaderWriter{TObject} interface.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Storage
{
    /// <summary>
    /// Provides a mechanism that allows the serialization and deserialization
    /// of a specific object type.
    /// </summary>
    /// <typeparam name="TObject">
    /// The type of the object that is serialized and deserialized.
    /// </typeparam>
    public interface IObjectReaderWriter<TObject> : 
        IObjectReaderWriter, IObjectReader<TObject>, IObjectWriter<TObject>
    {
    }
}
