// <copyright file="ReverseComparerTests.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>Defines the Atom.Collections.Comparers.Tests.ReverseComparerTests class.</summary>
// <author>Paul Ennemoser (Tick)</author>

namespace Atom.Collections.Comparers.Tests
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Pex.Framework;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests the usage of the <see cref="ReverseComparer{T}"/> class.
    /// </summary>
    [TestClass]
    public sealed partial class ReverseComparerTests
    {
        [TestMethod]
        public void Throws_OnCreation_WhenPassed_NullComparer()
        {
            CustomAssert.Throws<ArgumentNullException>(
                () => {
                    new ReverseComparer<int>( null );
                }
            );
        }

        [TestMethod]
        public void Throws_WhenSetting_Comparer_ToNull()
        {
            var comparer = new ReverseComparer<int>();

            CustomAssert.Throws<ArgumentNullException>(
                () => {
                    comparer.Comparer = null;
                }
            );
        }

        [TestMethod]
        public void Comparer_CanBeChangedAfterCreation()
        {
            // Arrange
            var comparer = new ReverseComparer<int>();
            var newComparer = new System.Collections.Generic.Moles.SComparer<int>();

            // Act
            comparer.Comparer = newComparer;
                
            // Assert
            Assert.AreEqual( newComparer, comparer.Comparer );
        }

        [PexMethod]
        public void Compare_AlwaysReturns_0_When_Comparing_SameElement<T>( T value )
        {
            // Arrange
            var comparer = new ReverseComparer<T>();

            // Act
            int compareValue = comparer.Compare( value, value );

            // Assert
            Assert.AreEqual( 0, compareValue );
        }

        [PexMethod]
        public void CompareAB_WithReverseComparer_Returns_Same_As_CompareBA_WithDefaultComparer<T>( T valueA, T valueB )
        {
            // Assume
            PexAssume.AreNotEqual( valueA, valueB );

            // Arrange
            var defaultComparer = Comparer<T>.Default;
            var reverseComparer = new ReverseComparer<T>( defaultComparer );

            // Act
            int defaultValue = defaultComparer.Compare( valueB, valueA );
            int reverseValue = reverseComparer.Compare( valueA, valueB );

            // Assert
            Assert.AreEqual( defaultValue, reverseValue );
        }

        [PexMethod]
        public void Compare_WithReverseComparer_WithReverseComparer_ReturnsSame_AsDefaultComparer<T>( T valueA, T valueB )
        {
            // Assume
            PexAssume.AreNotEqual( valueA, valueB );

            // Arrange
            var defaultComparer = Comparer<T>.Default;
            var reverseComparer = new ReverseComparer<T>( defaultComparer );
            var reverseReverseComparer = new ReverseComparer<T>( reverseComparer );

            // Act
            int defaultValue = defaultComparer.Compare( valueA, valueB );
            int reverseValue = reverseComparer.Compare( valueA, valueB );
            int reverseReverseValue = reverseReverseComparer.Compare( valueA, valueB );
            
            // Assert
            Assert.AreEqual( defaultValue, reverseReverseValue );
            Assert.AreNotEqual( defaultValue, reverseValue );
        }
    }
}
