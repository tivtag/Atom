// <copyright file="TypeActivator.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.TypeActivator class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom
{
    using System;

    /// <summary>
    /// Implements a mechanism that creates new objects given a type name.
    /// This class can't be inherited.
    /// </summary>
    public sealed class TypeActivator : ITypeActivator
    {
        /// <summary>
        /// Represents the singleton instance of the TypeActivator class.
        /// </summary>
        public static readonly TypeActivator Instance = new TypeActivator();

        /// <summary>
        /// Creates an instance of the type with the given typeName.
        /// </summary>
        /// <param name="typeName">
        /// The name that uniquely identifies the type to initiate.
        /// </param>
        /// <returns>
        /// The object that has been created.
        /// </returns>
        public object CreateInstance( string typeName )
        {
            Type type = Type.GetType( typeName, true );
            return Activator.CreateInstance( type );
        }
    }
}
