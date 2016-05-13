﻿// <copyright file="TypeActivatorTests.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Tests.TypeActivatorTests class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Tests
{
    using System;
    using Microsoft.Pex.Framework;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests the usage of the <see cref="TypeActivator"/> class.
    /// </summary>
    [TestClass]
    public sealed partial class TypeActivatorTests
    {
        [TestMethod]
        public void CreateInstance_IfPassedNullTypeName_Throws()
        {
            // Arrange
            var typeActivator = new TypeActivator();

            // Act & Assert
            CustomAssert.Throws<ArgumentNullException>( () => {
                typeActivator.CreateInstance( null );
            });
        }

        [TestMethod]
        public void CreateInstance_IfPassedEmptyTypeName_Throws()
        {
            // Arrange
            var typeActivator = new TypeActivator();

            // Act & Assert
            CustomAssert.Throws<TypeLoadException>( () => {
                typeActivator.CreateInstance( "" );
            } );
        }

        [TestMethod]
        public void CreateInstance_IfPassedArabitaryTypeName_Throws()
        {
            // Arrange
            var typeActivator = new TypeActivator();

            // Act & Assert
            CustomAssert.Throws<TypeLoadException>( () => {
                typeActivator.CreateInstance( "HelloNu" );
            } );
        }

        [PexMethod]
        public void CreateInstance_IfPassedValidTypeName_CreatesObject<T>()
        {
            // Arrange
            var typeActivator = new TypeActivator();
            var typeName = typeof( T ).GetTypeName();

            // Act
            T obj = (T)typeActivator.CreateInstance( typeName );

            // Assert
            Assert.IsNotNull( obj );
        }
    }
}
