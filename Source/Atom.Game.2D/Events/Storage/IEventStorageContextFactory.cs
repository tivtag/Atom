// <copyright file="IEventStorageContextFactory.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Events.IEventStorageContextFactory interface.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Events
{
    using System.IO;
    using Atom.Storage;

    /// <summary>
    /// Provides a mechanism that builds new <see cref="IEventSerializationContext"/> and <see cref="IEventDeserializationContext"/>
    /// objects.
    /// </summary>
    public interface IEventStorageContextFactory
        : IStorageContextFactory<EventManager, IEventSerializationContext, IEventDeserializationContext, 
                                 BinaryWriter, BinaryReader>
    {
    }
}
