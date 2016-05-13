// <copyright file="ArrayExtensionsTests.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Collections.Tests.ArrayExtensionsTests class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Collections.Tests
{
    using Microsoft.Pex.Framework;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests the usage of the <see cref="ArrayExtensions"/> class.
    /// </summary>
    [TestClass]
    public sealed partial class ArrayExtensionsTests
    {
        [PexMethod]
        public void ElementsEqual_WithDifferentlySizedArrays_ReturnsFalse<T>( [PexAssumeNotNull]T[] arrayA, [PexAssumeNotNull]T[] arrayB )
        {
            // Assume
            PexAssume.AreNotEqual( arrayA.Length, arrayB.Length );

            // Act
            bool areEqual = arrayA.ElementsEqual( arrayB );

            // Assert
            Assert.IsFalse( areEqual );
        }
    }
}
