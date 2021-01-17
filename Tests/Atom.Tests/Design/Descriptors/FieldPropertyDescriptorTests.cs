// <copyright file="FieldPropertyDescriptorTests.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>Defines the Atom.Design.Descriptors.Tests.FieldPropertyDescriptorTests class.</summary>
// <author>Paul Ennemoser</author>

namespace Atom.Design.Descriptors.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests the usage of the <see cref="FieldPropertyDescriptor"/> class.
    /// </summary>
    [TestClass]
    public sealed class FieldPropertyDescriptorTests
    {
        private sealed class TestObject
        {
            public string PublicField = "Hello";
            private string PrivateField = "Hello";

            /// <summary>
            /// This method exists to remove the warning that "PrivateField" has not been used.
            /// </summary>
            public void Haha()
            {
                System.Console.WriteLine( PrivateField );
            }
        }

        [TestMethod]
        public void GetValue_WithPublicField_ReturnsExpectedValue()
        {
            // Arrange
            const string TestValue = "Rawr";
            var obj = new TestObject() { PublicField = TestValue };
            
            var type = obj.GetType();
            var descriptor = new FieldPropertyDescriptor( type.GetField( "PublicField" ) );

            // Act
            object value = descriptor.GetValue( obj );

            // Assert
            Assert.AreEqual( TestValue, value );
        }

        [TestMethod]
        public void SetValue_WithPublicField_WorksAsExpected()
        {
            // Arrange
            var obj = new TestObject();

            var type = obj.GetType();
            var descriptor = new FieldPropertyDescriptor( type.GetField( "PublicField" ) );

            // Act
            const string TestValue = "Rawr";
            descriptor.SetValue( obj, TestValue );
            
            // Assert
            object value = descriptor.GetValue( obj );
            Assert.AreEqual( TestValue, value );
        }

        [TestMethod]
        public void PropertyType_ReturnsExpectedType()
        {
            // Arrange
            var descriptor = new FieldPropertyDescriptor( typeof(TestObject).GetField( "PublicField" ) );

            // Assert
            Assert.AreEqual( typeof( string ), descriptor.PropertyType );
        }

        [TestMethod]
        public void Creation_WithPrivateField_Throws()
        {
            CustomAssert.Throws<System.ArgumentException>(
                () => {
                    var descriptor = new FieldPropertyDescriptor( typeof( TestObject ).GetField( "PrivateField" ) );
                }
            );
        }
    }
}
