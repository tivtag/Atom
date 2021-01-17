// <copyright file="PropertyPropertyDescriptorTests.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>Defines the Atom.Design.Descriptors.Tests.PropertyPropertyDescriptorTests class.</summary>
// <author>Paul Ennemoser</author>

namespace Atom.Design.Descriptors.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests the usage of the <see cref="PropertyPropertyDescriptor"/> class.
    /// </summary>
    [TestClass]
    public sealed class PropertyPropertyDescriptorTests
    {
        private sealed class TestPropertyObject
        {
            public string PublicProperty
            {
                get;
                set;
            }

            public const string PublicReadOnlyPropertyValue = "Hello";

            public string PublicReadOnlyProperty
            {
                get { return PublicReadOnlyPropertyValue; }
            }

            private string PrivateProperty { get; set; }
        }

        [TestMethod]
        public void GetValue_WithPublicProperty_ReturnsExpectedValue()
        {
            // Arrange
            const string TestValue = "Rawr";
            var obj = new TestPropertyObject() { PublicProperty = TestValue };

            var type = obj.GetType();
            var descriptor = new PropertyPropertyDescriptor( type.GetProperty( "PublicProperty" ) );

            // Act
            object value = descriptor.GetValue( obj );

            // Assert
            Assert.AreEqual( TestValue, value );
        }

        [TestMethod]
        public void GetValue_WithPublicReadOnlyProperty_ReturnsExpectedValue()
        {
            // Arrange;
            var obj = new TestPropertyObject();
            var type = obj.GetType();
            var descriptor = new PropertyPropertyDescriptor( type.GetProperty( "PublicReadOnlyProperty" ) );

            // Act
            object value = descriptor.GetValue( obj );

            // Assert
            Assert.AreEqual( TestPropertyObject.PublicReadOnlyPropertyValue, value );
        }

        [TestMethod]
        public void SetValue_WithPublicProperty_WorksAsExpected()
        {
            // Arrange
            var obj = new TestPropertyObject();

            var type = obj.GetType();
            var descriptor = new PropertyPropertyDescriptor( type.GetProperty( "PublicProperty" ) );

            // Act
            const string TestValue = "Rawr";
            descriptor.SetValue( obj, TestValue );

            // Assert
            object value = descriptor.GetValue( obj );
            Assert.AreEqual( TestValue, value );
        }

        [TestMethod]
        public void SetValue_WithReadOnlyProperty_Throws()
        {
            // Arrange
            var obj = new TestPropertyObject();
            var type = obj.GetType();
            var descriptor = new PropertyPropertyDescriptor( type.GetProperty( "PublicReadOnlyProperty" ) );

            // Act & Assert
            CustomAssert.Throws<System.InvalidOperationException>(
                () => {
                    descriptor.SetValue( obj, "Bye" );
                }
            );
        }

        [TestMethod]
        public void IsReadOnly_WithReadOnlyProperty_ReturnsTrue()
        {
            // Arrange
            var obj = new TestPropertyObject();
            var type = obj.GetType();
            var descriptor = new PropertyPropertyDescriptor( type.GetProperty( "PublicReadOnlyProperty" ) );

            // Assert
            Assert.IsTrue( descriptor.IsReadOnly );
        }

        [TestMethod]
        public void IsReadOnly_WithNonReadOnlyProperty_ReturnsFalse()
        {
            // Arrange
            var obj = new TestPropertyObject();
            var type = obj.GetType();
            var descriptor = new PropertyPropertyDescriptor( type.GetProperty( "PublicProperty" ) );

            // Assert
            Assert.IsFalse( descriptor.IsReadOnly );
        }

        [TestMethod]
        public void PropertyType_ReturnsExpectedType()
        {
            // Arrange
            var descriptor = new PropertyPropertyDescriptor( typeof( TestPropertyObject ).GetProperty( "PublicProperty" ) );

            // Assert
            Assert.AreEqual( typeof( string ), descriptor.PropertyType );
        }

        [TestMethod]
        public void Creation_WithPrivateProperty_Throws()
        {
            CustomAssert.Throws<System.ArgumentException>(
                () => {
                    var descriptor = new PropertyPropertyDescriptor( typeof( TestPropertyObject ).GetProperty( "PrivateProperty" ) );
                }
            );
        }
    }
}
