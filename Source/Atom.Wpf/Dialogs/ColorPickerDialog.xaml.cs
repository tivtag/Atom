// <copyright file="ColorPickerDialog.xaml.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Wpf.Dialogs.ColorPickerDialog class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Wpf.Dialogs
{
    using System.Windows;
    using System.Windows.Media;

    /// <summary>
    /// Represents a dialog that allows the user to pick any color
    /// using the <see cref="Atom.Wpf.Controls.ColorPicker"/> control.
    /// </summary>
    public partial class ColorPickerDialog : Window
    {
        /// <summary>
        /// Gets or sets the color which has been set in the <see cref="ColorPickerDialog"/>.
        /// </summary>
        public Color SelectedColor
        {
            get
            {
                return this.colorPicker.SelectedColor;
            }

            set
            {
                this.colorPicker.SelectedColor = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorPickerDialog"/> class.
        /// </summary>
        public ColorPickerDialog()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Gets called when the user has clicked on the OK button.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The RoutedEventArgs that contains the event data.</param>
        private void OnOkButtonClicked( object sender, RoutedEventArgs e )
        {
            this.DialogResult = true;
            this.Hide();
        }

        /// <summary>
        /// Gets called when the user has clicked on the Cancel button.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The RoutedEventArgs that contains the event data.</param>
        private void OnCancelButtonClicked( object sender, RoutedEventArgs e )
        {
            this.DialogResult = false;
        }
    }
}