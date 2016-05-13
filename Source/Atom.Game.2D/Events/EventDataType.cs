// <copyright file="EventDataType.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Events.EventDataType enumeration.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Events
{
    /// <summary>
    /// Enumerates the different basic Event Data Types.
    /// </summary>
    public enum EventDataType
    {
        /// <summary>
        /// An <see cref="Atom.Events.Event"/> Type.
        /// </summary>
        Event,

        /// <summary>
        /// An <see cref="Atom.Events.EventTrigger"/> Type.
        /// </summary>
        Trigger
    }
}