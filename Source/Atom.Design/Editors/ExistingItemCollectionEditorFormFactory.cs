// <copyright file="ExistingItemCollectionEditorFormFactory.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Wpf.Design.ExistingItemCollectionEditorFormFactory interface.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Wpf.Design
{
    using Atom.Design;

    /// <summary>
    /// Implements IExistingItemCollectionEditorFormFactory creating a
    /// WindowsForms-Implementation of IExistingItemCollectionEditorForm.
    /// </summary>
    public sealed class ExistingItemCollectionEditorFormFactory : IExistingItemCollectionEditorFormFactory
    {
        /// <summary>
        /// Creates a new IExistingItemCollectionEditorForm.
        /// </summary>
        /// <returns>
        /// The newly created IExistingItemCollectionEditorForm.
        /// </returns>
        public IExistingItemCollectionEditorForm Build<TItem>( 
            object editValue, 
            ExistingItemCollectionEditor<TItem> editor ) 
            where TItem : class, IReadOnlyNameable
        {
            return new ExistingItemCollectionEditorForm<TItem>( editValue, editor );
        }
    }
}
