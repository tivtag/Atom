// <copyright file="ConfigPropertySearcher.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Configuration.ConfigPropertySearcher class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Implements a mechanism that finds all configuration properties of a type.
    /// </summary>
    public sealed class ConfigPropertySearcher : IConfigPropertySearcher
    {
        /// <summary>
        /// Finds the configuation properties of the specified <see cref="Type"/>.
        /// </summary>
        /// <param name="type">
        /// The type to query.
        /// </param>
        /// <returns>
        /// The properties that have been found.
        /// </returns>
        public IEnumerable<Tuple<PropertyInfo, IConfigPropertyAttribute>> Search( Type type )
        {
            return
                from property in type.GetProperties()
                let configAttribute = GetConfigAttribute( property )
                where configAttribute != null
                select Tuple.Create( property, configAttribute );
        }

        /// <summary>
        /// Attempts to locate an attribute that implements the <see cref="IConfigPropertyAttribute"/> by searching the
        /// specified <see cref="PropertyInfo"/>.
        /// </summary>
        /// <param name="propertyInfo">
        /// The <see cref="PropertyInfo"/> to query.
        /// </param>
        /// <returns>
        /// The IConfigPropertyAttribute that has been located; -or- if null if it does not exist.
        /// </returns>
        private static IConfigPropertyAttribute GetConfigAttribute( PropertyInfo propertyInfo )
        {
            var attributes = propertyInfo.GetCustomAttributes( true );

            return attributes.FirstOrDefault( 
                attribute => attribute.GetType().Implements( typeof( IConfigPropertyAttribute ) )
            ) as IConfigPropertyAttribute;
        }
    }
}
