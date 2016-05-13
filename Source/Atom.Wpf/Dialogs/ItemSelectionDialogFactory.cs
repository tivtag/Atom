// <copyright file="ItemSelectionDialogFactory.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Wpf.Dialogs.ItemSelectionDialogFactory class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Wpf.Dialogs
{
    using System.Collections.Generic;
    using Atom.Design;

    /// <summary>
    /// Implements a machanism for creating IItemSelectionDialog{TIem} instances.
    /// This class can't be inherited.
    /// </summary>
    public sealed class ItemSelectionDialogFactory : IItemSelectionDialogFactory
    {
        /// <summary>
        /// Creates a new IItemSelectionDialog{TItem} that allows the user
        /// to select the specified items.
        /// </summary>
        /// <typeparam name="TItem">
        /// The type of the nameable items the user can select in the IItemSelectionDialog{TItem}.
        /// </typeparam>
        /// <param name="items">
        /// The items the user might select.
        /// </param>
        /// <returns>
        /// The newly created IItemSelectionDialog{TItem}.
        /// </returns>
        public IItemSelectionDialog<TItem> Build<TItem>( IEnumerable<TItem> items )
            where TItem : class, IReadOnlyNameable
        {
            return new ItemSelectionDialog<TItem>( items );
        }
    }
}
