// <copyright file="TupleItem2ComparerTest.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>Defines the Atom.Collections.Comparers.Tests.TupleItem2ComparerTest class.</summary>
// <author>Paul Ennemoser (Tick)</author>

namespace Atom.Collections.Comparers.Tests
{
    using System;
    using System.Collections.Generic;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests the usage of the <see cref="TupleItem2Comparer{TSecond, TSecond}"/> class.
    /// </summary>
    [TestClass]
    public sealed class TupleItem2ComparerTest
    {
        [TestMethod]
        public void Sorts_By_SecondValue_OfPair()
        {
            // Arrange
            var pairNull = new Tuple<int, string>( 0, null );
            var pairA    = new Tuple<int, string>( 8, "A" );
            var pairB    = new Tuple<int, string>( 12, "B" );
            var pairC    = new Tuple<int, string>( 100, "C" );

            var list = new List<Tuple<int, string>>();
            list.Add( pairB );
            list.Add( pairA );
            list.Add( null );
            list.Add( pairNull );
            list.Add( pairC );
            list.Add( pairNull );

            var comparer = new TupleItem2Comparer<int, string>();
            
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
