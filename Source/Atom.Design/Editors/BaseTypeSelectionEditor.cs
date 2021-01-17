// <copyright file="BaseTypeSelectionEditor.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Design.BaseTypeSelectionEditor class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Design
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Represents an <see cref="System.Drawing.Design.UITypeEditor"/> that allows the user to select a <see cref="Type"/>.
    /// </summary>
    public abstract class BaseTypeSelectionEditor : BaseItemSelectionEditor<NameableObjectWrapper<Type>>
    {
        /// <summary>
        /// Gets or sets a value indicating whether the full type name of the objects
        /// to select should be shown or only the normal short name should be shown.
        /// </summary>
        /// <value>
        /// The default value is false.
        /// </value>
        public bool ShowFullTypeName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the types that the user can select in this BaseTypeSelectionEditor.
        /// </summary>
        /// <returns>
        /// The types the user can select.
        /// </returns>
        protected abstract IEnumerable<Type> GetTypes();

        /// <summary>
        /// Gets the items that can be selected in this BaseItemSelectionEditor{TItem}.
        /// </summary>
        /// <returns>
        /// The list of items.
        /// </returns>
        protected sealed override System.Collections.Generic.IEnumerable<NameableObjectWrapper<Type>> GetSelectableItems()
        {
            return this.GetTypes().Select( type => this.CreateTypeWrapper( type ) );
        }

        /// <summary>
        /// Creates the NameableObjectWrapper{Type} for the given <paramref name="type"/>.
        /// </summary>
        /// <param name="type">
        /// The type to wrap.
        /// </param>
        /// <returns>
        /// The wrapper that has been created.
        /// </returns>
        protected virtual NameableObjectWrapper<Type> CreateTypeWrapper( Type type )
        {
            if( this.ShowFullTypeName  )
            {
                return new NameableObjectWrapper<Type>( type, MapTypeToTypeName );
            }
            else
            {
                return new NameableObjectWrapper<Type>( type, MapTypeToName );
            }
        }

        /// <summary>
        /// Gets the NameableObjectWrapper{Type} that is associated with the given <paramref name="value"/>.
        /// </summary>
        /// <param name="value">
        /// The original value of the property.
        /// </param>
        /// <returns>
        /// The object that will be pre-selected in the dialog.
        /// </returns>
        protected override NameableObjectWrapper<Type> GetItemFromValue( object value )
        {
            if( value == null )
                return null;

            Type type = value as Type;
            
            if( type == null )
            {
                type = value.GetType();
            }
            
            return CreateTypeWrapper( type );
        }

        /// <summary>
        /// Maps the given Type onto its typename.
        /// </summary>
        /// <param name="type">
        /// The input type.
        /// </param>
        /// <returns>
        /// The respective type name.
        /// </returns>
        private static string MapTypeToTypeName( Type type )
        {
            if( type != null )
            {
                return type.GetTypeName();
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Maps the given Type onto its short name.
        /// </summary>
        /// <param name="type">
        /// The input type.
        /// </param>
        /// <returns>
        /// The respective type name.
        /// </returns>
        private static string MapTypeToName( Type type )
        {
            if( type != null )
            {
                return type.Name;
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
