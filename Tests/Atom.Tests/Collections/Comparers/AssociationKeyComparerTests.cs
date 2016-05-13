// <copyright file="AssociationKeyComparerTests.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>Defines the Atom.Collections.Comparers.Tests.AssociationKeyComparerTests class.</summary>
// <author>Paul Ennemoser (Tick)</author>

namespace Atom.Collections.Comparers.Tests
{
    using System.Collections.Generic;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests the usage of the <see cref="AssociationKeyComparerTests{TKey, TValue}"/> class.
    /// </summary>
    [TestClass]
    public sealed class AssociationKeyComparerTests
    {
        [TestMethod]
        public void Sorts_ByKey()
        {
            // Arrange
            var pairNull = new Association<string, int>( null, 8 );
            var pairA    = new Association<string, int>( "A", 1 );
            var pairB    = new Association<string, int>( "B", 4 );
            var pairC    = new Association<string, int>( "C", 2 );

            var list = new List<Association<string, int>>();
            list.Add( pairB );
            list.Add( pairA );
            list.Add( pairNull );
            list.Add( pairC );
            list.Add( null );
            list.Add( pairNull );
            
            var comparer = new AssociationKeyComparer<string, int>();
            
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
