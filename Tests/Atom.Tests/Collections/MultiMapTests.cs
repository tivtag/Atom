// <copyright file="MultiMapTests.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Collections.Tests.MultiMapTests class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Collections.Tests
{
    using System.Linq;
    using Microsoft.Pex.Framework;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests the usage of the <see cref="MultiMap{,}"/> class.
    /// </summary>
    [TestClass]
    [PexClass( typeof(MultiMap<,> ))]
    public partial class MultiMapTests 
    {
        [PexMethod( MaxBranches=40000, MaxConditions=2000 )]
        public void ContainsElementWhenAdded<TKey, TElement>(
            TKey key,
            TKey otherKey,
            TElement value,
            TElement otherValue )
        {
            // Assume
            PexAssume.AreNotEqual( key, otherKey );
            PexAssume.AreNotEqual( value, otherValue );

            // Setup
            var map = new MultiMap<TKey, TElement>();
            Assert.IsFalse( map.Contains( key ) );
            Assert.IsFalse( map.Contains( key, value ) );
            Assert.IsFalse( map.Contains( otherKey ) );
            Assert.IsFalse( map.Contains( key, otherValue ) );

            map.Add( key, value );

            // Assert
            Assert.IsTrue( map.Contains( key ) );
            Assert.IsTrue( map.Contains( key, value ) );
            Assert.IsFalse( map.Contains( otherKey ) );
            Assert.IsFalse( map.Contains( key, otherValue ) );
        }

        [PexMethod( MaxBranches=40000, MaxConditions=2000 )]
        public void AddsElementsOfSameKeyToSameGrouping<TKey, TElement>(
            TKey keyA,
            TKey keyB,
            TElement valueA,
            TElement valueB )
        {
            // Assume
            PexAssume.AreNotEqual( keyA, keyB );
            PexAssume.AreNotEqual( valueA, valueB );

            // Setup
            var map = new MultiMap<TKey, TElement>();
            map.Add( keyA, valueA );
            map.Add( keyA, valueB );
            map.Add( keyB, valueA );

            // Assert
            Assert.IsTrue( map[keyA].Contains( valueA ) );
            Assert.IsTrue( map[keyA].Contains( valueB ) );
            Assert.IsTrue( map[keyB].Contains( valueA ) );
            Assert.AreEqual( 2, map.Count );
        }

        [PexMethod( MaxConstraintSolverTime=8 )]
        public void RemovesElementCorrectly<TKey, TElement>(
            TKey key,
            TKey otherKey,
            TElement value,
            TElement otherValue )
        {
            // Assume
            PexAssume.AreNotEqual( key, otherKey );
            PexAssume.AreNotEqual( value, otherValue );

            // Setup
            var map = new MultiMap<TKey, TElement>();
            map.Add( key, value );
            map.Add( key, otherValue );
            map.Add( otherKey, value );
            map.Add( otherKey, otherValue );

            // Handle
            Assert.IsTrue( map.Remove( key, value ) );
            Assert.IsTrue( map.Remove( otherKey, otherValue ) );

            // Assert
            Assert.IsFalse( map.Contains( key, value ) );
            Assert.IsFalse( map.Contains( otherKey, otherValue ) );

            Assert.IsTrue( map.Contains( otherKey ) );
            Assert.IsTrue( map.Contains( key ) );
            Assert.IsTrue( map.Contains( otherKey, value ) );
            Assert.IsTrue( map.Contains( key, otherValue ) );
            Assert.AreEqual( 2, map.Count );
        }

        [PexMethod( MaxConstraintSolverTime=8 )]
        public void StaysUnchangedWhenTryingToRemoveNonExistantElement<TKey, TElement>(
            TKey key,
            TKey otherKey,
            TKey nonExistingKey,
            TElement value,
            TElement otherValue )
        {
            // Assume
            PexAssume.AreNotEqual( key, otherKey );
            PexAssume.AreNotEqual( key, nonExistingKey );
            PexAssume.AreNotEqual( otherKey, nonExistingKey );
            PexAssume.AreNotEqual( value, otherValue );

            // Setup
            var map = new MultiMap<TKey, TElement>();
            map.Add( key, value );
            map.Add( otherKey, otherValue );

            // Handle
            Assert.IsFalse( map.Remove( key, otherValue ) );
            Assert.IsFalse( map.Remove( otherKey, value ) );
            Assert.IsFalse( map.Remove( nonExistingKey, value ) );

            // Assert
            Assert.IsTrue( map.Contains( key, value ) );
            Assert.IsTrue( map.Contains( otherKey, otherValue ) );
            Assert.AreEqual( 2, map.Count );
        }

        [PexMethod]
        public void RemovesGroupingWhenAllElementsOfGroupingAreRemoved<TKey, TElement>(
            TKey key,
            TKey otherKey,
            TElement value,
            TElement otherValue )
        {
            // Assume
            PexAssume.AreNotEqual( key, otherKey );
            PexAssume.AreNotEqual( value, otherValue );

            // Setup
            var map = new MultiMap<TKey, TElement>();
            map.Add( key, value );
            map.Add( key, otherValue );
            map.Add( otherKey, value );

            // Handle
            Assert.IsTrue( map.Remove( key, value ) );
            Assert.IsTrue( map.Remove( key, otherValue ) );

            // Assert
            Assert.IsFalse( map.Contains( key ) );
            Assert.IsTrue( map.Contains( otherKey ) );
            Assert.AreEqual( 1, map.Count );
        }

        [PexMethod]
        public void DoesNotRemoveGroupingWhenElementsRemainAfterElementWasRemoved<TKey, TElement>(
            TKey key,
            TKey otherKey,
            TElement value,
            TElement otherValue )
        {
            // Assume
            PexAssume.AreNotEqual( key, otherKey );
            PexAssume.AreNotEqual( value, otherValue );

            // Setup
            var map = new MultiMap<TKey, TElement>();
            map.Add( key, value );
            map.Add( key, otherValue );
            map.Add( otherKey, value );

            // Handle
            Assert.IsTrue( map.Remove( key, value ) );

            // Assert
            Assert.IsTrue( map.Contains( key ) );
            Assert.IsTrue( map.Contains( otherKey ) );
            Assert.AreEqual( 2, map.Count );
        }

        [PexMethod]
        public void TryGetReturnsFalseAndElementsAreNullWhenPassingNonExistingKey<TKey, TElement>(
            TKey key,
            TKey otherKey,
            TKey nonExistingKey,
            TElement value,
            TElement otherValue )
        {
            // Assume
            PexAssume.AreNotEqual( key, otherKey );
            PexAssume.AreNotEqual( key, nonExistingKey );
            PexAssume.AreNotEqual( otherKey, nonExistingKey );
            PexAssume.AreNotEqual( value, otherValue );

            // Setup
            var map = new MultiMap<TKey, TElement>();
            map.Add( key, value );
            map.Add( otherKey, otherValue );

            // Assert
            System.Collections.Generic.IEnumerable<TElement> elements;
            Assert.IsFalse( map.TryGet( nonExistingKey, out elements ) );
            Assert.IsNull( elements );
        }

        [PexMethod]
        public void TryGetReturnsTrueAndCorrectElementsWhenPassingExistingKey<TKey, TElement>(
            TKey key,
            TKey otherKey,
            TElement value,
            TElement otherValue,
            TElement thirdValue )
        {
            // Assume
            PexAssume.AreNotEqual( key, otherKey );
            PexAssume.AreNotEqual( value, otherValue );
            PexAssume.AreNotEqual( value, thirdValue );
            PexAssume.AreNotEqual( otherValue, thirdValue );

            // Setup
            var map = new MultiMap<TKey, TElement>( 2 );
            map.Add( key, value );
            map.Add( key, otherValue );
            map.Add( otherKey, thirdValue );

            // Assert
            System.Collections.Generic.IEnumerable<TElement> elements;
            Assert.IsTrue( map.TryGet( key, out elements ) );
            Assert.IsNotNull( elements );
            CustomAssert.Contains( otherValue, elements );
        }
        
        [PexMethod]
        public void EnumeratesOverAllGroupingsContainingAllElements<TKey, TElement>(
            TKey key,
            TKey otherKey,
            TElement value,
            TElement otherValue,
            TElement thirdValue )
        {
            // Assume
            PexAssume.AreNotEqual( key, otherKey );
            PexAssume.AreNotEqual( value, otherValue );
            PexAssume.AreNotEqual( value, thirdValue );
            PexAssume.AreNotEqual( otherValue, thirdValue );

            // Setup
            var map = new MultiMap<TKey, TElement>( 2 );
            map.Add( key, value );
            map.Add( key, otherValue );
            map.Add( otherKey, otherValue );
            map.Add( otherKey, thirdValue );

            // Assert
            foreach( var grouping in map )
            {
                EnumeratesOverAllGroupingsContainingAllElements_TestGrouping<TKey, TElement>( 
                    key, otherKey, value, otherValue, thirdValue, grouping
                );
            }

            System.Collections.IEnumerable enumerable = map;
            foreach( var grouping in enumerable )
            {
                EnumeratesOverAllGroupingsContainingAllElements_TestGrouping<TKey, TElement>(
                    key, otherKey, value, otherValue, thirdValue, (IGrouping<TKey, TElement>)grouping
                );
            }
        }

        private static void EnumeratesOverAllGroupingsContainingAllElements_TestGrouping<TKey, TElement>(
            TKey key, TKey otherKey, TElement value, TElement otherValue, TElement thirdValue, IGrouping<TKey, TElement> grouping )
        {
            bool isGroupingA = grouping.Key.Equals( key ) && 
                    (grouping.Contains( new TElement[2] { value, otherValue } ) && !grouping.Contains( thirdValue ));
            bool isGroupingB = grouping.Key.Equals( otherKey ) && 
                    (grouping.Contains( new TElement[2] { otherValue, thirdValue } ) && !grouping.Contains( value ));

            Assert.IsTrue( isGroupingA || isGroupingB );
            Assert.IsFalse( isGroupingA && isGroupingB );
        }
    }
}
