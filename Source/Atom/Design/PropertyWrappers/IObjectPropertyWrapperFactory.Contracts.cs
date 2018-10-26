// <copyright file="IObjectPropertyWrapperFactory.Contracts.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Design.IObjectPropertyWrapperFactoryContracts class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Design
{
    using System;
    using Atom.Diagnostics.Contracts;

    // [ContractClassFor( typeof( IObjectPropertyWrapperFactory ) )]
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
        Justification = "Contract classes for interfaces aren't required to be documented." )] 
    internal abstract class IObjectPropertyWrapperFactoryContracts : IObjectPropertyWrapperFactory
    {
        IObjectPropertyWrapper IObjectPropertyWrapperFactory.ReceiveWrapper( object obj )
        {
            Contract.Requires<ArgumentNullException>( obj != null );
            // Contract.Ensures(
            //     !(Contract.Result<IObjectPropertyWrapper>() != null) ||
            //     (Contract.Result<IObjectPropertyWrapper>().WrappedObject == obj)
            // );

            return default( IObjectPropertyWrapper );
        }

        object IObjectPropertyWrapperFactory.ReceiveWrapperOrObject( object obj )
        {
            Contract.Requires<ArgumentNullException>( obj != null );
            // Contract.Ensures( Contract.Result<object>() != null ); 

            return default( object );
        }

        Type[] IObjectPropertyWrapperFactory.GetObjectTypes()
        {
            // Contract.Ensures( Contract.Result<Type[]>() != null );

            return default( Type[] );
        }

        void IObjectPropertyWrapperFactory.RegisterWrapper( IObjectPropertyWrapper wrapper )
        {
            Contract.Requires<ArgumentNullException>( wrapper != null );
        }

        bool IObjectPropertyWrapperFactory.UnregisterWrapper( Type type )
        {
            Contract.Requires<ArgumentNullException>( type != null );

            return default( bool );
        }
    }
}
