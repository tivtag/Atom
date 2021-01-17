// <copyright file="TupleItem1ComparerTest.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>Defines the Atom.Collections.Comparers.Tests.TupleItem1ComparerTest class.</summary>
// <author>Paul Ennemoser</author>

namespace Atom.Collections.Comparers.Tests
{
    using System;
    using System.Collections.Generic;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests the usage of the <see cref="TupleItem1Comparer{TFirst, TSecond}"/> class.
    /// </summary>
    [TestClass]
    public sealed class TupleItem1ComparerTest
    {
        [TestMethod]
        public void Sorts_By_FirstValue_OfPair()
        {
            // Arrange
            var pairNull = new Tuple<string, int>( null, 0 );
            var pairA    = new Tuple<string, int>( "A", 3 );
            var pairB    = new Tuple<string, int>( "B", 1 );
            var pairC    = new Tuple<string, int>( "C", -11 );

            var list = new List<Tuple<string, int>>();
            list.Add( pairB );
            list.Add( pairA );
            list.Add( null );
            list.Add( pairNull );
            list.Add( pairC );
            list.Add( pairNull );

            var comparer = new TupleItem1Comparer<string, int>();
 
            // Act
            list.Sort( comparer );

            // Assert
            Assert.AreEqual( null, list[0] );
            Assert.AreEqual( pairNull, list[1] );
            Assert.AreEqual( pairNull, list[2] );
            Assert.AreEqual( pairA, list[3] );
            Assert.AreEqual( pairB, list[4] );
            Assert.AreEqual( pairC, list[5] );
        }
    }
}
