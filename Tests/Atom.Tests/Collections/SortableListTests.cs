// <copyright file="SortableListTests.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Collections.Tests.SortableListTests class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Collections.Tests
{
    using System;
    using System.Linq;
    using Microsoft.Pex.Framework;
    using Microsoft.Pex.Framework.Validation;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests the usage of the <see cref="SortableList{T}"/> class.
    /// </summary>
    [TestClass]
    [PexClass( typeof( SortableList<int> ) )]
    public sealed partial class SortableListTests
    {
        [PexMethod]
        public void AddRange_ThenListItemAreSorted( [PexAssumeNotNull]int[] values )
        {
            // Assume
            PexAssume.IsTrue( values.Length > 1 );

            // Arrange
            var list = new SortableList<int>( values.Length );

            // Act
            list.AddRange( values );

            // Assert
            int previousValue = list[0];

            for( int i = 1; i < list.Count; ++i )
            {
                int value = list[i];

                Assert.IsTrue( previousValue <= value );
                previousValue = value;
            }
        }
    }
}
