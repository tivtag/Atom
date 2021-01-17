// <copyright file="BaseItemCreationEditor.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Design.BaseItemCreationEditor class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Design
{
    using System;
    using System.ComponentModel;

    /// <summary>
    /// Represents an <see cref="System.Drawing.Design.UITypeEditor"/> that allows the user to create an object.
    /// </summary>
    public abstract class BaseItemCreationEditor : BaseTypeSelectionEditor
    {
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
            Type type = value != null ? value.GetType() : null;
            Maybe<NameableObjectWrapper<Type>> maybeSelectedType = base.ShowSelectionDialog( 
                CreateTypeWrapper( type  )
            );

            if( maybeSelectedType.HasValue )
            {
                Type selectedType = maybeSelectedType.Value.Object;     
                if( type == selectedType )
                {
                    return value;
                }

                if( selectedType != null )
                {
                    object obj = this.CreateObject( selectedType );
                    this.SetupCreatedObject( obj );

                    return obj;
                }
                else
                {
                    return null;                    
                }
            }

            return value;
        }

        /// <summary>
        /// Creates an instance of the object of the given selected <see cref="Type"/>.
        /// </summary>
        /// <param name="type">
        /// The type that has been selected by the user.
        /// </param>
        /// <returns>
        /// The newly created object.
        /// </returns>
        protected virtual object CreateObject( Type type )
        {
            return Activator.CreateInstance( type );
        }

        /// <summary>
        /// Called after the given object has been created.
        /// </summary>
        /// <param name="obj">
        /// The object which has been created.
        /// </param>
        protected virtual void SetupCreatedObject( object obj )
        {
        }
    }
}
