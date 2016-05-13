// <copyright file="OverwritePromptDialog.xaml.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Wpf.Dialogs.OverwritePromptDialog class and OverwritePromptResult enumeration.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Wpf.Dialogs
{
    using System.Windows;

    /// <summary>
    /// Enumerates the results of an enumeration prompt.
    /// </summary>
    public enum OverwritePromptResult
    {
        /// <summary>
        /// The user wishes to overwrite the file/object/etc.
        /// </summary>
        Overwrite,

        /// <summary>
        /// The user wishes to overwrite all files/objects/etc.
        /// </summary>
        OverwriteAll,

        /// <summary>
        /// The user wishes to not overwrite the file/object/etc.
        /// </summary>
        Cancel
    }

    /// <summary>
    /// Implements a simple dialog that promts the user
    /// to overwrite a file/object/etc.
    /// </summary>
    public partial class OverwritePromptDialog : Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OverwritePromptDialog"/> class.
        /// </summary>
        public OverwritePromptDialog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="OverwritePromptDialog"/>
        /// allows the user to choose whether he wants to overwrite all.
        /// </summary>
        /// <value>
        /// The default value is 'true'.
        /// </value>
        public bool AskOverwriteAll
        {
            get
            {
                return buttonOverwriteAll.IsEnabled;
            }

            set
            {
                if( value )
                {
                    buttonOverwriteAll.Visibility = Visibility.Visible;
                    buttonOverwriteAll.IsEnabled  = true;
                }
                else
                {
                    buttonOverwriteAll.Visibility = Visibility.Hidden;
                    buttonOverwriteAll.IsEnabled  = false;
                }
            }
        }

        /// <summary>
        /// Gets or sets the string that is displayed to get the user an idea
        /// of what is going to be overwritten.
        /// </summary>
        public string InfoString
        {
            get { return textBoxInfo.Text; }
            set { textBoxInfo.Text = value; }
        }

        /// <summary>
        /// Opens this dialog and only returns when the
        /// newly opened dialog window is closed.
        /// </summary>
        /// <returns>
        /// The result of the dialog.
        /// </returns>
        public new OverwritePromptResult ShowDialog()
        {
            base.ShowDialog();
            return result;
        }

        /// <summary>
        /// Gets called when the user clicks on the Overwrite button.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The RoutedEventArgs that contains the event data.</param>
        private void Overwrite_Click( object sender, RoutedEventArgs e )
        {
            result = OverwritePromptResult.Overwrite;
            this.DialogResult = true;
        }

        /// <summary>
        /// Gets called when the user clicks on the OverwriteAll button.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The RoutedEventArgs that contains the event data.</param>
        private void OverwriteAll_Click( object sender, RoutedEventArgs e )
        {
            result = OverwritePromptResult.OverwriteAll;
            this.DialogResult = true;
        }

        /// <summary>
        /// Gets called when the user clicks on the Cancel button.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The RoutedEventArgs that contains the event data.</param>
        private void Cancel_Click( object sender, RoutedEventArgs e )
        {
            result = OverwritePromptResult.Cancel;
            this.DialogResult = false;
        }

        /// <summary>
        /// Stores the result of the dialog.
        /// </summary>
        private OverwritePromptResult result;
    }
}
