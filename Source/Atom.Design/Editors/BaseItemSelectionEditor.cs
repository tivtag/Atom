// <copyright file="BaseItemSelectionEditor.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Design.BaseItemSelectionEditor{TItem} class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Design
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using Atom;

    /// <summary>
    /// Represents an <see cref="System.Drawing.Design.UITypeEditor"/> that allows the user to select an item.
    /// </summary>
    /// <typeparam name="TItem">
    /// The type of item that can be selected in the BaseItemSelectionEditor{TItem}.
    /// </typeparam>
    public abstract class BaseItemSelectionEditor<TItem> : System.Drawing.Design.UITypeEditor
        where TItem : class, IReadOnlyNameable
    {
        /// <summary>
        /// Gets the items that can be selected in this BaseItemSelectionEditor{TItem}.
        /// </summary>
        /// <returns>
        /// The list of items.
        /// </returns>
        protected abstract IEnumerable<TItem> GetSelectableItems();

        /// <summary>
        /// Edits the value of the specified object using the editor style indicated
        /// by the System.Drawing.Design.UITypeEditor.GetEditStyle() method.
        /// </summary>
        /// <param name="context">
        /// An System.ComponentModel.ITypeDescriptorContext that can be used to gain
        /// additional context information.
        /// </param>
        /// <param name="provider">
        ///  An System.IServiceProvider that this editor can use to obtain services.
        ///  </param>
        /// <param name="value">  
        /// The object to edit.
        /// </param>
        /// <returns>
        /// The new value of the object.
        /// </returns>
        public override object EditValue( ITypeDescriptorContext context, IServiceProvider provider, object value )
        {
            Maybe<TItem> maybe = this.ShowSelectionDialog( GetItemFromValue( value ) );

            if( maybe.HasValue )
            {
                return this.GetFinalValue( maybe.Value );
            }
            else
            {
                return value;
            }
        }

        /// <summary>
        /// Allows the user to select an item.
        /// </summary>
        /// <param name="value">
        /// The item the user has initially selected in the dialog.
        /// </param>
        /// <returns>
        /// The item the user has selected.
        /// </returns>
        protected Maybe<TItem> ShowSelectionDialog( TItem value )
        {
            IItemSelectionDialogFactory dialogFactory = GlobalServices.GetService<IItemSelectionDialogFactory>();
            IItemSelectionDialog<TItem> dialog = dialogFactory.Build<TItem>( this.GetSelectableItems() );
            dialog.SelectedItem = value;

            if( dialog.ShowDialog() )
            {
                return new Maybe<TItem>( dialog.SelectedItem );
            }

            return Maybe<TItem>.None;
        }

        /// <summary>
        /// Gets the TItem that is associated with the given <paramref name="value"/>.
        /// </summary>
        /// <param name="value">
        /// The original value of the property.
        /// </param>
        /// <returns>
        /// The object that will be pre-selected in the dialog.
        /// </returns>
        protected virtual TItem GetItemFromValue( object value )
        {
            return value as TItem;
        }

        /// <summary>
        /// Gets the final value that is returned by this BaseItemSelectionEditor{TItem}.
        /// </summary>
        /// <param name="selectedItem">
        /// The item the user has selected.
        /// </param>
        /// <returns>
        /// The object that is returned from this BaseItemSelectionEditor{TItem}
        /// back to the property.
        /// </returns>
        protected virtual object GetFinalValue( TItem selectedItem )
        {
            return selectedItem;
        }

        /// <summary>
        /// Gets the editor style used by the 
        /// System.Drawing.Design.UITypeEditor.EditValue(System.IServiceProvider,System.Object)
        /// method.
        /// </summary>
        /// <param name="context">
        /// An System.ComponentModel.ITypeDescriptorContext that can be used to gain
        /// additional context information.
        /// </param>
        /// <returns>
        /// Returns UITypeEditorEditStyle.Modal.
        /// </returns>
        public override System.Drawing.Design.UITypeEditorEditStyle GetEditStyle( ITypeDescriptorContext context )
        {
            return System.Drawing.Design.UITypeEditorEditStyle.Modal;
        }
    }
}
