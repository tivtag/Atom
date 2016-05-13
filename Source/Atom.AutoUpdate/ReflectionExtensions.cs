// <copyright file="ReflectionExtensions.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>Defines the Atom.ReflectionExtensions class.</summary>
// <author>Paul Ennemoser (Tick)</author>

namespace Atom
{
    using System;
    using System.Globalization;

    /// <summary>
    /// Provides reflection related extension methods.
    /// </summary>
    /// <remarks>
    /// This class has been pulled from Atom.dll.
    /// </remarks>
    internal static class ReflectionExtensions
    {
        /// <summary>
        /// Receives the type name of the given <see cref="Type"/>,
        /// aka. "FullName, AssemblyName". Which can be used to create
        /// the given type using "Activator.CreateInstance( Type.GetType( typeName ) )"
        /// in the case that the type has a paramterless public constructor.
        /// </summary>
        /// <param name="type">
        /// The type for which to get the typename for.
        /// </param>
        /// <returns>
        /// The typename that uniquely identifies the given <paramref name="type"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="type"/> is null.
        /// </exception>
        public static string GetTypeName( this Type type )
        {
            return string.Format(
                CultureInfo.InvariantCulture,
                "{0}, {1}",
                type.FullName,
                type.Assembly.GetName().Name
            );
        }
    }
}
