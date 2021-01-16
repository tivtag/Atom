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

    public interface IExistingItemCollectionEditorForm : IDisposable
    {
        void ShowDialog();
    }
}
