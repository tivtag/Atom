// <copyright file="IItemSelectionDialog.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Xna.IItemSelectionDialog{TItem} interface.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Design
{
    using System.Collections.Generic;

    /// <summary>
    /// Provides a mechanism that allows the user to select one or more nameable items of type <typeparamref name="TItem"/>.
    /// </summary>
    /// <typeparam name="TItem">
    /// The type of the nameable items the user can select in the IItemSelectionDialog{TItem}.
    /// </typeparam>
    public interface IItemSelectionDialog<TItem>
        where TItem : class, IReadOnlyNameable
    {
        /// <summary>
        /// Gets or sets the item that the user has selected.
        /// </summary>
        TItem SelectedItem { get; set; }

        /// <summary>
        /// Gets the items that the user has selected.
        /// </summary>
        IEnumerable<TItem> SelectedItems { get; }

        /// <summary>
        /// Gets or sets a value indicating whether the user should
        /// be allowed to select multiple items.
        /// </summary>
        /// <value>
        /// The default value is false;
        /// </value>
        bool AllowMultipleSelection { get; set; }

        /// <summary>
        /// Shows this IItemSelectionDialog{TItem}
        /// </summary>
        /// <returns>
        /// true if the user has selected an item;
        /// -or- false if the user has canceled the operatioon. 
        /// </returns>
        bool ShowDialog();
    }
}
