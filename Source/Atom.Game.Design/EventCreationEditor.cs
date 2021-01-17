// <copyright file="EventCreationEditor.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Events.Design.EventCreationEditor class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Events.Design
{
    /// <summary>
    /// Defines an <see cref="System.Drawing.Design.UITypeEditor"/> that
    /// when clicked on opens up a list of Event Types.
    /// This is a sealed class.
    /// </summary>
    public sealed class EventCreationEditor : BaseEventCreationEditor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EventCreationEditor"/> class.
        /// </summary>
        public EventCreationEditor()
            : base( EventDataType.Event )
        {
        }
    }
}
