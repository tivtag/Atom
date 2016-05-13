// <copyright file="IObjectPropertyWrapper.Contracts.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Design.IObjectPropertyWrapperContracts class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Design
{
    using System;
    using System.Diagnostics.Contracts;

    [ContractClassFor( typeof( IObjectPropertyWrapper ) )]
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
        Justification = "Contract classes for interfaces aren't required to be documented." )] 
    internal abstract class IObjectPropertyWrapperContracts : IObjectPropertyWrapper
    {
        object IObjectPropertyWrapper.WrappedObject
        {
            get
            {
                return default( IObjectPropertyWrapper );
            }

            set
            {
                IObjectPropertyWrapper @this = this;

                Contract.Requires<ArgumentException>(
                    !(value != null) || @this.WrappedType.IsAssignableFrom( value.GetType() ),
                    ErrorStrings.TypeNotSupportedByPropertyWrapper
                );
            }
        }

        Type IObjectPropertyWrapper.WrappedType
        {
            get
            {
                Contract.Ensures( Contract.Result<Type>() != null );

                return default( Type );
            }
        }

        #region ICloneable Members

        object ICloneable.Clone()
        {
            return default( object );
        }

        #endregion

        #region INotifyPropertyChanged Members

        event System.ComponentModel.PropertyChangedEventHandler System.ComponentModel.INotifyPropertyChanged.PropertyChanged
        {
            add
            {
            }

            remove
            {
            }
        }

        #endregion
    }
}
