// <copyright file="EventCreationDialog.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Events.Design.EventCreationDialog class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Events.Design
{
    using System;
    using Atom.Diagnostics.Contracts;
    using System.Windows.Forms;
    
    /// <summary>
    /// Defines a WinForms dialog that can be used
    /// to select an <see cref="Event"/> type or an <see cref="EventTrigger"/> type
    /// which has been registered on the <see cref="EventTypeRegister"/>.
    /// This is a sealed class.
    /// </summary>
    public sealed partial class EventCreationDialog : Form
    {
        #region [ Properties ]
        
        /// <summary>
        /// Gets or sets the <see cref="Event"/> or <see cref="EventTrigger"/> <see cref="Type"/> 
        /// which is selected in the <see cref="EventCreationDialog"/>.
        /// </summary>
        /// <value>The type that has been selected by the user.</value>
        public Type SelectedType
        {
            get
            {
                return this.listBox.SelectedItem as Type;
            }

            set
            {
                this.listBox.SelectedItem = value;
            }
        }

        /// <summary>
        /// Gets the name that has been entered by the user.
        /// </summary>
        /// <value>The name that has been entered by the user.</value>
        public string EnteredName
        {
            get
            {
                return this.textBoxName.Text; 
            }
        }

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Initializes a new instance of the <see cref="EventCreationDialog"/> class.
        /// </summary>
        /// <param name="dataType">
        /// States what kind of Event Type Data can be selected in the EventCreationDialog.
        /// </param>
        /// <param name="eventManager">
        /// The <see cref="EventManager"/> object will manage the 
        /// about to be created Event Data.
        /// </param>
        public EventCreationDialog( EventDataType dataType, EventManager eventManager )
        {
            Contract.Requires<ArgumentNullException>( eventManager != null );

            this.InitializeComponent();

            this.listBox.Items.Add( string.Empty );
            if( dataType == EventDataType.Event )
            {
                this.listBox.Items.AddRange( EventTypeRegister.GetEventTypes() );
            }
            else
            {
                this.listBox.Items.AddRange( EventTypeRegister.GetTriggerTypes() );
            }

            this.dataType = dataType;
            this.eventManager = eventManager;
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Called when the user clicks on the 'Ok' button.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event.
        /// </param>
        /// <param name="e">
        /// The EventArgs that contains the event data.
        /// </param>
        private void OnButtonClicked( object sender, EventArgs e )
        {
            string name = this.textBoxName.Text.Trim();

            // Validate name string:
            if( name.Length == 0 )
            {
                MessageBox.Show( 
                    EventStrings.PleaseEnterName, 
                    EventStrings.MissingInformation,
                    MessageBoxButtons.OK 
                );

                this.DialogResult = DialogResult.None;
                return;
            }

            // Validate name:
            if( dataType == EventDataType.Event )
            {
                if( this.eventManager.ContainsEvent( name ) )
                {
                    string message = string.Format( 
                        System.Globalization.CultureInfo.CurrentUICulture,
                        EventStrings.Error_ThereAlreadyExistsAnEventNamedX,
                        name 
                    );

                    MessageBox.Show( message, ErrorStrings.Error, MessageBoxButtons.OK );

                    this.DialogResult = DialogResult.None;
                    return;
                }
            }
            else
            {
                if( this.eventManager.ContainsTrigger( name ) )
                {
                    string message = string.Format( 
                        System.Globalization.CultureInfo.CurrentUICulture,
                        EventStrings.Error_ThereAlreadyExistsAnEventTriggerNamedX,
                        name 
                    );

                    MessageBox.Show( message, ErrorStrings.Error, MessageBoxButtons.OK );

                    this.DialogResult = DialogResult.None;
                    return;
                }
            }

            this.DialogResult = DialogResult.OK;
        }

        #endregion

        #region [ Fields ]

        /// <summary>
        /// The EventManager currently in-use.
        /// </summary>
        private readonly EventManager eventManager;

        /// <summary>
        /// The type of event data created by this EventCreationDialog.
        /// </summary>
        private readonly EventDataType dataType;

        #endregion
    }
}