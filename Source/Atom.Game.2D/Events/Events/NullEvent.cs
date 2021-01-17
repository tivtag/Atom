// <copyright file="NullEvent.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Events.NullEvent class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Events
{
    /// <summary>
    /// Defines an <see cref="Event"/> that does nothing.
    /// </summary>
    internal sealed class NullEvent : Event
    {
        /// <summary>
        /// A read-only instance of the <see cref="NullEvent"/> class.
        /// </summary>
        public static readonly NullEvent Instance = new NullEvent();

        /// <summary>
        /// Does nothing.
        /// </summary>
        /// <param name="obj">
        /// This parameter is not used.
        /// </param>
        /// <returns>
        /// Always returns true.
        /// </returns>
        public override bool CanBeTriggeredBy( object obj )
        {
            return true;
        }

        /// <summary>
        /// Does nothing.
        /// </summary>
        /// <param name="obj">
        /// This parameter is not used.
        /// </param>
        public override void Trigger( object obj )
        {
        }
    }
}
