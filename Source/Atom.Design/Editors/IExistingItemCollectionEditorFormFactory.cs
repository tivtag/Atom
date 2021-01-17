// <copyright file="IExistingItemCollectionEditorForm.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Design.IExistingItemCollectionEditorForm interface.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Design
{
    /// <summary>
    /// Provides a machanism for creating IExistingItemCollectionEditorForm instances.
    /// </summary>
    public interface IExistingItemCollectionEditorFormFactory
    {
        /// <summary>
        /// Creates a new IExistingItemCollectionEditorForm.
        /// </summary>
        /// <returns>
        /// The newly created IExistingItemCollectionEditorForm.
        /// </returns>
        IExistingItemCollectionEditorForm Build<TItem>( 
            object editValue,
            ExistingItemCollectionEditor<TItem> editor
        ) where TItem : class, IReadOnlyNameable;
    }
}
