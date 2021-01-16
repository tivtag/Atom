// <copyright file="ExistingEventCollectionEditor.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Events.Design.ExistingEventCollectionEditor class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Events.Design
{
    using Atom.Design;

    /// <summary>
    /// Represents an <see cref="Atom.Design.ExistingItemCollectionEditor{Event}"/> that
    /// can be attached to a property that returns a collection of <see cref="Event"/>s.
    /// </summary>
    /// <remarks>
    /// Using this UITypeEditor one can insert <b>existing</b> <see cref="Event"/>s into a collection of events.
    /// </remarks>
    public sealed class ExistingEventCollectionEditor : Atom.Design.ExistingItemCollectionEditor<Event>
    {
        /// <summary>
        /// Creates the <see cref="Atom.Design.IItemSelectionDialog{Event}"/> this ExistingItemCollectionEditor{TItem}
        /// uses internally. Must be overriden by users of this class.
        /// </summary>
        /// <returns>
        /// The <see cref="Atom.Design.IItemSelectionDialog{Event}"/> that should be used by this ExistingItemCollectionEditor{TItem}.
        /// </returns>
        public override IItemSelectionDialog<Event> CreateSelectionDialog()
        {
            IEventManagerService service = GlobalServices.GetService<IEventManagerService>();

            EventManager eventManager = service.EventManager;
            ThrowHelper.IfServiceNull<EventManager>( eventManager, EventStrings.Error_EventManagerReturnedByEventServiceIsNull );

            IItemSelectionDialogFactory selectionDialogFactory = GlobalServices.GetService<IItemSelectionDialogFactory>();
            return selectionDialogFactory.Build<Event>( eventManager.Events );
        }
    }
}
