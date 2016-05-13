// <copyright file="BaseEventCreationEditor.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Events.Design.BaseEventCreationEditor class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Events.Design
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing.Design;
    using System.Security.Permissions;
    using System.Windows.Forms;
    using Atom.Design;
    using System.Collections.Generic;

    /// <summary>
    /// Defines an <see cref="System.Drawing.Design.UITypeEditor"/> that
    /// when clicked on opens up a list of Event/EventTrigger Types.
    /// !! Warning: The <see cref="BaseEventCreationEditor"/> requires
    /// an <see cref="IEventManagerService"/> to be available from the <see cref="Atom.GlobalServices"/>.
    /// </summary>
    public abstract class BaseEventCreationEditor : UITypeEditor
    {
        #region [ Constructors ]

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseEventCreationEditor"/> class.
        /// </summary>
        /// <param name="mode">
        /// States what kind of Event Data is created by the <see cref="EventCreationEditor"/>.
        /// </param>
        protected BaseEventCreationEditor( EventDataType mode )
        {
            this.mode = mode;
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Gets called when the user tries to edit the value
        /// this Editor is attached to.
        /// </summary>
        /// <param name="context">
        /// An ITypeDescriptorContext that can be used to optain additional context information.
        /// </param>
        /// <param name="provider">
        /// An IServiceProvider that this editor can use to obtain services.
        /// </param>
        /// <param name="value">
        /// The value currently edited.
        /// </param>
        /// <returns>
        /// The new value.
        /// </returns>
        public override object EditValue( ITypeDescriptorContext context, IServiceProvider provider, object value )
        {
            var service = GlobalServices.GetService<IEventManagerService>();
            var eventManager = service.EventManager;
            ThrowHelper.IfServiceNull<EventManager>( eventManager, EventStrings.Error_EventManagerReturnedByEventServiceIsNull );

            // Ask the user.
            using( var dialog = new EventSelectionOrCreationDialog() )
            {
                var selectionOrCreation = dialog.ShowDialog();

                if( selectionOrCreation == EventSelectionOrCreation.Create )
                {
                    return ShowCreateNewDialog( eventManager, value );
                }
                else
                {
                    return ShowSelectDialog( eventManager, value );
                }
            }
        }

        /// <summary>
        /// Shows the dialog that allows the user to create new Event Data.
        /// </summary>
        /// <param name="eventManager">
        /// The EventManager object.
        /// </param>
        /// <param name="value">
        /// The value currently edited.
        /// </param>
        /// <returns>
        /// The new value.
        /// </returns>
        private object ShowCreateNewDialog( EventManager eventManager, object value )
        {
            using( var dialog = new EventCreationDialog( mode, eventManager ) )
            {
                if( value != null )
                {
                    Type type = value.GetType();
                    dialog.SelectedType = type;
                }

                if( dialog.ShowDialog() == DialogResult.OK )
                {
                    Type type = dialog.SelectedType;
                    if( type == null )
                        return null;

                    value = Activator.CreateInstance( type );
                    Event e = value as Event;

                    if( e != null )
                    {
                        e.Name = dialog.EnteredName;
                        eventManager.Add( e );
                    }
                    else
                    {
                        var trigger = value as EventTrigger;
                        Debug.Assert( trigger != null, "The given value is not an Event nor an EventTrigger." );

                        trigger.Name = dialog.EnteredName;
                        eventManager.Add( trigger );
                    }
                }
            }

            return value;
        }

        /// <summary>
        /// Shows the dialog that allows the user to select an existing Event Data object.
        /// </summary>
        /// <param name="eventManager">
        /// The EventManager object.
        /// </param>
        /// <param name="value">
        /// The value currently edited.
        /// </param>
        /// <returns>
        /// The new value.
        /// </returns>
        private object ShowSelectDialog( EventManager eventManager, object value )
        {
            var dialogFactory = GlobalServices.GetService<IItemSelectionDialogFactory>();

            IEnumerable<IReadOnlyNameable> items;            
            if( this.mode == EventDataType.Event )
            {
                items = eventManager.Events;
            }
            else
            {
                items = eventManager.Triggers;
            }

            var dialog = dialogFactory.Build<IReadOnlyNameable>( items );
            {
                if( value != null )
                    dialog.SelectedItem = value as IReadOnlyNameable;

                if( dialog.ShowDialog() )
                {
                    return dialog.SelectedItem;
                }
            }

            return value;
        }

        /// <summary>
        /// Overriden to return what <see cref="UITypeEditorEditStyle"/> the custom Editor uses.
        /// </summary>
        /// <param name="context">
        /// An ITypeDescriptorContext that can be used to optain additional context information.
        /// </param>
        /// <returns>
        /// Returns <see cref="UITypeEditorEditStyle.Modal"/>.
        /// </returns>
        public override UITypeEditorEditStyle GetEditStyle( System.ComponentModel.ITypeDescriptorContext context )
        {
            return UITypeEditorEditStyle.Modal;
        }

        #endregion

        #region [ Fields ]

        /// <summary>
        /// States what kind of EventData this Editor is editting.
        /// </summary>
        private readonly EventDataType mode;

        #endregion
    }
}