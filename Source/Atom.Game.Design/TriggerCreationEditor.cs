// <copyright file="TriggerCreationEditor.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Events.Design.TriggerCreationEditor class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Events.Design
{
    /// <summary>
    /// Defines an <see cref="System.Drawing.Design.UITypeEditor"/> that
    /// when clicked on opens up a list of EventTrigger Types.
    /// This is a sealed class.
    /// </summary>
    public sealed class TriggerCreationEditor : BaseEventCreationEditor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TriggerCreationEditor"/> class.
        /// </summary>
        public TriggerCreationEditor()
            : base( EventDataType.Trigger )
        {
        }
    }
}
