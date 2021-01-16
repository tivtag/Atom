// <copyright file="EventSelectionOrCreationDialog.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Events.Design.EventSelectionOrCreationDialog class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Events.Design
{
    using System;
    using System.Windows.Forms;

    /// <summary>
    /// Provides a small dialog from which the user can
    /// choose whether he wants to select an existing Event
    /// or create a new one.
    /// </summary>
    internal partial class EventSelectionOrCreationDialog : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EventSelectionOrCreationDialog"/> class.
        /// </summary>
        public EventSelectionOrCreationDialog()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Shows the <see cref="EventSelectionOrCreationDialog"/>,
        /// returning the result of the 'query'.
        /// </summary>
        /// <returns>
        /// The mode that has been selcted by the user.
        /// </returns>
        public new EventSelectionOrCreation ShowDialog()
        {
            base.ShowDialog();
            return mode;
        }

        /// <summary>
        /// Called when the user clicks on the 'Select' button.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The EventArgs that contains the event data.</param>
        private void OnSelectButtonClicked( object sender, EventArgs e )
        {
            this.mode = EventSelectionOrCreation.Select;
            this.DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// Called when the user clicks on the 'Create' button.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The EventArgs that contains the event data.</param>
        private void OnCreateButtonClicked( object sender, EventArgs e )
        {
            this.mode = EventSelectionOrCreation.Create;
            this.DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// The currently selected mode.
        /// </summary>
        private EventSelectionOrCreation mode = EventSelectionOrCreation.None;
    }

    /// <summary>
    /// Enumerates the different modi the user
    /// can select within the <see cref="EventSelectionOrCreationDialog"/>.
    /// </summary>
    internal enum EventSelectionOrCreation
    {
        /// <summary>
        /// Nothing has been set.
        /// </summary>
        None,

        /// <summary>
        /// An existing even should be selected.
        /// </summary>
        Select,

        /// <summary>
        /// A new event should be created.
        /// </summary>
        Create
    }
}