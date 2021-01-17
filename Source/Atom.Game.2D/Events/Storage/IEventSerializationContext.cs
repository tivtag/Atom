// <copyright file="IEventSerializationContext.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Events.IEventSerializationContext interface.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Events
{
    using Atom.Storage;

    /// <summary>
    /// Provides a mechanism that allows serialization of <see cref="Event"/>s, <see cref="EventTrigger"/>s,
    /// and <see cref="EventManager"/>s.
    /// </summary>
    public interface IEventSerializationContext : IBinarySerializationContext, IEventStorageContext
    {
    }
}
