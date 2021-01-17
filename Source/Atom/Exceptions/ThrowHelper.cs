// <copyright file="ThrowHelper.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.ThrowHelper class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom
{
    using System;
    using System.Globalization;

    /// <summary>
    /// Defines static methods that make it easier to throw exceptions.
    /// </summary>
    public static class ThrowHelper
    {
        /// <summary>
        /// Throws an <see cref="InvalidVersionException"/> when the given <paramref name="version"/> 
        /// is not equal to the given <paramref name="expectedVersion"/>.
        /// </summary>
        /// <param name="version">
        /// The version.
        /// </param>
        /// <param name="expectedVersion">
        /// The expected version.
        /// </param>
        /// <param name="type">
        /// The type the version is related to.
        /// </param>
        /// <exception cref="InvalidVersionException">
        /// If the <paramref name="version"/> is not equal to the <paramref name="expectedVersion"/>.
        /// </exception>
        public static void InvalidVersion( int version, int expectedVersion, Type type )
        {
            if( version != expectedVersion )
            {
                throw new InvalidVersionException(
                    string.Format(
                        CultureInfo.CurrentCulture,
                        ErrorStrings.VersionXOfTypeYIsNotSupportedExpectedZ,
                        version.ToString( CultureInfo.CurrentCulture ),
                        type != null ? type.FullName : string.Empty,
                        expectedVersion.ToString( CultureInfo.CurrentCulture )
                    )
                );
            }
        }

        /// <summary>
        /// Throws an <see cref="InvalidVersionException"/> when the given <paramref name="version"/> 
        /// is not equal to the given <paramref name="expectedVersion"/>.
        /// </summary>
        /// <param name="version">
        /// The version.
        /// </param>
        /// <param name="expectedVersion">
        /// The expected version.
        /// </param>
        /// <param name="typeName">
        /// The name of the type the version is related to.
        /// </param>
        /// <exception cref="InvalidVersionException">
        /// If the <paramref name="version"/> is not equal to the <paramref name="expectedVersion"/>.
        /// </exception>
        public static void InvalidVersion( int version, int expectedVersion, string typeName )
        {
            if( version != expectedVersion )
            {
                throw new InvalidVersionException(
                    string.Format(
                        CultureInfo.CurrentCulture,
                        ErrorStrings.VersionXOfTypeYIsNotSupportedExpectedZ,
                        version.ToString( CultureInfo.CurrentCulture ),
                        typeName ?? string.Empty,
                        expectedVersion.ToString( CultureInfo.CurrentCulture )
                    )
                );
            }
        }

        /// <summary>
        /// Throws an <see cref="InvalidVersionException"/>
        /// when the given <paramref name="version"/> is not vailid.
        /// </summary>
        /// <param name="version">
        /// The version.
        /// </param>
        /// <param name="expectedVersionStart">
        /// The start range of allowed versions.
        /// </param>
        /// <param name="expectedVersionEnd">
        /// The end range of allowed versions.
        /// </param>
        /// <param name="type">
        /// The type the version is related to.
        /// </param>
        /// <exception cref="InvalidVersionException">
        /// If the <paramref name="version"/> is not greater equal to <paramref name="expectedVersionStart"/> and
        /// lesser equal to <paramref name="expectedVersionEnd"/>.
        /// </exception>
        public static void InvalidVersion(
            int version,
            int expectedVersionStart,
            int expectedVersionEnd,
            Type type )
        {
            if( version < expectedVersionStart || version > expectedVersionEnd )
            {
                throw new InvalidVersionException(
                    string.Format(
                        CultureInfo.CurrentCulture,
                        ErrorStrings.VersionXOfTypeYIsNotSupportedExpectedZToW,
                        version.ToString( CultureInfo.CurrentCulture ),
                        type != null ? type.FullName : string.Empty,
                        expectedVersionStart.ToString( CultureInfo.CurrentCulture ),
                        expectedVersionEnd.ToString( CultureInfo.CurrentCulture )
                    )
                );
            }
        }

        /// <summary>
        /// Throws an <see cref="InvalidVersionException"/>
        /// when the given <paramref name="version"/> is not valid.
        /// </summary>
        /// <param name="version">
        /// The version.
        /// </param>
        /// <param name="expectedVersionStart">
        /// The start range of allowed versions.
        /// </param>
        /// <param name="expectedVersionEnd">
        /// The end range of allowed versions.
        /// </param>
        /// <param name="typeName">
        /// The name of the type the version is related to.
        /// </param>
        /// <exception cref="InvalidVersionException">
        /// If the <paramref name="version"/> is not greater equal to <paramref name="expectedVersionStart"/> and
        /// lesser equal to <paramref name="expectedVersionEnd"/>.
        /// </exception>
        public static void InvalidVersion(
            int version,
            int expectedVersionStart,
            int expectedVersionEnd,
            string typeName )
        {
            if( version < expectedVersionStart || version > expectedVersionEnd )
            {
                throw new InvalidVersionException(
                    string.Format(
                        CultureInfo.CurrentCulture,
                        ErrorStrings.VersionXOfTypeYIsNotSupportedExpectedZToW,
                        version.ToString( CultureInfo.CurrentCulture ),
                        typeName ?? string.Empty,
                        expectedVersionStart.ToString( CultureInfo.CurrentCulture ),
                        expectedVersionEnd.ToString( CultureInfo.CurrentCulture )
                    )
                );
            }
        }

        /// <summary>
        /// Throws an exception if the specifies Component is null.
        /// </summary>
        /// <typeparam name="TComponent">
        /// The type of the component to verify.
        /// </typeparam>
        /// <param name="component">
        /// The component to verify.
        /// </param>
        public static void IfComponentNull<TComponent>( TComponent component )
            where TComponent : Atom.Components.IComponent
        {
            if( component == null )
            {
                throw new Atom.Components.ComponentNotFoundException( typeof( TComponent ) );
            }
        }

        /// <summary>
        /// Throws an <see cref="ServiceNotFoundException"/> if the specified <paramref name="service"/> is null.
        /// </summary>
        /// <typeparam name="TService">
        /// The type of the service to verify.
        /// </typeparam>
        /// <param name="service">
        /// The service to verify.
        /// </param>
        public static void IfServiceNull<TService>( TService service )
        {
            if( service == null )
            {
                throw new ServiceNotFoundException( typeof( TService ) );
            }
        }

        /// <summary>
        /// Throws an <see cref="ServiceNotFoundException"/> if the specified <paramref name="service"/> is null.
        /// </summary>
        /// <typeparam name="TService">
        /// The type of the service to verify.
        /// </typeparam>
        /// <param name="service">
        /// The service to verify.
        /// </param>
        /// <param name="message">
        /// The additional message to display.
        /// </param>
        public static void IfServiceNull<TService>( TService service, string message )
        {
            if( service == null )
            {
                throw new ServiceNotFoundException( message, typeof( TService ) );
            }
        }
    }
}
