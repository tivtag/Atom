// <copyright file="NameableObjectWrapperTests.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Design.Tests.NameableObjectWrapperTests class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Design.Tests
{
    using System;
    using Microsoft.Pex.Framework;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests the usage of the <see cref="NameableObjectWrapper"/> class.
    /// </summary>
    [TestClass]
    public sealed partial class NameableObjectWrapperTests
    {
        [TestMethod]
        public void Construction_WithNullMappingFunction_Throws()
        {
            CustomAssert.Throws<ArgumentNullException>( () => { new NameableObjectWrapper<string>( null, null ); } );
        }

        [TestMethod]
        public void GetObject_ReturnsInitialObject()
        {
            // Arrange
            var obj = new Atom.Diagnostics.Moles.SILog();
            var wrapper = new NameableObjectWrapper<Atom.Diagnostics.Moles.SILog>( obj, v => "N" );

            // Assert
            Assert.AreEqual( obj, wrapper.Object );
        }

        [PexMethod]
        public void GetName_AppliesMappingFunctionOnObjectToReceiveName( int value )
        {
            // Arrange
            var wrapper = new NameableObjectWrapper<int>( value, v => "N_" + v.ToString() );

            // Assert
            Assert.AreEqual( "N_" + value.ToString(), wrapper.Name );
        }

        [PexMethod]
        public void GetName_AppliesMappingFunction_Continiously( int value, int tries )
        {
            PexAssume.IsTrue( tries > 0 );

            // Arrange
            int index = 0;
            var wrapper = new NameableObjectWrapper<int>( 
                value, 
                v => {
                    return "N_" + v.ToString() + "_" + (index++).ToString();
                }
            );
                        
            // Assert
            string startString = "N_" + value.ToString();
            
            for( int i = 0; i < tries; ++i )
            {
                Assert.AreEqual( startString + "_" + i.ToString(), wrapper.Name );
            }
        }

        [PexMethod]
        public void GetHashCode_ReturnsSameHashCode_AsObject<T>( [PexAssumeNotNull]T obj )
        {
            // Arrange
            var wrapper = new NameableObjectWrapper<T>( obj, v => "N" );

            // Assert
            Assert.AreEqual( obj.GetHashCode(), wrapper.GetHashCode() );
        }

        [TestMethod]
        public void GetHashCode_WithNullObject_ReturnsZero()
        {
            // Arrange
            var wrapper = new NameableObjectWrapper<string>( null, v => "N" );

            // Assert
            Assert.AreEqual( 0, wrapper.GetHashCode() );
        }

        [TestMethod]
        public void Equals_Null_ReturnsFalse()
        {
            // Arrange
            var wrapper = new NameableObjectWrapper<string>( null, v => "N" );

            // Act
            bool areEqual = wrapper.Equals( null );

            // Assert
            Assert.IsFalse( areEqual );
        }

        [TestMethod]
        public void Equals_DifferntType_ReturnsFalse()
        {
            // Arrange
            var wrapper = new NameableObjectWrapper<string>( "No", v => "N" );

            // Act
            bool areEqual = wrapper.Equals( "No" );

            // Assert
            Assert.IsFalse( areEqual );
        }

        [TestMethod]
        public void Equals_WithNullObject_AgainstWrappedNullObject_ReturnsTrue()
        {
            // Arrange
            var wrapperA = new NameableObjectWrapper<string>( null, v => "A" );
            var wrapperB = new NameableObjectWrapper<string>( null, v => "B" );

            // Act
            bool areEqual = wrapperA.Equals( wrapperB );

            // Assert
            Assert.IsTrue( areEqual );
        }

        [TestMethod]
        public void Equals_Same_ReturnsTrue()
        {
            // Arrange
            var wrapper = new NameableObjectWrapper<string>( "Meow", v => "A" );

            // Act
            bool areEqual = wrapper.Equals( (object)wrapper );

            // Assert
            Assert.IsTrue( areEqual );
        }

        [TestMethod]
        public void Equals_SameObjects_ReturnsTrue()
        {
            // Arrange
            var wrapperA = new NameableObjectWrapper<string>( "Meow", v => "A" );
            var wrapperB = new NameableObjectWrapper<string>( "Meow", v => "B" );

            // Act
            bool areEqual = wrapperA.Equals( wrapperB );

            // Assert
            Assert.IsTrue( areEqual );
        }

        [TestMethod]
        public void Equals_DifferentObjects_ReturnsFalse()
        {
            // Arrange
            var wrapperA = new NameableObjectWrapper<string>( "Meow", v => "A" );
            var wrapperB = new NameableObjectWrapper<string>( "Boow", v => "B" );

            // Act
            bool areEqual = wrapperA.Equals( wrapperB );

            // Assert
            Assert.IsFalse( areEqual );
        }
    }
}
