// <copyright file="ReflectionExtensions.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.ReflectionExtensions class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Provides reflection related extension methods.
    /// </summary>
    public static class ReflectionExtensions
    {    
        /// <summary>
        /// Attempts to get the value of a public constant or static field of type <typeparamref name="TConstant"/>
        /// on the specified class that has the specified <paramref name="constantName"/>.
        /// </summary>
        /// <typeparam name="TConstant">
        /// The type of the constant.
        /// </typeparam>
        /// <param name="type">
        /// The type of the class that contains the definition of the constant.
        /// </param>
        /// <param name="constantName">
        /// The name of the constant to locate.
        /// </param>
        /// <returns>
        /// The value of teh constant or nothing.
        /// </returns>
        public static Maybe<TConstant> GetConstantValue<TConstant>( this Type type, string constantName )
        {
            Contract.Requires<ArgumentNullException>( type != null );
            Contract.Requires<ArgumentNullException>( constantName != null );

            var constants = 
                from field in type.GetFields( BindingFlags.Public | BindingFlags.Static )
                select field;

            var constant = constants.FirstOrDefault( c => c.Name.Equals( constantName, StringComparison.Ordinal ) );

            if( constant == null )
            {
                return Maybe.None<TConstant>();
            }

            return Maybe.Some( (TConstant)constant.GetValue( null ) );
        }

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
            Contract.Requires<ArgumentNullException>( type != null );

            return string.Format(
                CultureInfo.InvariantCulture,
                "{0}, {1}",
                type.FullName,
                type.Assembly.GetName().Name
            );
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="Type"/> implements another other <see cref="Type"/>.
        /// </summary>
        /// <param name="thisType">The type that is supposed to implement the other type.</param>
        /// <param name="type">The type that is supposed to be implemented.</param>
        /// <returns>
        /// Returns true if <paramref name="thisType"/> implements the given <paramref name="type"/>;
        /// otherwise false.
        /// </returns>
        public static bool Implements( this Type thisType, Type type )
        {
            return type != null && type.IsAssignableFrom( thisType );
        }

        /// <summary>
        /// Tries the find the best matching constructor of the <see cref="Type"/>
        /// that match the specified requirements.
        /// </summary>
        /// <remarks>
        /// Remember that the best matching constructor may still have an invalid signature.
        /// </remarks>
        /// <param name="type">
        /// The type to investigate.
        /// </param>
        /// <param name="parameters">
        /// The paramters the constructor should have.
        /// </param>
        /// <returns>
        /// The best matching constructor.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// If the <see cref="Type"/> has no public constructor
        /// with as many parameters as required.
        /// </exception>
        public static ConstructorInfo GetBestMatchingConstructor( this Type type, params object[] parameters )
        {
            Contract.Requires<ArgumentNullException>( type != null );

            var constructors = GetConstructors( type );
            var countMatches = GetConstructorsWithMatchingParameterCount( type, parameters, constructors );
            var bestMatch    = GetBestMatchingConstructor( countMatches, parameters );

            return bestMatch;
        }

        /// <summary>
        /// Gets the constructors associated with the given Type.
        /// </summary>
        /// <param name="type">
        /// The input type.
        /// </param>
        /// <returns>
        /// The public constructors of the given Type.
        /// </returns>
        private static ConstructorInfo[] GetConstructors( Type type )
        {
            Contract.Ensures( Contract.Result<ConstructorInfo[]>() != null );
            Contract.Ensures( Contract.Result<ConstructorInfo[]>().Length >= 1 );

            ConstructorInfo[] constructors = type.GetConstructors();

            if( constructors.Length == 0 )
            {
                throw new ArgumentException(
                    string.Format(
                        CultureInfo.CurrentCulture,
                        ErrorStrings.TypeHasNoPublicConstructors,
                        type.ToString()
                    ),
                    "type"
                );
            }

            return constructors;
        }

        /// <summary>
        /// Gets the public constructors of the given Type that have the required number of paramters.
        /// </summary>
        /// <param name="type">
        /// The input type.
        /// </param>
        /// <param name="parameters">
        /// The parameters passed to the constructor.
        /// </param>
        /// <param name="constructors">
        /// The array of public constructors associated with the given input type.
        /// </param>
        /// <exception cref="ArgumentException">
        /// If the <see cref="Type"/> has no public constructor
        /// with as many parameters as required.
        /// </exception>
        /// <returns>
        /// The list of all matching constructors.
        /// </returns>
        private static List<ConstructorInfo> GetConstructorsWithMatchingParameterCount( 
            Type type, 
            object[] parameters, 
            ConstructorInfo[] constructors )
        {
            Contract.Ensures( Contract.Result<List<ConstructorInfo>>() != null );
            Contract.Ensures( Contract.Result<List<ConstructorInfo>>().Count >= 1 );

            int parameterCount = parameters != null ? parameters.Length : 0;
            var countMatches = new System.Collections.Generic.List<ConstructorInfo>();

            for( int i = 0; i < constructors.Length; ++i )
            {
                if( constructors[i].GetParameters().Length == parameterCount )
                {
                    countMatches.Add( constructors[i] );
                }
            }

            if( countMatches.Count == 0 )
            {
                throw new ArgumentException(
                    string.Format(
                        CultureInfo.CurrentCulture,
                        ErrorStrings.TypeHasNoPublicConstructorWithParameters,
                        type.ToString(),
                        parameterCount.ToString( System.Globalization.CultureInfo.CurrentCulture )
                    ),
                    "type"
                );
            }

            return countMatches;
        }

        /// <summary>
        /// Gets the constructor that matches the given parameters the best.
        /// </summary>
        /// <param name="countMatches">
        /// The list of constructors that have a matching number of parameters.
        /// </param>
        /// <param name="parameters">
        /// The parameters to the constructor.
        /// </param>
        /// <returns>
        /// The best matching constructor.
        /// </returns>
        private static ConstructorInfo GetBestMatchingConstructor(
            List<ConstructorInfo> countMatches,
            object[] parameters )
        {
            Contract.Requires( countMatches != null );
            Contract.Requires( 0 < countMatches.Count  );

            int indexOfBest = 0;
            int currentBestMatches = 0;

            for( int i = 0; i < countMatches.Count; ++i )
            {
                ParameterInfo[] paramInfos = countMatches[i].GetParameters();

                int matchingParameters = 0;
                for( int j = 0; j < paramInfos.Length; ++j )
                {
                    if( paramInfos[j].ParameterType.Equals( parameters[j].GetType() ) )
                    {
                        ++matchingParameters;
                    }
                }

                if( matchingParameters > currentBestMatches )
                {
                    currentBestMatches = matchingParameters;
                    indexOfBest = i;
                }
            }

            return countMatches[indexOfBest];
        }
    }
}
