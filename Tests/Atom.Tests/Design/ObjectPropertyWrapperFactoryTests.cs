﻿// <copyright file="ObjectPropertyWrapperFactoryTests.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Design.Tests.ObjectPropertyWrapperFactoryTests class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Design.Tests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests the usage of the <see cref="ObjectPropertyWrapperFactory"/> class.
    /// </summary>
    [TestClass]
    public sealed partial class ObjectPropertyWrapperFactoryTests
    {
        [TestMethod]
        public void RegisterWrapper_Throws_PassedNull()
        {
            // Arrange
            var factory = new ObjectPropertyWrapperFactory();

            // Act & Assert
            CustomAssert.Throws<ArgumentNullException>(
                () => {
                    factory.RegisterWrapper( null );
                }
            );
        }

        [TestMethod]
        public void GetObjectTypes_WithEmptyFactory_ReturnsEmptyArray()
        {
            // Arrange
            var factory = new ObjectPropertyWrapperFactory();

            // Act
            Type[] types = factory.GetObjectTypes();

            // Assert
            Assert.AreEqual( 0, types.Length );
        }

        [TestMethod]
        public void GetObjectTypes_WithNonEmptyFactory_ReturnsArrayContainingExpectedType()
        {
            // Arrange
            var factory = new ObjectPropertyWrapperFactory();
            factory.RegisterWrapper( new TestStringPropertyWrapper() );

            // Act
            Type[] types = factory.GetObjectTypes();

            // Assert
            CustomAssert.Contains( typeof( string ), types );
        }

        [TestMethod]
        public void RegisterWrapper_Throws_WheRegisteringSameTypeTwice()
        {
            // Arrange
            var factory = new ObjectPropertyWrapperFactory();
            factory.RegisterWrapper( new TestStringPropertyWrapper() );

            // Act & Assert
            CustomAssert.Throws<ArgumentException>(
                () => {
                    factory.RegisterWrapper( new TestStringPropertyWrapper() );
                }
            );
        }

        [TestMethod]
        public void UnregisterWrapper_Removes_AsExpected()
        {
            // Arrange
            var factory = new ObjectPropertyWrapperFactory();
            factory.RegisterWrapper( new TestStringPropertyWrapper() );

            // Act
            bool removed = factory.UnregisterWrapper( typeof( string ) );

            // Assert
            Assert.IsTrue( removed );
            CustomAssert.DoesNotContain( typeof( string ), factory.GetObjectTypes() );
        }

        [TestMethod]
        public void UnregisterWrapper_WithEmptyFactory_ReturnsFalse()
        {
            // Arrange
            var factory = new ObjectPropertyWrapperFactory();

            // Act
            bool removed = factory.UnregisterWrapper( typeof( string ) );

            // Assert
            Assert.IsFalse( removed );
        }
        
        [TestMethod]
        public void UnregisterWrapper_WhenPassedNull_Throws()
        {
            // Arrange
            var factory = new ObjectPropertyWrapperFactory();

            // Act & Assert
            CustomAssert.Throws<ArgumentNullException>(
                () => {
                    factory.UnregisterWrapper( null );
                }
            );
        }

        [TestMethod]
        public void ReceiveWrapper_WhenPassedUnknownObject_ReturnsNull()
        {
            // Arrange
            var factory = new ObjectPropertyWrapperFactory();

            // Act
            var wrapper = factory.ReceiveWrapper( "Hello World" );

            // Assert
            Assert.IsNull( wrapper );
        }

        [TestMethod]
        public void ReceiveWrapper_WhenPassedNull_Throws()
        {
            // Arrange
            var factory = new ObjectPropertyWrapperFactory();

            // Assert & Act
            CustomAssert.Throws<ArgumentNullException>(
                () => {
                    factory.ReceiveWrapper( null );
                }
            );
        }

        [TestMethod]
        public void ReceiveWrapperOrObject_WhenPassedNull_Throws()
        {
            // Arrange
            var factory = new ObjectPropertyWrapperFactory();

            // Assert & Act
            CustomAssert.Throws<ArgumentNullException>(
                () =>
                {
                    factory.ReceiveWrapperOrObject( null );
                }
            );
        }
    }
}
