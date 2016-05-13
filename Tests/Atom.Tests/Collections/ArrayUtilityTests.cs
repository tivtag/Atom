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
    using System;
    using System.Linq;
    using Microsoft.Pex.Framework;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    
    /// <summary>
    /// Tests the usage of the <see cref="ArrayUtilityTests"/> class.
    /// </summary>
    [TestClass]
    public sealed partial class ArrayUtilityTests
    {
        [TestMethod]
        public void RemoveAt_Throws_IfPassedNullArray()
        {
            int[] array = null;

            CustomAssert.Throws<ArgumentNullException>(
                () => {
                    ArrayUtilities.RemoveAt<int>( ref array, 0 );
                }
            );
        }

        [PexMethod]
        public void RemoveAt_Throws_IfIndexIsOutOfRange<T>( [PexAssumeNotNull]T[] array, int index )
        {
            PexAssume.IsTrue( index < 0 || index >= array.Length );
            
            CustomAssert.Throws<ArgumentOutOfRangeException>(
                () => {
                    ArrayUtilities.RemoveAt<T>( ref array, index );
                }
            );
        }

        [PexMethod]
        public void RemoveAt_IndexValueZero_RemovesFirstElement<T>( [PexAssumeNotNull]T[] array )
        {
            // Assume
            PexAssume.IsTrue( array.Length > 0 );
            PexAssume.IsTrue( array.HasDistinctElements() );

            // Arrange
            T first = array[0];
            T[] oldArray = (T[])array.Clone();

            // Act
            ArrayUtilities.RemoveAt( ref array, 0 );

            // Assert
            Assert.AreEqual( oldArray.Length - 1, array.Length );
            CustomAssert.DoesNotContain( first, array );
        }

        [PexMethod]
        public void RemoveAt_ValidIndex_WorksAsExpected<T>( [PexAssumeNotNull]T[] array, int index )
        {
            // Assume
            PexAssume.IsTrue( array.Length > 0 );
            PexAssume.IsTrue( array.HasDistinctElements() );
            PexAssume.IsTrue( index >= 0 && index < array.Length );

            // Arrange
            T[] oldArray = (T[])array.Clone();

            // Act
            ArrayUtilities.RemoveAt( ref array, index );

            // Assert
            Assert.AreEqual( oldArray.Length - 1, array.Length );

            for( int i = 0; i < index; ++i )
            {
                Assert.AreEqual( oldArray[i], array[i] );
            }

            for( int i = index; i < array.Length; ++i )
            {
                Assert.AreEqual( oldArray[i + 1], array[i] );
            }
        }
        
        [TestMethod]
        public void InsertAt_Throws_IfPassedNullArray()
        {
            int[] array = null;

            CustomAssert.Throws<ArgumentNullException>(
                () => {
                    ArrayUtilities.InsertAt<int>( ref array, 0, -1 );
                }
            );
        }

        [PexMethod]
        public void InsertAt_Throws_IfIndexIsOutOfRange<T>( [PexAssumeNotNull]T[] array, int index )
        {
            PexAssume.IsTrue( index < 0 || index >= array.Length );

            CustomAssert.Throws<ArgumentOutOfRangeException>(
                () => {
                    ArrayUtilities.InsertAt<T>( ref array, index, default(T) );
                }
            );
        }

        [PexMethod]
        public void InsertAt_ValidIndex_WorksAsExpected<T>( [PexAssumeNotNull]T[] array, int index, T item )
        {
            // Assume
            PexAssume.IsTrue( array.Length > 0 );
            PexAssume.IsTrue( array.HasDistinctElements() );
            PexAssume.IsTrue( index >= 0 && index < array.Length );
            PexAssume.IsFalse( array.Contains( item ) );

            // Arrange
            T[] oldArray = (T[])array.Clone();

            // Act
            ArrayUtilities.InsertAt( ref array, index, item );

            // Assert
            Assert.AreEqual( oldArray.Length + 1, array.Length );
            Assert.AreEqual( item, array[index] );
            Assert.IsTrue( array.Contains( oldArray ) );
        }
        
        [PexMethod]
        public void InsertAt_ValidIndexGreaterOne_WorksAsExpected<T>( [PexAssumeNotNull]T[] array, int index, T item )
        {
            PexAssume.IsTrue( index > 1 );
            InsertAt_ValidIndex_WorksAsExpected( array, index, item );
        }
    }
}
