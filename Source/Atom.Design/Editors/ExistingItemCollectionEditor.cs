// <copyright file="ExistingItemCollectionEditor.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Design.ExistingItemCollectionEditor{TItem} class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Design
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing.Design;
    using System.Linq;

    /// <summary>
    /// Represents an <see cref="UITypeEditor"/> that allows one to add existing
    /// objects to a collection using the property grid.
    /// </summary>
    /// <remarks>
    /// This is similiar to the System.ComponentModel.Design.CollectionEditor, but
    /// instead of adding new items the user can select existing items.
    /// </remarks>
    /// <example>
    /// To use this class simply inherit from it:
    /// <code>
    /// public class MyListEditor : Atom.Design.ExistingItemCollectionEditor{MyItemType}
    /// {
    ///    public MyListEditor()
    ///    {
    ///         protected override IItemSelectionDialog{MyItemType} CreateSelectionDialog()
    ///         {
    ///             return new MySelectionDialog();
    ///         }
    ///    }
    /// }
    /// </code>
    /// and then apply the editor on your property: 
    /// <code>
    /// [Editor( typeof( MyListEditor ), typeof( UITypeEditor ) )]
    /// </code>
    /// </example>
    /// <typeparam name="TItem">The type of item the collection contains.</typeparam>
    public abstract class ExistingItemCollectionEditor<TItem> : UITypeEditor
        where TItem : class, IReadOnlyNameable
    {
        /// <summary>
        /// Creates the <see cref="IItemSelectionDialog{TItem}"/> this ExistingItemCollectionEditor{TItem}
        /// uses internally. Must be overriden by users of this class.
        /// </summary>
        /// <returns>
        /// The <see cref="IItemSelectionDialog{TItem}"/> that should be used by this ExistingItemCollectionEditor{TItem}.
        /// </returns>
        public abstract IItemSelectionDialog<TItem> CreateSelectionDialog();

        /// <summary>
        /// Overriden. Edits the value of the specified object.
        /// </summary>
        /// <param name="context">
        /// An System.ComponentModel.ITypeDescriptorContext that can be used to gain additional context information.
        /// </param>
        /// <param name="provider">An System.IServiceProvider that this editor can use to obtain services.</param>
        /// <param name="value">The object to edit.</param>
        /// <returns>
        /// The new value of the object. If the value of the object has not changed,
        /// this should return the same object it was passed.
        /// </returns>
        public override object EditValue( ITypeDescriptorContext context, IServiceProvider provider, object value )
        {
            IExistingItemCollectionEditorFormFactory editorFormFactory = GlobalServices.GetService<IExistingItemCollectionEditorFormFactory>();

            // Create.
            using( IExistingItemCollectionEditorForm form = editorFormFactory.Build( value, this ) )
            {
                this.selectionDialog = this.CreateSelectionDialog();

                // :)))
                form.ShowDialog();

                return value;
            }
        }

        /// <summary>
        /// Overriden. Gets the editor style used by the EditValue method. 
        /// </summary>
        /// <param name="context">
        /// An System.ComponentModel.ITypeDescriptorContext that can be used to gain additional context information.
        /// </param>
        /// <returns>
        /// Returns UITypeEditorEditStyle.Modal.
        /// </returns>
        public override UITypeEditorEditStyle GetEditStyle( ITypeDescriptorContext context )
        {
            return UITypeEditorEditStyle.Modal;
        }

        /// <summary>
        /// Gets the items in the given collection.
        /// </summary>
        /// <param name="editValue">The input value to the editor.</param>
        /// <returns>The array of items.</returns>
        public virtual TItem[] GetItems( object editValue )
        {
            var input = editValue as IEnumerable<TItem>;

            if( input == null )
            {
                return new TItem[0];
            }

            return input.ToArray();
        }

        /// <summary>
        /// Sets the items in the given collection.
        /// </summary>
        /// <param name="editValue">The input value to the editor.</param>
        /// <param name="value">The array of items to set.</param>
        public virtual void SetItems( object editValue, TItem[] value )
        {
            if( editValue != null )
            {
                var list = editValue as ICollection<TItem>;

                if( list != null )
                {
                    list.Clear();
                    for( int i = 0; i < value.Length; ++i )
                    {
                        list.Add( value[i] );
                    }
                }
            }
        }

        /// <summary>
        /// The IItemSelectionDialog that allows the user of the class to plugin
        /// their custom dialog that selects an item.
        /// </summary>
        private IItemSelectionDialog<TItem> selectionDialog;
    }
}
