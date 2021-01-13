// <copyright file="BaseObjectPropertyWrapperTests.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>Defines the Atom.Design.Tests.BaseObjectPropertyWrapperTests class.</summary>
// <author>Paul Ennemoser (Tick)</author>

namespace Atom.Design.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests the usage of the <see cref="BaseObjectPropertyWrapper"/> class.
    /// </summary>
    [TestClass]
    public sealed partial class BaseObjectPropertyWrapperTests
    {
        [TestMethod]
        public void WrappedObject_IsNullByDefault()
        {
            // Arrange
            var wrapper = new TestStringPropertyWrapper();

            // Assert
            Assert.IsNull( wrapper.WrappedObject );
        }

        [TestMethod]
        public void SetWrappedObject_RaisesPropertyChanged()
        {
            // Arrange
            var wrapper = new TestStringPropertyWrapper();

            bool notified = false;
            wrapper.PropertyChanged += ( sender, e ) =>  {
                if( e.PropertyName == "WrappedObject" )
                {
                    notified = true;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            };

            // Act
            wrapper.WrappedObject = "";

            // Assert
            Assert.IsTrue( notified );
        }

        [TestMethod]
        public void WrappedType_ReturnsExpectedType()
        {
            // Arrange
            var wrapper = new TestStringPropertyWrapper();

            // Assert
            Assert.AreEqual( typeof( string ), wrapper.WrappedType );
        }

        [TestMethod]
        public void Clone_ReturnsNoneNull()
        {
            // Arrange
            var wrapper = new TestStringPropertyWrapper();
            
            // Act
            var clone = wrapper.Clone();

            // Assert
            Assert.IsNotNull( clone );
        }

        [TestMethod]
        public void Clone_ReturnsWrapper_WithNullWrappedObject()
        {
            // Arrange
            var wrapper = new TestStringPropertyWrapper() { WrappedObject = "TestObject" };

            // Act
            var clone = wrapper.Clone();

            // Assert
            Assert.IsNull( clone.WrappedObject );
        }
    }
}
