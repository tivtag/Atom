// <copyright file="FastMultiMapTests.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Collections.Tests.FastMultiMapTests class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Collections.Tests
{
    using System.Linq;
    using Microsoft.Pex.Framework;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Collections.Generic;

    /// <summary>
    /// Tests the usage of the <see cref="FastMultiMap{TKey, TElement}"/> class.
    /// </summary>
    [TestClass]
    [PexClass( typeof( FastMultiMap<,> ) )]
    public partial class FastMultiMapTests
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
            var map = new FastMultiMap<TKey, TElement>();
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
            var map = new FastMultiMap<TKey, TElement>();
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
            var map = new FastMultiMap<TKey, TElement>();
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
            var map = new FastMultiMap<TKey, TElement>();
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
        public void DoesNotRemoveGroupingWhenAllElementsOfGroupingAreRemoved<TKey, TElement>(
            TKey key,
            TKey otherKey,
            TElement value,
            TElement otherValue )
        {
            // Assume
            PexAssume.AreNotEqual( key, otherKey );
            PexAssume.AreNotEqual( value, otherValue );

            // Setup
            var map = new FastMultiMap<TKey, TElement>();
            map.Add( key, value );
            map.Add( key, otherValue );
            map.Add( otherKey, value );

            // Handle
            Assert.IsTrue( map.Remove( key, value ) );
            Assert.IsTrue( map.Remove( key, otherValue ) );

            // Assert
            Assert.IsTrue( map.Contains( key ) );
            Assert.IsTrue( map.Contains( otherKey ) );
            Assert.AreEqual( 2, map.Count );
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
            var map = new FastMultiMap<TKey, TElement>();
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
            var map = new FastMultiMap<TKey, TElement>();
            map.Add( key, value );
            map.Add( otherKey, otherValue );

            // Handle
            List<TElement> list;
            Assert.IsFalse( map.TryGet( nonExistingKey, out list ) );

            // Assert
            Assert.IsNull( list );

            // Handle
            IMultiMap<TKey, TElement> mapI = map;
            IEnumerable<TElement> enumerable;
            Assert.IsFalse( mapI.TryGet( nonExistingKey, out enumerable ) );

            // Assert
            Assert.IsNull( enumerable );
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
            var map = new FastMultiMap<TKey, TElement>( 2 );
            map.Add( key, value );
            map.Add( key, otherValue );
            map.Add( otherKey, thirdValue );

            // Handle
            System.Collections.Generic.List<TElement> list;
            Assert.IsTrue( map.TryGet( key, out list ) );

            // Assert
            Assert.IsNotNull( list );
            CustomAssert.Contains( value, list );
            CustomAssert.Contains( otherValue, list );
            CustomAssert.DoesNotContain( thirdValue, list );

            // Handle
            IMultiMap<TKey, TElement> mapI = map;
            IEnumerable<TElement> enumerable;
            Assert.IsTrue( mapI.TryGet( key, out enumerable ) );

            // Assert
            Assert.IsNotNull( enumerable );
            CustomAssert.Contains( value, enumerable );
            CustomAssert.Contains( otherValue, enumerable );
            CustomAssert.DoesNotContain( thirdValue, enumerable );
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
            var map = new FastMultiMap<TKey, TElement>( 2 );
            map.Add( key, value );
            map.Add( key, otherValue );
            map.Add( otherKey, otherValue );
            map.Add( otherKey, thirdValue );

            // Assert
            foreach( var grouping in map )
            {
                EnumeratesOverAllGroupingsContainingAllElements_TestGrouping<TElement>( 
                    value, 
                    otherValue, 
                    thirdValue,
                    grouping 
                );
            }

            // Assert
            System.Collections.IEnumerable enumerable = map;
            foreach( var grouping in enumerable )
            {
                EnumeratesOverAllGroupingsContainingAllElements_TestGrouping<TElement>(
                    value,
                    otherValue,
                    thirdValue,
                    (List<TElement>)grouping
                );
            }
        }

        private static void EnumeratesOverAllGroupingsContainingAllElements_TestGrouping<TElement>( 
            TElement value,
            TElement otherValue,
            TElement thirdValue,
            List<TElement> grouping )
        {
            bool isGroupingA = 
                    (grouping.Contains( new TElement[2] { value, otherValue } ) && !grouping.Contains( thirdValue ));
            bool isGroupingB = 
                    (grouping.Contains( new TElement[2] { otherValue, thirdValue } ) && !grouping.Contains( value ));

            Assert.IsTrue( isGroupingA || isGroupingB );
            Assert.IsFalse( isGroupingA && isGroupingB );
        }
    }
}
