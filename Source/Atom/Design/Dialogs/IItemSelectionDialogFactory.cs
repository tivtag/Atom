// <copyright file="IItemSelectionDialogFactory.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Design.IItemSelectionDialogFactory interface.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Design
{
    using System.Collections.Generic;

    /// <summary>
    /// Provides a machanism for creating IItemSelectionDialog{TIem} instances.
    /// </summary>
    public interface IItemSelectionDialogFactory
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
        IItemSelectionDialog<TItem> Build<TItem>( IEnumerable<TItem> items )
            where TItem : class, IReadOnlyNameable;
    }
}
