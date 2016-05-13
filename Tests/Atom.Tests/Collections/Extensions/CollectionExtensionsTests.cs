// <copyright file="CollectionExtensionsTests.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Collections.Tests.CollectionExtensionsTests class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Collections.Tests
{
    using System.Collections.Generic;
    using Microsoft.Pex.Framework;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests the usage of the <see cref="CollectionExtensions"/> class.
    /// </summary>
    [TestClass]
    public sealed partial class CollectionExtensionsTests
    {
        [PexMethod]
        public void AddRange_CollectionContains_AddedItems<T>(
            [PexAssumeNotNull]List<T> list,
            [PexAssumeNotNull]IEnumerable<T> itemsToAdd )
        {
            // Arrange
            var collection = (ICollection<T>)list;

            // Assume
            PexAssume.IsFalse( collection.Contains( itemsToAdd ) );

            // Act
            collection.AddRange( itemsToAdd );

            // Assert
            Assert.IsTrue( collection.Contains( itemsToAdd ) );
        }
    }
}
