// <copyright file="NameOfTests.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Tests.NameOfTests class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Tests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests the usage of the <see cref="NameOf{T}"/> class.
    /// </summary>
    [TestClass]
    public sealed class NameOfTests
    {
        [TestMethod]
        public void NameOfStringLength_ShouldBe_Length()
        {
            // Act
            string name = NameOf<string>.Property( e => e.Length );

            // Assert
            Assert.AreEqual( "Length", name );
        }

        [TestMethod]
        public void NameOf_Throws_WhenPassed_Null()
        {
            CustomAssert.Throws<ArgumentNullException>(
                () => NameOf<string>.Property<int>( null )
            );
        }
    }
}