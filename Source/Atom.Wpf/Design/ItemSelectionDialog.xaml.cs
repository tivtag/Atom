// <copyright file="ItemSelectionDialog.xaml.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Wpf.Design.ItemSelectionDialog class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Wpf.Design
{
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using Atom.Design;

    /// <summary>
    /// Implements a mechanism that allows the user to select one or more nameable items.
    /// </summary>
    public sealed partial class ItemSelectionDialog : Window, IItemSelectionDialog<IReadOnlyNameable>
    {
        /// <summary>
        /// Gets or sets the item that the user has selected.
        /// </summary>
        public IReadOnlyNameable SelectedItem
        {
            get
            {
                return this.listBoxItems.SelectedItem as IReadOnlyNameable;
            }

            set
            {
                this.listBoxItems.SelectedItem = value;
                this.listBoxItems.ScrollIntoView( value );
            }
        }

        /// <summary>
        /// Gets the items that the user has selected.
        /// </summary>
        public IEnumerable<IReadOnlyNameable> SelectedItems
        {
            get
            {
                var list = new List<IReadOnlyNameable>();

                foreach( var item in this.listBoxItems.SelectedItems )
                {
                    list.Add( item as IReadOnlyNameable );
                }

                return list;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the user should
        /// be allowed to select multiple Assets.
        /// </summary>
        /// <value>
        /// The default value is false;
        /// </value>
        public bool AllowMultipleSelection
        {
            get
            {
                return this.listBoxItems.SelectionMode == SelectionMode.Multiple;
            }

            set
            {
                this.listBoxItems.SelectionMode = value ? SelectionMode.Multiple : SelectionMode.Single;
            }
        }

        /// <summary>
        /// Initializes a new instance of the ItemSelectionDialog class.
        /// </summary>
        /// <param name="items">
        /// The items that should be selectable by the user.
        /// </param>
        public ItemSelectionDialog( IEnumerable<IReadOnlyNameable> items )
        {
            this.InitializeComponent();

            foreach( var item in items )
            {
                this.listBoxItems.Items.Add( item );
            }

            this.textBoxSearch.Focus();
        }

        /// <summary>
        /// Selects the Asset at the given index.
        /// </summary>
        /// <param name="assetIndex">
        /// The zero-based index of the asset to select.
        /// </param>
        private void SelectAsset( int assetIndex )
        {
            this.listBoxItems.SelectedIndex = assetIndex;
            this.listBoxItems.ScrollIntoView( this.listBoxItems.SelectedItem );
        }

        /// <summary>
        /// Called when the user has entered text into the textBox.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event.
        /// </param>
        /// <param name="e">
        /// The TextChangedEventArgs that contain the event data.
        /// </param>
        private void OnAssetSearchBoxTextChanged( object sender, TextChangedEventArgs e )
        {
            var textBox = sender as TextBox;
            if( textBox.Text.Length == 0 || this.listBoxItems.Items.Count == 0 )
                return;

            this.FindSearchMatches( textBox.Text );

            if( this.matches.Count == 0 )
                return;
            this.SelectAsset( this.matches[0] );
        }

        /// <summary>
        /// Finds the indices of the assets whose name match the given searchText.
        /// </summary>
        /// <param name="searchText">
        /// The asset name that should be searched.
        /// </param>
        private void FindSearchMatches( string searchText )
        {
            this.matches.Clear();
            this.matchIndex = 0;

            for( int index = 0; index < this.listBoxItems.Items.Count; ++index )
            {
                IReadOnlyNameable asset = (IReadOnlyNameable)this.listBoxItems.Items[index];

                if( asset != null )
                {
                    if( asset.Name.Contains( searchText, System.StringComparison.OrdinalIgnoreCase ) )
                    {
                        matches.Add( index );
                    }
                }
                else
                {
                    if( searchText.Length == 0 )
                    {
                        matches.Add( index );
                    }
                }
            }
        }

        /// <summary>
        /// Called when the user has entered text into the textBox.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event.
        /// </param>
        /// <param name="e">
        /// The KeyEventArgs that contain the event data.
        /// </param>
        private void OnAssetSearchBoxTextKeyDown( object sender, System.Windows.Input.KeyEventArgs e )
        {
            switch( e.Key )
            {
                case Key.Enter:
                    if( this.listBoxItems.SelectedIndex >= 0 )
                    {
                        this.DialogResult = true;
                        return;
                    }
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// Called when the user has entered text into the textBox.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event.
        /// </param>
        /// <param name="e">
        /// The KeyEventArgs that contain the event data.
        /// </param>
        private void OnAssetSearchBoxTextPreviewKeyDown( object sender, KeyEventArgs e )
        {
            switch( e.Key )
            {
                case Key.Up:
                    if( this.matches.Count == 0 )
                        return;
                    --this.matchIndex;

                    if( this.matchIndex < 0 )
                    {
                        this.matchIndex = this.matches.Count - 1;
                    }

                    this.SelectAsset( this.matches[this.matchIndex] );
                    e.Handled = true;
                    break;

                case Key.Down:
                    if( this.matches.Count == 0 )
                        return;
                    ++this.matchIndex;

                    if( this.matchIndex >= this.matches.Count )
                    {
                        this.matchIndex = 0;
                    }

                    this.SelectAsset( this.matches[this.matchIndex] );
                    e.Handled = true;
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// Called when the user has pressed any key while the listBoxItems is focused.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event.
        /// </param>
        /// <param name="e">
        /// The RoutedEventArgs that contains the event args.
        /// </param>
        private void OnListBoxItemsKeyDown( object sender, KeyEventArgs e )
        {
            if( e.Key == Key.Enter )
            {
                if( this.listBoxItems.SelectedIndex >= 0 )
                {
                    this.DialogResult = true;
                    return;
                }
            }
        }

        /// <summary>
        /// Called when the user has clicked on the Select button.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event.
        /// </param>
        /// <param name="e">
        /// The RoutedEventArgs that contains the event args.
        /// </param>
        private void OnSelectButtonClicked( object sender, System.Windows.RoutedEventArgs e )
        {
            this.DialogResult = true;
        }

        /// <summary>
        /// Called when the user has clicked on the Cancel button.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event.
        /// </param>
        /// <param name="e">
        /// The RoutedEventArgs that contains the event args.
        /// </param>
        private void OnCancelButtonClicked( object sender, System.Windows.RoutedEventArgs e )
        {
            this.DialogResult = false;
        }

        /// <summary>
        /// Called when the user has pressed any key while the window is focused.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event.
        /// </param>
        /// <param name="e">
        /// The RoutedEventArgs that contains the event args.
        /// </param>
        private void OnWindowKeyDown( object sender, KeyEventArgs e )
        {
            if( e.Key == Key.Escape )
            {
                this.DialogResult = false;
            }
        }

        /// <summary>
        /// Shows this ItemSelectionDialog.
        /// </summary>
        /// <returns>
        /// true if the user has selected an item;
        /// -or- false if the user has canceled the operatioon. 
        /// </returns>
        public new bool ShowDialog()
        {
            return base.ShowDialog() == true;
        }

        /// <summary>
        /// The zero-based index of the currently selected match in the <see cref="matches"/> list.
        /// </summary>
        private int matchIndex;

        /// <summary>
        /// The list of indices that are a match for the current search.
        /// </summary>
        private readonly List<int> matches = new List<int>();
    }
}
