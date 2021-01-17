// <copyright file="ReverseComparerTests.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>Defines the Atom.Collections.Comparers.Tests.ReverseComparerTests class.</summary>
// <author>Paul Ennemoser</author>

namespace Atom.Collections.Comparers.Tests
{
    using System;
    using System.Collections.Generic;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests the usage of the <see cref="ReverseComparer{T}"/> class.
    /// </summary>
    [TestClass]
    public sealed partial class ReverseComparerTests
    {
        [TestMethod]
        public void Throws_OnCreation_WhenPassed_NullComparer()
        {
            CustomAssert.Throws<ArgumentNullException>(
                () => {
                    new ReverseComparer<int>( null );
                }
            );
        }

        [TestMethod]
        public void Throws_WhenSetting_Comparer_ToNull()
        {
            var comparer = new ReverseComparer<int>();

            CustomAssert.Throws<ArgumentNullException>(
                () => {
                    comparer.Comparer = null;
                }
            );
        }
    }
}
