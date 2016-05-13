// <copyright file="ToggleEditor.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Design.ToggleEditor class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Design
{
    using System;
    using System.ComponentModel;
    using System.Drawing.Design;

    /// <summary>
    /// Represents an <see cref="UITypeEditor"/> that can be used as a simple toggle button
    /// for a boolean value.
    /// </summary>
    public sealed class ToggleEditor : UITypeEditor
    {        
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
            if( value is bool )
            {
                return !(bool)value;
            }

            return value;
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
        public override UITypeEditorEditStyle GetEditStyle( System.ComponentModel.ITypeDescriptorContext context )
        {
            return UITypeEditorEditStyle.Modal;
        }
    }
}
