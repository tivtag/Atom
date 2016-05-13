// <copyright file="ThrowHelperTests.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>Defines the Atom.Tests.ThrowHelperTests class.</summary>
// <author>Paul Ennemoser (Tick)</author>

namespace Atom.Tests
{
    using System;
    using Microsoft.Pex.Framework;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests the usage of the <see cref="ThrowHelper"/> class.
    /// </summary>
    [TestClass]
    public sealed partial class ThrowHelperTests
    {
        [PexMethod]
        public void InvalidVersion_Throws_WhenExpected( int version, int expectedVersion, string typeName )
        {
            // Act
            bool hasThrown = TestHelper.HasThrown<InvalidVersionException>(
                () => ThrowHelper.InvalidVersion( version, expectedVersion, typeName )
            );
            
            // Assert
            Assert.AreEqual( hasThrown, version != expectedVersion );
        }

        [PexMethod]
        public void InvalidVersion_Throws_WhenExpected( int version, int expectedVersion, Type type )
        {
            // Act
            bool hasThrown = TestHelper.HasThrown<InvalidVersionException>(
                () => ThrowHelper.InvalidVersion( version, expectedVersion, type )
            );

            // Assert
            Assert.AreEqual( version != expectedVersion, hasThrown );
        }

        [PexMethod]
        public void InvalidVersion_WithRange_Throws_WhenExpected( 
            int version,
            int expectedVersionStart,
            int expectedVersionEnd,
            string typeName )
        {
            // Assume
            PexAssume.IsTrue( expectedVersionEnd >= expectedVersionStart );

            // Act
            bool hasThrown = TestHelper.HasThrown<InvalidVersionException>(
                () => ThrowHelper.InvalidVersion( version, expectedVersionStart, expectedVersionEnd, typeName )
            );

            // Assert
            Assert.AreEqual( (version < expectedVersionStart || version > expectedVersionEnd),  hasThrown );
        }

        [PexMethod]
        public void InvalidVersion_WithRange_Throws_WhenExpected(
            int version,
            int expectedVersionStart,
            int expectedVersionEnd,
            Type type )
        {
            // Assume
            PexAssume.IsTrue( expectedVersionEnd >= expectedVersionStart );

            // Act
            bool hasThrown = TestHelper.HasThrown<InvalidVersionException>(
                () => ThrowHelper.InvalidVersion( version, expectedVersionStart, expectedVersionEnd, type )
            );

            // Assert
            Assert.AreEqual( (version < expectedVersionStart || version > expectedVersionEnd), hasThrown );
        }

        [PexMethod]
        public void InvalidVersion_WithInvalidRange_Throws(
            int version,
            int expectedVersionStart,
            int expectedVersionEnd,
            Type type )
        {
            PexAssume.IsTrue( version < expectedVersionStart || version > expectedVersionEnd );

            InvalidVersion_WithRange_Throws_WhenExpected( version, expectedVersionStart, expectedVersionEnd, type );
        }

        [PexMethod]
        public void InvalidVersion_WithInvalidRange_Throws(
            int version,
            int expectedVersionStart,
            int expectedVersionEnd )
        {
            InvalidVersion_WithInvalidRange_Throws( version, expectedVersionStart, expectedVersionEnd, typeof(Int32) );
        }

        [PexMethod]
        public void InvalidVersion_WithInvalidRange_Throws(
            int version,
            int expectedVersionStart,
            int expectedVersionEnd,
            string typeName )
        {
            PexAssume.IsTrue( version < expectedVersionStart || version > expectedVersionEnd );

            InvalidVersion_WithRange_Throws_WhenExpected( version, expectedVersionStart, expectedVersionEnd, typeName );
        }

        [TestMethod]
        public void IfNullComponent_WhenPassedNull_ThrowsExepectedException()
        {
            var exception = CustomAssert.Throws<Atom.Components.ComponentNotFoundException>(
                () => {
                    ThrowHelper.IfComponentNull<Atom.Components.Tests.TestComponent>( null );
                }
            );

            CustomAssert.Contains( "Atom.Components.Tests.TestComponent", exception.Message );
        }
        
        [TestMethod]
        public void IfNullComponent_WhenPassedValidComponent_DoesNotThrow()
        {
            CustomAssert.DoesNotThrow(
                () => {
                    ThrowHelper.IfComponentNull<Atom.Components.Tests.MasterComponent>( new Atom.Components.Tests.MasterComponent() );
                }
            );
        }
        
        [TestMethod]
        public void IfNullService_WhenPassedNull_ThrowsExepectedException()
        {
            var exception = CustomAssert.Throws<ServiceNotFoundException>(
                () => {
                    ThrowHelper.IfServiceNull<String>( null );
                }
            );

            CustomAssert.Contains( "System.String", exception.Message );
            Assert.AreEqual( typeof(String), exception.ServiceType );
        }

        [TestMethod]
        public void IfNullService_WhenPassedNullAndMessage_ThrowsExepectedExceptionWithMessage()
        {
            const string Message = "Hello, boo!";

            var exception = CustomAssert.Throws<ServiceNotFoundException>(
                () => {
                    ThrowHelper.IfServiceNull<String>( null, Message );
                }
            );

            Assert.AreEqual( Message, exception.Message );
            Assert.AreEqual( typeof( String ), exception.ServiceType );
        }

        [TestMethod]
        public void IfNullService_WhenPassedValidService_DoesNotThrow()
        {
            CustomAssert.DoesNotThrow(
                () => {
                    ThrowHelper.IfServiceNull<String>( string.Empty );
                }
            );
        }

        [TestMethod]
        public void IfNullService_WhenPassedValidServiceWithMessage_DoesNotThrow()
        {
            CustomAssert.DoesNotThrow(
                () => {
                    ThrowHelper.IfServiceNull<String>( string.Empty, string.Empty );
                }
            );
        }
    }
}
