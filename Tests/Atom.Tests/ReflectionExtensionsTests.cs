// <copyright file="ReflectionExtensionsTests.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Tests.ReflectionExtensionsTests class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Tests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests the usage of the <see cref="ReflectionsExtensions"/> class.
    /// </summary>
    [TestClass]
    public sealed class ReflectionExtensionsTests
    {
        private enum TestEnum
        {
            Value = 25
        }

        private class TestClass
        {
            public const int Id = 100;
            public static readonly TestEnum Enum = TestEnum.Value;
        }

        [TestMethod]
        public void GetConstantValue_WhenGivenCorrectInput_ReturnsCorrectConstantValue()
        {
            Maybe<int> constant = typeof( TestClass ).GetConstantValue<int>( "Id" );
            Assert.AreEqual( constant.Value, 100 );
        }

        [TestMethod]
        public void GetConstantValue_WhenAskedForStaticReadOnly_ReturnsCorrectConstantValue()
        {
            Maybe<TestEnum> constant = typeof( TestClass ).GetConstantValue<TestEnum>( "Enum" );
            Assert.AreEqual( constant.Value, TestEnum.Value );
        }
        
        [TestMethod]
        public void GetConstantValue_WhenAskedForStaticReadOnly_ReturnsCorrectCastedValue()
        {
            Maybe<int> constant = typeof( TestClass ).GetConstantValue<int>( "Enum" );
            Assert.AreEqual( constant.Value, (int)TestEnum.Value );
        }

        [TestMethod]
        public void ImplementsTest()
        {
            Assert.IsTrue( typeof( string ).Implements( typeof( string ) ) );
            Assert.IsTrue( typeof( System.ArgumentException ).Implements( typeof( System.Exception ) ) );

            Assert.IsFalse( typeof( int ).Implements( typeof( string ) ) );
            Assert.IsFalse( typeof( System.Exception ).Implements( typeof( System.ArgumentException ) ) );

            Assert.IsFalse( ReflectionExtensions.Implements( null, null ) );
            Assert.IsFalse( ReflectionExtensions.Implements( null, typeof( string ) ) );
            Assert.IsFalse( ReflectionExtensions.Implements( typeof( string ), null ) );
        }

        [TestMethod]
        public void GetTypeNameTest()
        {
            GetTypeNameTest( typeof( int ) );
            GetTypeNameTest( typeof( string ) );
            GetTypeNameTest( typeof( ChangedValue<int> ) );
            GetTypeNameTest( typeof( ChangedValue<object> ) );
            GetTypeNameTest( typeof( Atom.Diagnostics.ConsoleLog ) );

            CustomAssert.Throws<ArgumentNullException>(
                () => {
                    ReflectionExtensions.GetTypeName( null );
                }
            );
        }

        /// <summary>
        /// Helper method that tests whether the GetTypeName method
        /// for the given Type.
        /// </summary>
        /// <param name="type"></param>
        private void GetTypeNameTest( Type type )
        {
            string typeName = type.GetTypeName();
            CustomAssert.Contains( type.Name, typeName );
            CustomAssert.Contains( type.Assembly.GetName().Name, typeName );

            Type typeFromTypeName = Type.GetType( typeName );
            Assert.AreEqual( type, typeFromTypeName );
        }

        [TestMethod]
        public void GetBestMatchingConstructor_Throws_OnInvalidUse()
        {
            CustomAssert.Throws<ArgumentNullException>(
               () => {
                  ReflectionExtensions.GetBestMatchingConstructor( null, "Huh" );
               }
            );

            CustomAssert.Throws<ArgumentException>(
                () => {
                    typeof( PrivateClass ).GetBestMatchingConstructor( "ThereExistsNoPublicConstructor" );
                }
            );

            CustomAssert.Throws<ArgumentException>(
                () => {
                    typeof( PublicClass ).GetBestMatchingConstructor( "More", "Parameters", "Than", "There Are" );
                }
            );
        }

        [TestMethod]
        public void GetBestMatchingConstructorTest()
        {
            GetBestMatchingConstructorTest( typeof( PublicClass ), (object[])null );
            GetBestMatchingConstructorTest( typeof( PublicClass ), 100 );
            GetBestMatchingConstructorTest( typeof( PublicClass ), 100, 200, 300 );
            GetBestMatchingConstructorTest( typeof( PublicClass ), "Hello", 100, 200 ); 
        }

        /// <summary>
        /// Helper method that tests whether the GetBestMatchingConstructor method
        /// for the given Type and paramters.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="paramters"></param>
        private void GetBestMatchingConstructorTest( Type type, params object[] paramters )
        {
            var constructor = type.GetBestMatchingConstructor( paramters );

            Assert.IsNotNull( constructor );
            Assert.IsTrue( constructor.IsConstructor );
            Assert.AreEqual( paramters != null ? paramters.Length : 0, constructor.GetParameters().Length );
        }

        private sealed class PrivateClass
        {
            private PrivateClass()
            {
            }
        }
        public sealed class PublicClass
        {
            public PublicClass()
            {
            }
            public PublicClass( int parameter1 )
            {
            }
            public PublicClass( int parameter1, int paramter2, int parameter3 )
            {
            }
            public PublicClass( string parameter1, object paramter2, int parameter3 )
            {
            }
        }
    }
}