// <copyright file="PermanentEvent.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Events.PermanentEvent class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Events
{
    /// <summary>
    /// Represents an <see cref="Event"/> that is permanently active
    /// and as such gets updated each frame.
    /// </summary>
    /// <remarks>
    /// The difference between a <see cref="PermanentEvent"/> and a <see cref="LongTermEvent"/> is
    /// that the <see cref="LongTermEvent"/> adds itself to the EventManager when it gets triggered.
    /// This is not required with <see cref="PermanentEvent"/>s.
    /// <para>
    /// In the case of a game a <see cref="PermanentEvent"/> may contain game logic that is
    /// constantly active and doesn't need to be activated by for example an <see cref="EventTrigger"/>.
    /// </para>
    /// </remarks>
    public abstract class PermanentEvent : Event
    {
        /// <summary>
        /// Updates this <see cref="PermanentEvent"/>.
        /// </summary>
        /// <remarks>
        /// This method is called once per frame on the PermanentEvent <see cref="LongTermEvent"/>s.
        /// </remarks>
        /// <param name="updateContext">
        /// The current IUpdateContext.
        /// </param>
        public abstract void Update( IUpdateContext updateContext );

        /// <summary>
        /// Has been overriden to do nothing. PermanentEvents are
        /// permanently active.
        /// </summary>
        /// <param name="obj">
        /// The object that triggered the event.
        /// </param>
        public override void Trigger( object obj )
        {
        }
    }
}
