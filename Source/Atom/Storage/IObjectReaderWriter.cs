// <copyright file="IObjectReaderWriter.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Storage.IObjectReaderWriter interface.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Storage
{
    using System;

    /// <summary>
    /// Provides a mechanism that allows the serialization and deserialization
    /// of a specific object type.
    /// </summary>
    public interface IObjectReaderWriter : IObjectReader, IObjectWriter
    {
    }
}
