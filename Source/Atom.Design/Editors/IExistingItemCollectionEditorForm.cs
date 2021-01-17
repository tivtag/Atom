// <copyright file="IExistingItemCollectionEditorForm.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Design.IExistingItemCollectionEditorForm interface.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Design
{
    using System;

    /// <summary>
    /// Represents a form that gets displayed when the user wants to edit a collection
    /// at design time.
    /// </summary>
    public interface IExistingItemCollectionEditorForm : IDisposable
    {
        /// <summary>
        /// Shows the dialog.
        /// </summary>
        void ShowDialog();
    }
}
