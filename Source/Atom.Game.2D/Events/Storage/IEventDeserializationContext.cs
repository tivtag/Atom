// <copyright file="IEventDeserializationContext.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Events.IEventDeserializationContext interface.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Events
{
    using Atom.Storage;

    /// <summary>
    /// Provides a mechanism that allows deserialization of <see cref="Event"/>s, <see cref="EventTrigger"/>s
    /// and <see cref="EventManager"/>s.
    /// </summary>
    public interface IEventDeserializationContext : IBinaryDeserializationContext, IEventStorageContext
    {
    }
}
