// <copyright file="PairTests.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Collections.Tests.ArrayUtilityTests class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Collections.Tests
{
    using Microsoft.Pex.Framework;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    [PexClass( typeof(Association<,>) )]
    public partial class AssociationTests
    {
        [PexMethod]
        public void GetHashCode_OfEqualAssociations_ReturnSameValues<TKey, TValue>(
            [PexAssumeNotNull]Association<TKey, TValue> association )
        {
            int hashCodeA = association.GetHashCode();
            int hashCodeB = association.GetHashCode();

            Assert.AreEqual( hashCodeA, hashCodeB );
        }

        [PexMethod]
        public void ToString_ReturnsStringThatContains_BothKeyAndValue<TKey, TValue>( 
            [PexAssumeNotNull]Association<TKey, TValue> association )
        {
            PexAssume.IsNotNull( association.Key );
            PexAssume.IsNotNull( association.Value );

            // Act
            string str = association.ToString();

            // Assert
            CustomAssert.Contains( association.Key.ToString(), str );
            CustomAssert.Contains( association.Value.ToString(), str );
        }
        
        [PexMethod]
        public void ToKeyValuePair_HasMatchingValues<TKey, TValue>(
            [PexAssumeNotNull]Association<TKey, TValue> association )
        {
            PexAssume.IsNotNull( association.Key );
            PexAssume.IsNotNull( association.Value );

            // Act
            var pair = association.ToKeyValuePair();

            // Assert
            Assert.AreEqual( association.Key, pair.Key );
            Assert.AreEqual( association.Value, pair.Value );
        }

        [PexMethod]
        public void SetKey_ChangesExpectedValue<TKey, TValue>(
            [PexAssumeNotNull]Association<TKey, TValue> association, TKey newKey )
        {
            var original = new Association<TKey, TValue>( association.Key, association.Value );

            // Act
            association.Key = newKey;

            // Assert
            Assert.AreEqual( newKey, association.Key );
            Assert.AreEqual( original.Value, association.Value );
        }

        [PexMethod]
        public void SetValue_ChangesExpectedValue<TKey, TValue>(
            [PexAssumeNotNull]Association<TKey, TValue> association, TValue newValue )
        {
            var original = new Association<TKey, TValue>( association.Key, association.Value );

            // Act
            association.Value = newValue;

            // Assert
            Assert.AreEqual( newValue, association.Value );
            Assert.AreEqual( original.Key, association.Key );
        }

        [PexMethod]
        public void ToString_WithNullKey_ReturnsStringThatContainsNullConstant( int value )
        {
            // Arrange
            var association = new Association<object, int>( null, value );

            // Act
            var str = association.ToString();

            // Assert
            CustomAssert.Contains( "null", str );
        }

        [PexMethod]
        public void ToString_WithNullValue_ReturnsStringThatContainsNullConstant( int key )
        {
            // Arrange
            var association = new Association<int, object>( key, null );

            // Act
            var str = association.ToString();

            // Assert
            CustomAssert.Contains( "null", str );
        }
    }
}
