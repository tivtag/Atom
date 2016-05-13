using Microsoft.Pex.Framework.Validation;
using System;
// <copyright file="EnumerableExtensionsTests.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>Defines the Atom.Collections.Tests.EnumerableExtensionsTests class.</summary>
// <author>Paul Ennemoser (Tick)</author>

namespace Atom.Collections.Tests
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using Microsoft.Pex.Framework;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests the usage of the <see cref="EnumerableExtensions"/> class.
    /// </summary>
    [TestClass]
    public sealed partial class EnumerableExtensionsTests
    {
        [PexMethod]
        public void IndexOf_WithPredicateAlwaysReturningFalse_ReturnsMinusOne<T>( [PexAssumeNotNull]IEnumerable<T> sequence )
        {
            // Act
            int index = sequence.IndexOf( item => false );

            // Assert
            Assert.AreEqual( -1, index );
        }

        [PexMethod]
        public void IndexOf_OnNonEmptySequence_WithPredicateAlwaysReturningTrue_ReturnsZero<T>( [PexAssumeNotNull]IList<T> sequence )
        {
            // Assume
            PexAssume.IsTrue( sequence.Count > 0 );

            // Act
            int index = sequence.IndexOf( item => true );

            // Assert
            Assert.AreEqual( 0, index );
        }

        [TestMethod]
        public void IndexOf_OnEmptySequence_WithPredicateAlwaysReturningTrue_ReturnsMinusOne()
        {
            // Arrange
            var sequence = new List<int>();

            // Act
            int index = sequence.IndexOf( item => true );

            // Assert
            Assert.AreEqual( -1, index );
        }

        [TestMethod]
        public void IndexOf_OnNullSequence_Throws()
        {
            CustomAssert.Throws<ArgumentNullException>( 
                () => {
                    EnumerableExtensions.IndexOf<int>( null, item => false );
                } 
            );
        }

        [TestMethod]
        public void IndexOf_WithNullPredicate_Throws()
        {
            CustomAssert.Throws<ArgumentNullException>(
                () => {
                    EnumerableExtensions.IndexOf<int>( new List<int>(), null );
                }
            );
        }

        [PexMethod]
        public void IndexOf_WithSpecificList_AndSpecificPredicate_ReturnsTrue( int count, int itemToSearch )
        {
            // Assume
            PexAssume.IsTrue( count > 0 );
            PexAssume.IsTrue( itemToSearch >= 0 && itemToSearch < count );

            // Arrange
            var sequence = Enumerable.Range( 0, count );

            // Act
            int index = sequence.IndexOf( element => element == itemToSearch ); 

            // Assert
            Assert.AreEqual( index, itemToSearch );
        }

        [PexMethod]
        public void Contains_Self_ReturnsTrue<T>( [PexAssumeNotNull]IEnumerable<T> enumerable )
        {
            Assert.IsTrue( enumerable.Contains( enumerable ) );
        }

        [PexMethod]
        public void Contains_EnumerableMinusOneElement_Enumerable_ReturnsFalse<T>( [PexAssumeNotNull]IEnumerable<T> enumerable )
        {
            // Assume
            PexAssume.IsTrue( enumerable.Count() > 0 );
            PexAssume.IsTrue( enumerable.HasDistinctElements() );

            // Assert
            Assert.IsFalse( enumerable.Skip( 1 ).Contains( enumerable ) );
        }

        [PexMethod]
        public void Contains_Enumerable_EnumerableMinusOneElement_ReturnsTrue<T>( [PexAssumeNotNull]IEnumerable<T> enumerable )
        {
            // Assume
            PexAssume.IsTrue( enumerable.Count() > 0 );
            PexAssume.IsTrue( enumerable.HasDistinctElements() );

            // Assert
            Assert.IsTrue( enumerable.Contains( enumerable.Skip( 1 ) ) );
        }

        [PexMethod]
        public void ElementsEqual_WhenPassedDifferentlySizedSequences_ReturnsFalse<T>(
            [PexAssumeNotNull]IEnumerable<T> sequence,
            [PexAssumeNotNull]IEnumerable<T> otherSequence )
        {
            // Assume
            PexAssume.AreNotEqual( sequence.Count(), otherSequence.Count() );

            // Act
            bool elementEqual = sequence.ElementsEqual( otherSequence );

            // Assert
            Assert.IsFalse( elementEqual );
        }

        [PexMethod]
        public void ElementsEqual_WhenPassedSameSequences_ReturnsTrue<T>(
            [PexAssumeNotNull]IEnumerable<T> sequence )
        {
            // Act
            bool elementEqual = sequence.ElementsEqual( sequence );

            // Assert
            Assert.IsTrue( elementEqual );
        }

        [PexMethod]
        public void ElementsEqual_WhenPassedSameSizedSequencesButDifferentValue_ReturnsFalse<T>(
            [PexAssumeNotNull]IList<T> sequence,
            T value,
            int index )
        {
            // Assume
            PexAssume.IsTrue( sequence.Count > 1 );
            PexAssume.IsTrue( index >= 0 && index < sequence.Count );
            PexAssume.AreNotEqual( value, sequence[index] );

            // Arrange
            var otherSequence = new List<T>( sequence );
            otherSequence[index] = value;

            // Act
            bool elementEqual = sequence.ElementsEqual( otherSequence );

            // Assert
            Assert.IsFalse( elementEqual );
        }

        [PexMethod]
        public void HasDistinctElements_WhenAnyElementIsEqual_ReturnsFalse<T>(
            [PexAssumeNotNull]IList<T> sequence,
            int indexA,
            int indexB )
        {
            // Assume
            PexAssume.AreNotEqual( indexA, indexB );
            PexAssume.IsTrue( indexA >= 0 && indexA < sequence.Count );
            PexAssume.IsTrue( indexB >= 0 && indexB < sequence.Count );
            PexAssume.AreEqual( sequence[indexA], sequence[indexB] );

            // Act
            bool isDistinct = sequence.HasDistinctElements();

            // Assert
            Assert.IsFalse( isDistinct );
        }

        [PexMethod]
        public void ContainsAny_NonEmptyEnumeration_AgainstSameEnumeration_ReturnsTrue<T>(
            [PexAssumeNotNull]IEnumerable<T> elements )
        {
            // Assume
            PexAssume.IsTrue( elements.Count() > 0 );

            // Act
            bool containsAny = elements.ContainsAny( elements );

            // Assert
            Assert.IsTrue( containsAny );
        }

        [PexMethod, PexAllowedException( typeof( ArgumentNullException ) )]
        public void ContainsAny_EmptyEnumeration_ReturnsFalse<T>( IEnumerable<T> valuesToSearchFor )
        {
            // Arrange
            IEnumerable<T> emptyEnumerable = new List<T>();

            // Assert
            Assert.IsFalse( emptyEnumerable.ContainsAny( valuesToSearchFor ) );
        }

        [PexMethod, PexAllowedException( typeof( ArgumentNullException ) )]
        public void Concat_WithSameTypes_WorksAsExpected( IEnumerable<string> sequenceA, IEnumerable<string> sequenceB )
        {
            // Assume
            if( sequenceA != null )
            {
                PexAssume.IsTrue( sequenceA.HasDistinctElements() );
            }

            if( sequenceB != null )
            {
                PexAssume.IsTrue( sequenceB.HasDistinctElements() );
            }

            // Act
            IEnumerable<string> result = sequenceA.Concat<string, string, string>( sequenceB );
            
            // Assert
            Assert.IsNotNull( result );
            Assert.IsTrue( result.Take( sequenceA.Count() ).ElementsEqual( sequenceA ) );
            Assert.IsTrue( result.Skip( sequenceA.Count() ).ElementsEqual( sequenceB ) );
        }

        [PexMethod, PexAllowedException( typeof( ArgumentNullException ) )]
        public void Concat_WithConverters_WorksAsExpected( IEnumerable<float> sequenceA, IEnumerable<double> sequenceB )
        {
            // Assume
            if( sequenceA != null )
            {
                PexAssume.IsTrue( sequenceA.HasDistinctElements() );
            }

            if( sequenceB != null )
            {
                PexAssume.IsTrue( sequenceB.HasDistinctElements() );
            }

            // Act
            IEnumerable<int> result = sequenceA.Concat<int, float, double>( sequenceB, FloatToInt, DoubleToInt );
            
            // Assert
            Assert.IsNotNull( result );
            Assert.IsTrue( result.Take( sequenceA.Count() ).ElementsEqual( sequenceA.Cast<float, int>( FloatToInt ) ) );
            Assert.IsTrue( result.Skip( sequenceA.Count() ).ElementsEqual( sequenceB.Cast<double, int>( DoubleToInt ) ) );
        }

        private static int FloatToInt( float value )
        {
            return (int)value;
        }

        private static int DoubleToInt( double value )
        {
            return (int)value;
        }
    }
}
