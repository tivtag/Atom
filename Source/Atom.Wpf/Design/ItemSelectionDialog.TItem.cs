// <copyright file="ItemSelectionDialog.TItem.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Wpf.Design.ItemSelectionDialog{TItem} class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Wpf.Design
{
    using System.Collections.Generic;
    using System.Linq;
    using Atom.Design;

    /// <summary>
    /// Implements a mechanism that allows the user to select one or more nameable items of type <typeparamref name="TItem"/>.
    /// This class can't be inherited.
    /// </summary>
    /// <typeparam name="TItem">
    /// The type of the nameable items the user can select in the IItemSelectionDialog{TItem}.
    /// </typeparam>
    public sealed class ItemSelectionDialog<TItem> : IItemSelectionDialog<TItem>
        where TItem : class, IReadOnlyNameable
    {
        /// <summary>
        /// Gets or sets the item that the user has selected.
        /// </summary>
        public TItem SelectedItem
        {
            get
            {
                return (TItem)this.dialog.SelectedItem;
            }

            set
            {
                this.dialog.SelectedItem = value;
            }
        }

        /// <summary>
        /// Gets the items that the user has selected.
        /// </summary>
        public IEnumerable<TItem> SelectedItems 
        {
            get
            {
                return this.dialog.SelectedItems.Cast<TItem>();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the user should
        /// be allowed to select multiple items.
        /// </summary>
        /// <value>
        /// The default value is false;
        /// </value>
        public bool AllowMultipleSelection 
        {
            get
            {
                return this.dialog.AllowMultipleSelection;
            }

            set
            {
                this.dialog.AllowMultipleSelection = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the ItemSelectionDialog class.
        /// </summary>
        /// <param name="items">
        /// The items that should be selectable by the user.
        /// </param>
        public ItemSelectionDialog( IEnumerable<TItem> items )
        {
            this.dialog = new ItemSelectionDialog( items );
        }

        /// <summary>
        /// Shows this IItemSelectionDialog{TItem}
        /// </summary>
        /// <returns>
        /// true if the user has selected an item;
        /// -or- false if the user has canceled the operatioon. 
        /// </returns>
        public bool ShowDialog()
        {
            return this.dialog.ShowDialog();
        }

        /// <summary>
        /// The actual WPF dialog that is used to drive the dialog.
        /// </summary>
        private readonly ItemSelectionDialog dialog;
    }
}
